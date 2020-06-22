using System;
using UnityEngine;
using UnityEngine.U2D;
using Dreamteck.Splines;
using System.Runtime.InteropServices.ComTypes;

public class Note : MonoBehaviour
{
    //Public Variables
    public GameObject targetPrefab;
    public GameObject endPrefab;
    public GameObject endMissPrefab;
    public Material matDefault;
    public Material matMissed;
    public bool noteMissed = false;
    public bool checkForClip = false;

    //Instantiation Variables
    private Vector3 _start;
    private Vector3 _end;
    private int _index;
    private SplineComputer _noteSpline;
    private Vector3 _pastTracePosition;
    private float _aimAngle;
    private GameObject _myEndNote;

    //Regular Variables
    private bool _lockMiss = false;

    // Start is called before the first frame update
    void Start()
    {
        //Turn off the spline projector while not in use, projections are expensive
        GetComponent<SplineProjector>().enabled = false;

        //Setup the positions for the first and last nodes in the spline
        _noteSpline = GetComponent<SplineComputer>();
        SplinePoint[] points = _noteSpline.GetPoints();
        _start = points[0].position;
        _index = points.Length -1;
        _end = points[_index].position;
        _end = _end + new Vector3(0, 0, 0);

        //Establish the rotation for the note's end cap
        _pastTracePosition = _noteSpline.EvaluatePosition(0.99);
        Vector3 _aimVector = _pastTracePosition - _end;
        _aimAngle = Mathf.Atan2(_aimVector.y, _aimVector.x) * Mathf.Rad2Deg + 180;

        //Create the target circle and the cap
        BuildNote();
    }

    // Update is called once per frame
    void Update()
    {
        //Enable the projector if the note collides with the fan
        if (checkForClip == true && GetComponent<SplineProjector>().enabled == false)
        {
            GetComponent<SplineProjector>().enabled = true;
        }

        //Begin clipping the note as it passes by the fan position
        if (GetComponent<SplineProjector>().enabled == true)
        {
            _noteSpline.GetComponent<SplineRenderer>().clipFrom = GetComponent<SplineProjector>().result.percent;

            //Delete the note if it's passed the middle
            if (GetComponent<SplineProjector>().result.percent == 1)
            {
                Destroy(gameObject);
            }
        }

        //Load the blue texture and end cap if the note becomes 'missed'
        if (noteMissed == true && _lockMiss == false)
        {
            GetComponent<MeshRenderer>().material = matMissed;
            ReplaceEnd();
            _lockMiss = true; //Prevent multiple spawns of the endcap
        }
    }

    void BuildNote()
    {
        //Create an instance of the target circle
        GameObject target = Instantiate(targetPrefab) as GameObject;
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        target.transform.position = _start;

        //Create an instance of the end cap
        GameObject endCap = Instantiate(endPrefab) as GameObject;
        _myEndNote = endCap.gameObject;
        endCap.transform.eulerAngles = Vector3.forward * _aimAngle;
        Physics2D.IgnoreCollision(endCap.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        endCap.transform.position = _end;
    }

    void ReplaceEnd()
    {
        Destroy(_myEndNote);
        _myEndNote = null;
        GameObject endCapMiss = Instantiate(endMissPrefab) as GameObject;
        endCapMiss.transform.eulerAngles = Vector3.forward * _aimAngle;
        Physics2D.IgnoreCollision(endCapMiss.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        endCapMiss.transform.position = _end;
    }
}
