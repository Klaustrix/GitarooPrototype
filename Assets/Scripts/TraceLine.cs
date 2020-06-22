using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceLine : MonoBehaviour
{
    //Materials
    public Material aimTrue;                //Material for if aim is correct (Green)
    public Material aimFalse;               //Material for it aim is incorrect (Blue)

    //Note Children
    public static GameObject[] myNotes;     //An array of the notes assigned to this trace line

    //General
    public static int assignedPhrase;       //The phrase of the song this trace line is assigned to
    public static int traceNumber;          //Which trace line this is withing the phrase
    public static Vector3 centrePosition;   //Public reference to the position of the trace line object
    private bool _aimCheck = false;         //Confirms if the cursor is pointed at the current trace line
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Publicly available centre position of the trace line
        centrePosition = transform.position;

        //Change colour if the cursor is aimed at the trace line
        _aimCheck = Cursor.aimAccurate;

        if (_aimCheck == true)
        {
            GetComponent<MeshRenderer>().material = aimTrue;
        }
        else
        {
            GetComponent<MeshRenderer>().material = aimFalse;
        }
    }
}
