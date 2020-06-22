using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private bool _isPressed;
    private Vector3 _big;
    private Color _colorSolid;
    private Color _colorTransparent;

    // Start is called before the first frame update
    void Start()
    {
        _big = new Vector3(15, 15, 15);
        _colorSolid = GetComponent<SpriteRenderer>().color;
        _colorTransparent = GetComponent<SpriteRenderer>().color;
        _colorTransparent.a = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("XboxA") == true)
        {
            transform.localScale += _big;
        }
        if (Input.GetButtonUp("XboxA") == true)
        {
            transform.localScale -= _big;
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
