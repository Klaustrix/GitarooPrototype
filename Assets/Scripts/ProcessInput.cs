using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//This code handles input from the new Input System
public class ProcessInput : MonoBehaviour
{
    private PlayerInput _playerInput;   //A reference to the player input object for the scene
    public Vector2 aimDirection;        //The x and y values of the analogue stick
    public bool attackPressed;          //Toggles on if the button is being pressed
    public bool upPressed;              //
    public bool downPressed;            //
    public bool leftPressed;            //
    public bool rightPressed;           //
    public bool selectPressed;          //
    public bool backPressed;            //
        
    private void Start()
    {
        //A reference to the player input object for the scene
        _playerInput = GameObject.Find("SceneInput").GetComponent<PlayerInput>();

        //Identify the current scene to adjust which action map to use
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        //Check the scene and load the appropriate action map
        if (sceneName == "TitleScreen" || sceneName == "SongMenu" || sceneName == "Results")
        {
            _playerInput.SwitchCurrentActionMap("Menu");
        }
        if (sceneName == "Gameplay")
        {
            _playerInput.SwitchCurrentActionMap("Battle");
        }
        if (sceneName == "Editor")
        {
            _playerInput.SwitchCurrentActionMap("Edit-Atk");
        }
    }

    //Control Inputs - Menu
    void OnUp(InputValue value)
    {
        upPressed = value.isPressed;
    }
    void OnDown(InputValue value)
    {
        downPressed = value.isPressed;
    }
    void OnLeft(InputValue value)
    {
        leftPressed = value.isPressed;
    }
    void OnRight(InputValue value)
    {
        rightPressed = value.isPressed;
    }
    void OnSelect(InputValue value)
    {
        selectPressed = value.isPressed;
    }
    void OnBack(InputValue value)
    {
        backPressed = value.isPressed;
    }

    //Control Inputs - Battle
    void OnDefendUp(InputValue value)
    {
        //Instructions

    }
    void OnDefendDown(InputValue value)
    {
        //Instructions

    }
    void OnDefendLeft(InputValue value)
    {
        //Instructions

    }
    void OnDefendRight(InputValue value)
    {
        //Instructions

    }
    void OnAttack(InputValue value)
    {
        attackPressed = value.isPressed;
    }
    void OnAim(InputValue value)
    {
        //Record the x and y values of the analogue stick
        aimDirection = value.Get<Vector2>();
    }
    void OnPause(InputValue value)
    {
        //Instructions

    }

    //Control Inputs - Edit - Attack Mode
    void OnTraceline(InputValue value)
    {
        //Instructions

    }
    void OnNote(InputValue value)
    {
        //Instructions

    }
    void OnForward(InputValue value)
    {
        //Instructions

    }
    void OnBackward(InputValue value)
    {
        //Instructions

    }
    void OnRotateCW(InputValue value)
    {
        //Instructions

    }
    void OnRotateCCW(InputValue value)
    {
        //Instructions

    }
    void OnModify(InputValue value)
    {
        //Instructions

    }
    void OnCycleNoteTimingUp(InputValue value)
    {
        //Instructions

    }
    void OnCycleNoteTimingDown(InputValue value)
    {
        //Instructions

        
    }
    void OnChangeGameMode(InputValue value)
    {
        //Instructions

    }
    void OnSplit(InputValue value)
    {
        //Instructions

    }
    void OnMenu(InputValue value)
    {
        //Instructions

    }

    //Control Inputs - Edit - Defense Mode
    void OnCreateNorthNote(InputValue value)
    {
        //Instructions

    }
    void OnCreateSouthNote(InputValue value)
    {
        //Instructions

    }
    void OnCreateEastNote(InputValue value)
    {
        //Instructions

    }
    void OnCreateWestNote(InputValue value)
    {
        //Instructions

    }
}

