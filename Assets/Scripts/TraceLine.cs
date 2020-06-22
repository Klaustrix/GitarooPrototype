using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceLine : MonoBehaviour
{
    public Material aimTrue;
    public Material aimFalse;

    private bool aimCheck = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aimCheck = Activator.aimAccurate;

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
