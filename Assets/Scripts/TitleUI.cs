using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleUI : MonoBehaviour
{
    private int _cursorPosition = 0;
    private int _timer = 0;
    private int _delay = 300;

    private float _heightMod;
    private float _widthMod;

    // Start is called before the first frame update
    void Start()
    {
        //Initialise player prefs variables
        PlayerPrefs.SetInt("GameMode", 0);
        PlayerPrefs.SetInt("CursorPos", 0);

        //Set a position aligned to the screen
        _heightMod = 12.5f;
        _widthMod = 100f;

    }

    // Update is called once per frame
    void Update()
    {
        if (_timer == 0)
        {
            //Logic to move cursor up
            if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().upPressed == true)
            {
                if (_cursorPosition > 0)
                {
                    _cursorPosition--;
                    _timer = _delay;
                }
            }

            //Logic to move cursor down
            else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().downPressed == true)
            {
                if (_cursorPosition < 3)
                {
                    _cursorPosition++;
                    _timer = _delay;
                }
            }

            //Logic to make a selection from the menu
            else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().selectPressed == true)
            {
                switch (_cursorPosition)
                {
                    //Song Select
                    case 0:
                        //transition to song select with gameMode0
                        PlayerPrefs.SetInt("gameMode", 0);
                        Loader.Load(Loader.Scene.SongMenu);
                        break;
                    //Song Editor
                    case 1:
                        //transition to song select with gameMode1
                        PlayerPrefs.SetInt("gameMode", 1);
                        Loader.Load(Loader.Scene.SongMenu);
                        break;
                    //Settings Menu
                    case 2:
                        //Display the settings menu
                        break;
                    //Exit Game
                    case 3:
                        //Exit the game
                        QuitGame();
                        break;
                }
            }
        }
        else if (_timer >= 1)
        {
            _timer--;
        }

        //Adjust the cursor position
        switch (_cursorPosition)
        {
            case 0:
                transform.position = new Vector2(
                    GameObject.Find("FreePlay").GetComponent<RectTransform>().position.x - _widthMod,
                    GameObject.Find("FreePlay").GetComponent<RectTransform>().position.y + _heightMod);
                break;
            case 1:
                transform.position = new Vector2(
                    GameObject.Find("SongEditor").GetComponent<RectTransform>().position.x - _widthMod,
                    GameObject.Find("SongEditor").GetComponent<RectTransform>().position.y + _heightMod);
                break;
            case 2:
                transform.position = new Vector2(
                    GameObject.Find("Settings").GetComponent<RectTransform>().position.x - _widthMod,
                    GameObject.Find("Settings").GetComponent<RectTransform>().position.y + _heightMod);
                break;
            case 3:
                transform.position = new Vector2(
                    GameObject.Find("Quit").GetComponent<RectTransform>().position.x - _widthMod,
                    GameObject.Find("Quit").GetComponent<RectTransform>().position.y + _heightMod);
                break;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
