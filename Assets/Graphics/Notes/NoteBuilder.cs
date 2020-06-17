using System;
using UnityEngine;
using UnityEngine.U2D;
using Dreamteck.Splines;
using System.Runtime.InteropServices.ComTypes;

public class NoteBuilder : MonoBehaviour
{
    public GameObject targetPrefab;
    public GameObject endPrefab;

    private Vector3 _start;
    private Vector3 _end;
    private int _index;
    private Quaternion _endRotation;

    // Start is called before the first frame update
    void Start()
    {
        SplineComputer _spline = GetComponent<SplineComputer>();
        SplinePoint[] points = _spline.GetPoints();
        //Set the first node as the back of the note
        _start = points[0].position;

        //Set the last node as the front of the note
        _index = points.Length -1;
        _end = points[_index].position;

        //Create the target circle and the cap
        BuildNote(points);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BuildNote(SplinePoint[] points)
    {
        //Create an instance of the target circle
        GameObject target = Instantiate(targetPrefab) as GameObject;
        //Place the target circle at one end of the note
        target.transform.position = _start;

        //Create an instance of the end cap
        GameObject endCap = Instantiate(endPrefab) as GameObject;
        //Place the end cap at the other end of the note
        endCap.transform.position = _end;

        //Calculate the spline angle for the end cap's rotation
        float c = Mathf.Atan2((points[_index].position.y), (points[_index].position.x));
        c = c * Mathf.Rad2Deg;

        //Apply rotation to the end cap's rotation transformation
        endCap.transform.rotation = Quaternion.Euler(0,0,c -80);
    }
}
