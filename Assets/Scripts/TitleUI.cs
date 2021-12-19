using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleUI : MonoBehaviour
{
    private int _multiplier = 0;
    private int _timer = 0;
    private int _delay = 300;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_timer == 0)
        {
            //Logic to move cursor up
            if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().upPressed == true)
            {
                if (_multiplier > 0)
                {
                    _multiplier--;
                    _timer = _delay;
                }
            }

            //Logic to move cursor down
            else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().downPressed == true)
            {
                if (_multiplier < 3)
                {
                    _multiplier++;
                    _timer = _delay;
                }
            }

            //Logic to make a selection from the menu
            else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().selectPressed == true)
            {
                switch (_multiplier)
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

            //Logic to move cancel or move back a screen - matters for options menu
            else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().backPressed == true)
            {

            }
        }
        else if (_timer >= 1)
        {
            _timer--;
        }

        //Update the cursor position
        transform.position = new Vector2(5.42f, (-1.07f - (0.93f * _multiplier)));
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
