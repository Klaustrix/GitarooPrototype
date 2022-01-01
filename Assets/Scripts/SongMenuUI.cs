using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongMenuUI : MonoBehaviour
{
    public GameObject tilePrefab;       //A prefab for note tiles on the song menu
    public int cursor = 0;              //Manages cursor position in the song list
    public bool cursorMoved = true;      //Changes the currently playing song preview

    private Vector3 _defaultUITilePos;  //The default position for UI tile components
    private bool tilesMade = false;     //Prevent tiles being created more than once
    private int _timer = 0;             //Manages input delay
    private int _delay = 300;           //How many updates to delay any input for

    //The collection of UI tile elements for each song loaded
    public static List<GameObject> tileList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        //Set a position aligned to the screen
        _defaultUITilePos = new Vector3((Screen.width / 2 + (Screen.width / 4)), (Screen.height / 2), 0);

        //Initialise the tile list
        tileList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        //Only generate the tiles once
        if (tilesMade == false)
        {
            //Generate the UI tiles for each song
            foreach (SongPackage newPackage in GameObject.Find("SongManager").GetComponent<SongManager>().songArchive)
            {
                //Create an instance of the UI Tile
                GameObject tile = Instantiate(tilePrefab, tilePrefab.transform.position, tilePrefab.transform.rotation) as GameObject;

                //Make the note a child of the canvas object
                tile.transform.SetParent(GameObject.Find("Canvas").transform, false);

                //Set the default position of the tile, height adjusted by value
                tile.transform.position = _defaultUITilePos;

                //Set the song tile text
                tile.transform.Find("SongTitle").GetComponent<TextMeshProUGUI>().text = newPackage.songTitle;
                tile.transform.Find("SongArtist").GetComponent<TextMeshProUGUI>().text = newPackage.songArtist;

                //Add the tile to the list of tiles
                tileList.Add(tile);
            }

            tilesMade = true;
        }
        //Once the tiles have been made allow manipulation of them
        else
        {
            //Update the tile positions in the song list
            int _counter2 = 0;

            //Timer allows input delay
            if (_timer == 0)
            {
                //Logic to move cursor up
                if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().upPressed == true)
                {
                    //Loop around the list
                    if (cursor == 0)
                    {
                        cursor = (GameObject.Find("SongManager").GetComponent<SongManager>().songArchive.Count - 1);
                    }
                    //Move the cursor up the list
                    else if (cursor > 0 && _timer == 0)
                    {
                        cursor--;
                    }

                    _timer = _delay;
                    cursorMoved = true;
                }

                //Logic to move cursor down
                else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().downPressed == true)
                {
                    //Loop around the list
                    if (cursor == (GameObject.Find("SongManager").GetComponent<SongManager>().songArchive.Count - 1))
                    {
                        cursor = 0;
                    }
                    //Move the cursor down the list
                    else if (cursor < (GameObject.Find("SongManager").GetComponent<SongManager>().songArchive.Count - 1) && _timer == 0)
                    {
                        cursor++;
                    }

                    _timer = _delay;
                    cursorMoved = true;
                }

                //Logic to make a selection from the menu
                else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().selectPressed == true)
                {
                    //Save the cursor position, can also be used to identify which song was selected
                    PlayerPrefs.SetInt("CursorPos", cursor);

                    //Run the code to reset this scene for future use
                    PrepareToExit();

                    if (PlayerPrefs.GetInt("GameMode") == 1)
                    {
                        //transition to the game
                        Loader.Load(Loader.Scene.Gameplay);
                    }

                    if (PlayerPrefs.GetInt("GameMode") == 2)
                    {
                        //transition to the editor
                        Loader.Load(Loader.Scene.Editor);
                    }
                }

                //Logic to move cancel or move back a screen - matters for options menu
                else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().backPressed == true)
                {
                    //Run the code to reset this scene for future use
                    PrepareToExit();

                    //Destroy the tile list
                    foreach (GameObject tile in tileList)
                    {
                        Destroy(tile);
                    }
                    //Return to title screen
                    Loader.Load(Loader.Scene.TitleScreen);
                }

            }
            else if (_timer >= 1)
            {
                _timer--;
            }

            //Modify tiles based on the song manager data
            if (cursorMoved == true)
            {
                foreach (GameObject tile in tileList)
                {
                    //Check the difference between where the cursor is and where we are in the song list
                    int _difference = Mathf.Abs(cursor - _counter2);

                    //The highlighted song pokes out a bit
                    if (_counter2 == cursor)
                    {
                        tile.transform.position = _defaultUITilePos;
                        tile.transform.position -= new Vector3(20, 10, 0);
                    }
                    //The other songs are +/- 100px for each position above or below the cursor
                    else if (_counter2 < cursor)
                    {
                        tile.transform.position = _defaultUITilePos;
                        tile.transform.position += new Vector3(0, (110 * _difference), 0);
                    }
                    else if (_counter2 > cursor)
                    {
                        tile.transform.position = _defaultUITilePos;
                        tile.transform.position -= new Vector3(0, (110 * _difference), 0);
                    }

                    _counter2++;
                }
            }

            //Create a reference to the scene's audio source
            AudioSource sceneAudio = GameObject.Find("Main Camera").GetComponent<AudioSource>();

            //Play the preview for the currently highlighted song
            if (cursorMoved == true)
            {
                //Fade the volume of any currently playing song
                if (sceneAudio.volume > 0)
                {
                    sceneAudio.volume -= 0.001f;
                }
                else
                {
                    //Stop the current song
                    sceneAudio.Stop();

                    //Load the new song into the audio source on the camera
                    sceneAudio.clip = GameObject.Find("SongManager").GetComponent<SongManager>().songArchive[cursor].songAudio;

                    //Restore volume
                    sceneAudio.volume = 1f;

                    //Play the new song
                    sceneAudio.PlayScheduled(GameObject.Find("SongManager").GetComponent<SongManager>().songArchive[cursor].songPreviewStartTime);

                    //Reset the switch
                    GameObject.Find("Canvas").GetComponent<SongMenuUI>().cursorMoved = false;
                }
            }

            //Loop the song preview
            if (sceneAudio.isPlaying)
            {
                if (sceneAudio.time > GameObject.Find("SongManager").GetComponent<SongManager>().songArchive[cursor].songPreviewStartTime + 10f)
                {
                    if (sceneAudio.volume > 0)
                    {
                        sceneAudio.volume -= 0.0015f;
                    }
                    else
                    {
                        sceneAudio.Stop();
                        sceneAudio.Play();
                        sceneAudio.volume = 1;
                    }
                }
            }
        }
    }

    void PrepareToExit()
    {
        //Stop song preview
        GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
    }
}
