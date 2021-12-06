using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Vector3 _big;
    private Color _colorSolid;
    private Color _colorTransparent;
    private bool _primaryInputState = false;
    private bool _lock = false;
    private bool _test = false;

    // Start is called before the first frame update
    void Start()
    {
        _big = new Vector3(15, 15, 15);
        _colorSolid = GetComponent<SpriteRenderer>().color;
        _colorTransparent = GetComponent<SpriteRenderer>().color;
        _colorTransparent.a = 50;
    }

    //This code makes the button bigger while you hold the primary input down
    void Update()
    {
        //Check the SceneInput object for the state of the primary input button
        _primaryInputState = GameObject.Find("SceneInput").GetComponent<ProcessInput>().attackPressed;

        if (_test != _primaryInputState)
        {
            _lock = false;
            _test = _primaryInputState;
        }

        //If A is pressed, increase size
        if (_primaryInputState == true && _lock == false)
        {
            transform.localScale += _big;
            _lock = true;
        }
        //If A is released, decrease size
        if (_primaryInputState == false && _lock == false)
        {
            transform.localScale -= _big;
            _lock = true;
        }

        //THIS SHOUOLD BE CORRECTED TO WHEN THERE IS/ISN'T AN ACTIVE TRACE LINE
        /*if (Activator.aimAccurate == true)
        {
            GetComponent<SpriteRenderer>().color = _colorTransparent;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = _colorSolid;
        }*/
    }
}
