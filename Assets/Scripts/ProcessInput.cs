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

    public Vector2 moveDirection;       //The x and y values of the analogue stick
    public float smoothInputSpeed;
    public Vector2 stickInput;
    float deadzone = 0.25f;

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
        aimDirection = value.Get<Vector2>();
    }
    void OnPause(InputValue value)
    {
        //Instructions

    }

    //Control Inputs - Edit
    void OnMoveCursor(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
        smoothInputSpeed = 0.01f;

        stickInput = new Vector2(moveDirection.x, moveDirection.y);
        if (stickInput.magnitude < deadzone)
            stickInput = Vector2.zero;
        else
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));
    }
}

