using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceLine : MonoBehaviour
{
    public Material aimTrue;                //Material for if aim is correct (Green)
    public Material aimFalse;               //Material for it aim is incorrect (Blue)
    private bool aimCheck = false;          //Confirms if the cursor is pointed at the current trace line

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aimCheck = Cursor.aimAccurate;

        if (aimCheck == true)
        {
            GetComponent<MeshRenderer>().material = aimTrue;
        }
        else
        {
            GetComponent<MeshRenderer>().material = aimFalse;
        }
    }
}
