using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Path;
using UnityEngine;

public class Activator : MonoBehaviour
{
    //Variables for notes
    GameObject noteObject;
    private bool _active = false;           //Toggles if currently in contact with a note object - Not really used
    private bool _noteWindow = false;       //Toggles while in contact with the note target
    private bool _noteActive = false;       //Toggles if you successfully hit a note within the window
    private bool _noteEndWindow = false;    //Toggles while in contact with the note end cap

    //Varivables for fan rotation
    private float _fanAngle;
    private Vector3 _fanVector; //Unused

    //Variables for aiming the fan at the trace line
    private SplineComputer _activeTraceSpline;
    private SplineFollower _follower;
    private double _currentTracePosition;
    private Vector3 _futureTracePosition;
    public static float aimAngle;
    public static bool aimAccurate = false;
    public bool aimAssist = false;

    //Variables for counting score and note accuracy
    public static int songScore = 0;
    public static int missedNotes = 0;
    public static int okNotes = 0;
    public static int goodNotes = 0;
    public static int greatNotes = 0;

    // Start is called before the first frame update
    void Start()
    {
        _activeTraceSpline = GameObject.Find("TraceLine").GetComponent<SplineComputer>();
        _follower = GetComponent<SplineFollower>();

    }

    // Update is called once per frame
    void Update()
    {
        FanControl();
        InputControl();
    }

    private void FanControl()
    {
        //------------------------FAN ROTATION------------------------//
        //The angle the analogue stick is pointed at in degrees
        _fanAngle = Mathf.Atan2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * Mathf.Rad2Deg;

        //A vector position derived from the x and y axis of the analogue stick
        _fanVector = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0);

        //------------------------FIND DIRECTION------------------------//
        //The total % travelled along the current spline being followed
        _currentTracePosition = _follower.result.percent;

        //The vector position of a future % point along the current active trace spline
        _futureTracePosition = _activeTraceSpline.EvaluatePosition(_currentTracePosition + 0.001);

        //The vector position of the future point - the current point on the spline
        Vector3 _aimVector = _futureTracePosition - transform.position;

        //The angle between the current and future points on the spline in degrees
        float _aimAngle = Mathf.Atan2(_aimVector.y, _aimVector.x) * Mathf.Rad2Deg;

        //A quaternion rotation to the exact angle the spline is being travelled along
        Quaternion _aimQuat = Quaternion.AngleAxis(_aimAngle, Vector3.forward);

        //------------------------CHECK AIM------------------------//
        //Get the angle between the fan's angle and the angle of the spline
        float _aimTest = Mathf.DeltaAngle(_fanAngle, _aimAngle);

        //Test to see if the analogue is pointing the fan within 40 degrees +/- of the spline position
        if (_aimTest > -40 && _aimTest < 40)
        {
            //If true, tell the TraceLine script to turn green
            aimAccurate = true;

            //If aim assist is active, follow the spline exactly while within this range
            if (aimAssist == true)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, _aimQuat, Time.deltaTime * 20);
            }
            //Otherwise just use the current direction the analogue is being aimed to rotate the fan object
            else
            {
                transform.eulerAngles = Vector3.forward * _fanAngle;
            }

        }
        //If you aren't pointed at the line...
        else
        {
            //Tell the trace line to turn blue
            aimAccurate = false;

            //Soft miss
            ResetNote(1);

            //Use the analogue rotation for the fan rotation
            transform.eulerAngles = Vector3.forward * _fanAngle;
        }
    }

    private void InputControl()
    {
        //Add support for all buttons - ABXY

        //When you press the A button
        if (Input.GetButtonDown("XboxA") == true)
        {
            //If you are aiming correctly, and are over the note target
            if (_noteWindow == true && aimAccurate == true)
            {
                //Register that you are holding down an active note
                _noteActive = true;

                //Close the window for hitting the target
                _noteWindow = false;

                //Gain score - adjust later to be based on accuracy
                ScoreCounter("Hit");
            }
        }

        //While holding the A button...
        if (Input.GetButton("XboxA") == true)
        {
            //and you are holding down on a hit note
            if (_noteActive == true && aimAccurate == true)
            {
                //GainLife in Charge Mode
                //DealDamage in Attack Mode
            }
        }

        //When you release the A button...
        if (Input.GetButtonUp("XboxA") == true)
        {
            //And you held the note to the end
            if (_noteActive == true && aimAccurate == true && _noteEndWindow == true)
            {
                //Reset the note
                ResetNote(0);
            }
            //And you let go early
            if (_noteActive == true && aimAccurate == true && _noteEndWindow == false)
            {
                //Soft Miss
                ResetNote(1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When you arrive at a note target...
        if (collision.gameObject.tag == "NoteTarget" && aimAccurate == true)
        {
            //Make the note related code active and open the window for hitting the target
            _active = true;
            _noteWindow = true;
        }

        //When you arrive at the note record the current note for use elsewhere
        if (collision.gameObject.tag == "Note")
        {
            noteObject = collision.gameObject;
        }

        //When you arrive at a note end cap...
        if (collision.gameObject.tag == "NoteEnd")
        {
            _noteEndWindow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NoteTarget" ||
            collision.gameObject.tag == "NoteEnd")
        {
            Destroy(collision.gameObject);

            //If you reach the end of the note target...
            if (collision.gameObject.tag == "NoteTarget")
            {
                //Close the window for hitting the target
                _noteWindow = false;

                //If you didn't hit the target in time, register as a miss
                if (_noteActive != true)
                {
                    //Register note miss and reset note flags, null note object
                    ResetNote(2);
                }
            }

            //If you reach the end of a note...
            if (collision.gameObject.tag == "NoteEnd")
            {
                //Close the window for safely releasing the button
                _noteEndWindow = false;
                //Do a soft reset just incase any other end states didn't get caught?
                //ResetNote(0);
            }
        }
    }

    private void ResetNote(int _resetType)
    {
        //Reset Note Only
        if (_resetType >= 0)
        {
            _active = false;
            _noteActive = false;

            //Soft Miss + Turn note blue
            if (_resetType >= 1)
            {
                if (noteObject != null)
                {
                    noteObject.GetComponent<Note>().noteMissed = true;
                }

                //Hard Miss, also shake screen & lose life
                if (_resetType >= 2)
                {
                    //Screen Shake
                    //LoseLife()
                    ScoreCounter("Miss");
                }
            }

            noteObject = null;
        }
    }

    private void ScoreCounter(string type)
    {
        //Need to do a lot more work here to be properly working

        if (type == "Miss")
        {
            missedNotes++;
        }

        if (type == "Hit")
        {
            greatNotes++;
            songScore += 100;
        }
    }
}
