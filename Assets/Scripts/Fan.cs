using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Fan : MonoBehaviour
{
    private Quaternion _aimQuat;    //A rotation to aim straight at the current trace line
    private Vector2 _aim;           //The current analogue x and y values

    //This code rotates the fan to aim in the direction the analogue of pushed
    void Update()
    {
        //Move with the activator
        transform.position = Activator.centrePosition;

        //The angle the analogue stick is pointed at in degrees
        _aim = GameObject.Find("SceneInput").GetComponent<ProcessInput>().aimDirection;
        float _fanAngle = Mathf.Atan2(_aim.y, _aim.x) * Mathf.Rad2Deg;

        //Get the current rotation to aim directly at the current trace line
        _aimQuat = Cursor.aimQuat;

        //If auto-aim or aim-assist are active, follow the spline exactly while within this range
        if (Cursor.aimAccurate == true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _aimQuat, Time.deltaTime * 20);
        }    
        //Otherwise just use the current direction the analogue is being aimed to rotate the fan object
        else
        {
            transform.eulerAngles = Vector3.forward * _fanAngle;
        }
    }
}
