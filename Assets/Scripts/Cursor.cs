using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public static bool aimAssist = false;   //Toggle to turn on snapping to the trace line
    public static float aimAngle;           //Angle between current and future points on the trace line, in degrees
    public static bool aimAccurate = false; //Toggles when you are aimed at the trace line
    public static Quaternion aimQuat;       //A rotation for aiming directly at the current trace line
    private float _cursorAngle;             //Angle the analogue stick is pointed, in degrees
    private GameObject _activatorRef;       //A reference to the activator

    // Start is called before the first frame update
    void Start()
    {
        _activatorRef = GameObject.Find("Activator");
    }

    // Update is called once per frame
    void Update()
    {
        //Move with the activator
        transform.position = Activator.centrePosition;

        //------------------------FIND ROTATION------------------------//
        //The angle the analogue stick is pointed at in degrees
        _cursorAngle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg;

        //------------------------FIND DIRECTION------------------------//
        //The vector position of the future point - the current point on the spline
        Vector3 _aimVector = Activator.futureTracePosition - transform.position;

        //The angle between the current and future points on the spline in degrees
        float _aimAngle = Mathf.Atan2(_aimVector.y, _aimVector.x) * Mathf.Rad2Deg;

        //A quaternion rotation to the exact angle the spline is being travelled along
        aimQuat = Quaternion.AngleAxis(_aimAngle, Vector3.forward);

        //------------------------CHECK AIM------------------------//
        //Get the angle between the fan's angle and the angle of the spline
        float _aimTest = Mathf.DeltaAngle(_cursorAngle, _aimAngle);

        //Test to see if the analogue is pointing the fan within 40 degrees +/- of the spline position
        if (_aimTest > -40 && _aimTest < 40)
        {
            //If true, tell the TraceLine script to turn green
            aimAccurate = true;

            //If aim assist is active, follow the spline exactly while within this range
            if (aimAssist == true)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, aimQuat, Time.deltaTime * 20);
            }
            //Otherwise just use the current direction the analogue is being aimed to rotate the fan object
            else
            {
                transform.eulerAngles = Vector3.forward * _cursorAngle;
            }

        }
        //If you aren't pointed at the line...
        else
        {
            //Tell the trace line to turn blue
            aimAccurate = false;

            //Soft miss
            _activatorRef.GetComponent<Activator>().ResetNote(1);

            //Use the analogue rotation for the fan rotation
            transform.eulerAngles = Vector3.forward * _cursorAngle;
        }
    }
}
