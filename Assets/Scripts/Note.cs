using System;
using UnityEngine;
using UnityEngine.U2D;
using Dreamteck.Splines;
using System.Runtime.InteropServices.ComTypes;

public class Note : MonoBehaviour
{
    //Prefabs
    public GameObject targetPrefab;     //A prefab for the 'target' at the front of the note
    public GameObject endPrefab;        //A prefab for the 'end cap' at the end of the note
    public GameObject endMissPrefab;    //A prefab for the 'end cap' at the end of the note when you've missed

    //Materials
    public Material matDefault;         //The default pink material for the spline mesh
    public Material matMissed;          //The alternative blue material for the spline mesh

    //Variables for creating the note parts (target/endcap)
    private Vector3 _start;             //The starting node of the note spline
    private Vector3 _end;               //The last node of the note spline
    private int _index;                 //Manages the total number of nodes within the spline
    private SplineComputer _noteSpline; //A reference to the note spline's spline computer
    private Vector3 _pastTracePosition; //A position just before the end of the last node of the note spline
    private float _aimAngle;            //The angle to aim the end cap so it lines up with the end of the spline
    private GameObject _myEndNote;      //A reference to the end cap game object for later deletion if it needs to be replaced (the player missed the note)
    
    //Variables for changing color
    public bool noteMissed = false;     //Toggled by other scripts if the note has been missed and needs to turn blue
    
    //Variables for clipping the spline
    public bool checkForClip = false;   //Toggles if the spline projector should be turned on; (OFF BY DEFAULT, HEAVY LOAD)

    //Regular Variables
    public GameObject myParentTraceline;
    public static Vector3 notePosition;
    public static Vector3 notePositionRelative;
    private bool _lockMiss = false;

    // Start is called before the first frame update
    void Start()
    {
        //Turn off the spline projector while not in use, projections are expensive
        GetComponent<SplineProjector>().enabled = false;

        //Setup note position
        notePosition = transform.position;

        //Setup note position relative to the parent Trace Line
        //notePositionRelative = transform.position;

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
            if (_myEndNote == null)
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
        GameObject endCapMiss = Instantiate(endMissPrefab) as GameObject;
        _myEndNote = endCapMiss.gameObject;
        endCapMiss.transform.eulerAngles = Vector3.forward * _aimAngle;
        Physics2D.IgnoreCollision(endCapMiss.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        endCapMiss.transform.position = _end;
    }
}
