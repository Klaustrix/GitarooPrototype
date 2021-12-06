using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cursor : MonoBehaviour
{   
    public static bool autoAim = false;     //Toggle to turn on automatic aim towards the trace line
    public static bool aimAssist = false;   //Toggle to turn on snapping to the trace line
    public static float aimAngle;           //Angle between current and future points on the trace line, in degrees
    public static bool aimAccurate = false; //Toggles when you are aimed at the trace line
    public static Quaternion aimQuat;       //A rotation for aiming directly at the current trace line
    
    private float _cursorAngle;             //Angle the analogue stick is pointed, in degrees
    private GameObject _activatorRef;       //A reference to the activator
    private Vector2 _aim;                   //The current analogue x and y values

    // Start is called before the first frame update
    void Start()
    {
        _activatorRef = GameObject.Find("Activator");
    }

    //Check the analogue aim
    void Update()
    {
        //Move with the activator
        transform.position = Activator.centrePosition;

        //FIND THE ROTATION
        //Get the current analogue stick input data from the SceneInput object
        _aim = GameObject.Find("SceneInput").GetComponent<ProcessInput>().aimDirection;
        //Convert the vector to degrees
        _cursorAngle = Mathf.Atan2(_aim.y,_aim.x) * Mathf.Rad2Deg;

        //FIND THE DIRECTION
        //The vector position of the future point - the current point on the spline
        Vector3 _aimVector = Activator.futureTracePosition - transform.position;

        //The angle between the current and future points on the spline in degrees
        float _aimAngle = Mathf.Atan2(_aimVector.y, _aimVector.x) * Mathf.Rad2Deg;

        //A quaternion rotation to the exact angle of the spline that is being travelled along
        aimQuat = Quaternion.AngleAxis(_aimAngle, Vector3.forward);

        //CHECK AIM
        //Get the angle between the fan's angle and the angle of the spline
        float _aimTest = Mathf.DeltaAngle(_cursorAngle, _aimAngle);

        //If auto aim is on, just aim at the trace line automatically
        if (autoAim == true)
        {
            aimAccurate = true;
            transform.rotation = Quaternion.Lerp(transform.rotation, aimQuat, Time.deltaTime * 20);
        }
        //If aim assist is on, snap to the line within +/- 40 degrees
        else
        {
            //Test to see if the analogue is pointing at the trace line within 40 degrees +/- of the spline position
            if (_aimTest > -40 && _aimTest < 40)
            {
                aimAccurate = true;
                
                if (aimAssist == true)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, aimQuat, Time.deltaTime * 20);
                }
                
            }
            //If the analogue is pointing elsewhere, enable miss state and rotate to wherever the analogue is pointing
            else
            {
                //Tell the trace line to turn blue
                aimAccurate = false;

                //Soft miss
                _activatorRef.GetComponent<Activator>().NoteMissLogic(1);

                //Use the analogue rotation for the fan rotation
                transform.eulerAngles = Vector3.forward * _cursorAngle;
            }
        }
    }
}
