using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Fan : MonoBehaviour
{
    private Quaternion _aimQuat;    //A rotation to aim straight at the current trace line

    // Update is called once per frame
    void Update()
    {
        //Move with the activator
        transform.position = Activator.centrePosition;

        //The angle the analogue stick is pointed at in degrees
        float _fanAngle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg;

        //Get the current rotation to aim directly at the current trace line
        _aimQuat = Cursor.aimQuat;

        //If aim assist is active, follow the spline exactly while within this range
        if (Cursor.aimAssist == true)
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
