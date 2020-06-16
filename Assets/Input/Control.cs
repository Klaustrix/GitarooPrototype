using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Control : MonoBehaviour
{
    private Vector3 _cursorAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Store the analogue x and y axis positions in the variable
        _cursorAngle = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 4096);

        //Convert the analogue positions to a quaternion angle
        Quaternion _targetRotation = Quaternion.LookRotation(_cursorAngle, Vector3.forward);

        //Rotate the object to the resulting quaternion angle
        //(delta time is used to control responsiveness, Lerp is used to smooth movement
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, Time.deltaTime * 20);
    }
}
