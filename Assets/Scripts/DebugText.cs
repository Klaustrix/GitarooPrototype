using Packages.Rider.Editor.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    public bool controllerDebug;

    private string _debugText;

    private void Update()
    {
        if (controllerDebug == true)
        {
            ControllerDebugText();
        }
        
    }

    private void ControllerDebugText()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        float htAxis = Input.GetAxis("HorizontalTurn");
        float vtAxis = Input.GetAxis("VerticalTurn");
        float ltaxis = Input.GetAxis("XboxLeftTrigger");
        float rtaxis = Input.GetAxis("XboxRightTrigger");
        float dhaxis = Input.GetAxis("XboxDpadHorizontal");
        float dvaxis = Input.GetAxis("XboxDpadVertical");

        bool xbox_a = Input.GetButton("XboxA");
        bool xbox_b = Input.GetButton("XboxB");
        bool xbox_x = Input.GetButton("XboxX");
        bool xbox_y = Input.GetButton("XboxY");
        bool xbox_lb = Input.GetButton("XboxLB");
        bool xbox_rb = Input.GetButton("XboxRB");
        bool xbox_ls = Input.GetButton("XboxLS");
        bool xbox_rs = Input.GetButton("XboxRS");
        bool xbox_view = Input.GetButton("XboxView");
        bool xbox_menu = Input.GetButton("XboxStart");

        _debugText = string.Format(
                "Horizontal: {14:0.000} Vertical: {15:0.000}\n" +
                "HorizontalTurn: {16:0.000} VerticalTurn: {17:0.000}\n" +
                "LTrigger: {0:0.000} RTrigger: {1:0.000}\n" +
                "A: {2} B: {3} X: {4} Y:{5}\n" +
                "LB: {6} RB: {7} LS: {8} RS:{9}\n" +
                "View: {10} Menu: {11}\n" +
                "Dpad-H: {12:0.000} Dpad-V: {13:0.000}\n",
                ltaxis, rtaxis,
                xbox_a, xbox_b, xbox_x, xbox_y,
                xbox_lb, xbox_rb, xbox_ls, xbox_rs,
                xbox_view, xbox_menu,
                dhaxis, dvaxis,
                hAxis, vAxis,
                htAxis, vtAxis);

        GetComponent<Text>().text = _debugText;
    }
}
