using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Vector3 _cursorAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float _cursorAngle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg;

        //Snap to the line if the analogue is pointed in the right direction
        if (Activator.aimAccurate == true)
        {
            transform.eulerAngles = Vector3.forward * _cursorAngle;
        }
        else
        {
            transform.eulerAngles = Vector3.forward * _cursorAngle;
        }
    }
}
