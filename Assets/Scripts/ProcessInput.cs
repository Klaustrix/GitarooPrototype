using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProcessInput : MonoBehaviour
{
    public Vector2 aimDirection;    //The x and y values of the analogue stick
    public bool attackPressed;      //Toggles on if the primary button is being pressed

    // When the Aim input is activated do this...
    void OnAim(InputValue value)
    {
        //Record the x and y values of the analogue stick
        aimDirection = value.Get<Vector2>();
    }

    void OnAttack(InputValue value)
    {
        attackPressed = value.isPressed;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

