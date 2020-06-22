﻿using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Activator : MonoBehaviour
{
    //General Variables
    public static Vector3 centrePosition;   //A public reference to the position at the centre of the screen

    //Variables for following the current trace line
    public static double currentTracePosition;  //The activator's current position along the trace line
    public static Vector3 futureTracePosition;  //The activator's future position along the current trace line
    private SplineComputer _activeTraceSpline;  //A reference to the spline computer of the current trace line
    private SplineFollower _follower;           //A reference to the follower component

    //Variables for clipping the trace line being followed
    public float noteClipPercent;

    //Variables for note logic
    private GameObject noteObject;          //Contains a reference to the current note gameobject
    private bool _active = false;           //Toggles if currently in contact with a note object - Not really used
    private bool _noteWindow = false;       //Toggles while in contact with the note target
    private bool _noteActive = false;       //Toggles if you successfully hit a note within the window
    private bool _noteEndWindow = false;    //Toggles while in contact with the note end cap

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
        //Publicly available position of the activator
        centrePosition = transform.position;

        //Update public stats for the trace line being followed
        SplineStats();

        //Update controller input
        InputControl();

        //Clip the trace line as it passes over the centre of the screen
        if (_activeTraceSpline != null)
        {
            _activeTraceSpline.GetComponent<SplineRenderer>().clipFrom = currentTracePosition;
        }
    }

    private void SplineStats()
    {
        //The total % travelled along the current spline being followed
        currentTracePosition = _follower.result.percent;

        //The vector position of a future % point along the current active trace spline
        if (currentTracePosition != 1)
        {
            futureTracePosition = _activeTraceSpline.EvaluatePosition(currentTracePosition + 0.001);
        }
    }

    private void InputControl()
    {
        //Add support for all buttons - ABXY

        //When you press the A button
        if (Input.GetButtonDown("XboxA") == true)
        {
            //If you are aiming correctly, and are over the note target
            if (_noteWindow == true && Cursor.aimAccurate == true)
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
            if (_noteActive == true && Cursor.aimAccurate == true)
            {
                //GainLife in Charge Mode
                //DealDamage in Attack Mode
            }
        }

        //When you release the A button...
        if (Input.GetButtonUp("XboxA") == true)
        {
            //And you held the note to the end
            if (_noteActive == true && Cursor.aimAccurate == true && _noteEndWindow == true)
            {
                //Reset the note
                ResetNote(0);
            }
            //And you let go early
            if (_noteActive == true && Cursor.aimAccurate == true && _noteEndWindow == false)
            {
                //Soft Miss
                ResetNote(1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When you arrive at a note target...
        if (collision.gameObject.tag == "NoteTarget" && Cursor.aimAccurate == true)
        {
            //Make the note related code active and open the window for hitting the target
            _active = true;
            _noteWindow = true;
        }

        //When you arrive at the note record the current note for use elsewhere
        if (collision.gameObject.tag == "Note")
        {
            noteObject = collision.gameObject;
            noteObject.GetComponent<Note>().checkForClip = true;
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

    public void ResetNote(int _resetType)
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
