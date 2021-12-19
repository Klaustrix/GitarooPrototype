using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongManager : MonoBehaviour
{
    public GameObject tilePrefab;       //A prefab for note tiles on the song menu
    public int menuCursor = 0;          //A variable to track the cursor position on the menu

    private int _timer = 0;             //Manages input delay
    private int _cursor = 0;            //Manages cursor position in the song list
    private int _delay = 300;           //How many updates to delay any input for
    private bool _songSwitch = true;    //Changes the currently playing song preview

    public class SongPackage
    {
        //Song file paths
        public FileInfo songPath { get; set; }
        public FileInfo imagePath { get; set; }
        public FileInfo chartPath { get; set; }

        //Song Data
        public string chartAuthor { get; set; }
        public int totalNotes { get; set; }
        public string songTitle { get; set; }
        public string songSubtitle { get; set; }
        public string songArtist { get; set; }
        public int songBPM { get; set; }
        public int songPreviewStartTime { get; set; }
        public AudioClip songAudio { get; set; }

        public List<string> songP1Events_standard   = new List<string>();
        public List<string> songP1Events_master     = new List<string>();
        public List<string> songP2Events_standard   = new List<string>();
        public List<string> songP2Events_master     = new List<string>();

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }

    }

    //The song archive is a list of SongPackage files, containing references to related files for each song
    public List<SongPackage> songArchive = new List<SongPackage>();

    //The collection of UI tile elements for each song loaded
    List<GameObject> tileList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //First scan the songs folder and create a song package object for each song found
        ScanFiles();

        int counter = 0;

        //For each song package, instantiate the song's information and files into its song package & create a UI Tile for it
        foreach (SongPackage newPackage in songArchive)
        {
            //Instantiate each song member
            BuildSongPackages(newPackage);

            //Instantiate the song files so they can be played later
            //Trim the path string
            string _tempString = newPackage.songPath.ToString().Remove(0, 73);
            _tempString = _tempString.Remove((_tempString.Length - 4), 4);
            //Debug.Log(_tempString);

            //Load the audio clip from the file
            AudioClip _tempClip = Resources.Load<AudioClip>(_tempString);

            //Assign the loaded clip to the song's audio member
            newPackage.songAudio = _tempClip;

            //-------------UI TILES-------------------
            //Create an instance of the UI Tile
            GameObject tile = Instantiate(tilePrefab) as GameObject;
            
            //Make the note a child of the canvas object
            tile.transform.SetParent(GameObject.Find("Canvas").transform);
            
            //Set the default position of the tile, height adjusted by value
            Vector3 tileAdjust = new Vector3(764, (271-(counter*100)), 0);
            tile.transform.position = tileAdjust;
            
            //Set the song tile text
            tile.transform.Find("SongTitle").GetComponent<TextMeshProUGUI>().text = newPackage.songTitle;
            tile.transform.Find("SongArtist").GetComponent<TextMeshProUGUI>().text = newPackage.songArtist;

            //Add the tile to the list of tiles
            tileList.Add(tile);

            //Move the next tile down by 100px
            counter++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_timer == 0)
        {
            //Logic to move cursor up
            if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().upPressed == true)
            {
                //Loop around the list
                if (_cursor == 0)
                {
                    _cursor = (songArchive.Count - 1);
                }
                //Move the cursor up the list
                else if (_cursor > 0 && _timer == 0)
                {
                    _cursor--;
                }
                
                _timer = _delay;
                _songSwitch = true;
            }

            //Logic to move cursor down
            else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().downPressed == true)
            {
                //Loop around the list
                if (_cursor == (songArchive.Count - 1))
                {
                    _cursor = 0;
                }
                //Move the cursor down the list
                else if (_cursor < (songArchive.Count - 1) && _timer == 0)
                {
                    _cursor++;
                }

                _timer = _delay;
                _songSwitch = true;
            }

            //Logic to make a selection from the menu
            else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().selectPressed == true)
            {
                if (PlayerPrefs.GetInt("GameMode") == 0)
                {
                    //Register the selected song
                    PlayerPrefs.SetInt("selectedSong", _cursor);
                    //transition to the game
                    Loader.Load(Loader.Scene.Gameplay);
                }

                if (PlayerPrefs.GetInt("GameMode") == 1)
                {
                    //Register the selected song
                    PlayerPrefs.SetInt("selectedSong", _cursor);
                    //transition to the editor
                    Loader.Load(Loader.Scene.Editor);
                }
            }

            //Logic to move cancel or move back a screen - matters for options menu
            else if (GameObject.Find("SceneInput").GetComponent<ProcessInput>().backPressed == true)
            {
                //Stop song preview
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
                //Return to title screen
                Loader.Load(Loader.Scene.TitleScreen);
            }

            //Update the tile positions in the song list
            int _counter2 = 0;

            foreach (GameObject tile in tileList)
            {
                //Check the difference between where the cursor is and where we are in the song list
                int _difference = Mathf.Abs(_cursor - _counter2);

                //The highlighted song pokes out a bit
                if (_counter2 == _cursor)
                {
                    tile.transform.position = new Vector3(744, 271, 0);
                }
                //The other songs are +/- 100px for each position above or below the cursor
                else if (_counter2 < _cursor)
                {
                    tile.transform.position = new Vector3(764, 271 + (100 * _difference), 0);
                }
                else if (_counter2 > _cursor)
                {
                    tile.transform.position = new Vector3(764, 271 - (100 * _difference), 0);
                }

                _counter2++;
            }
        }
        else if (_timer >= 1)
        {
            _timer--;
        }

        //Play the preview for the currently highlighted song
        if (_songSwitch == true)
        {
            //Fade the volume of any currently playing song
            if (GameObject.Find("Main Camera").GetComponent<AudioSource>().volume > 0)
            {
                GameObject.Find("Main Camera").GetComponent<AudioSource>().volume -= 0.001f;
            }
            else
            {
                //Stop the current song
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();

                //Load the new song into the audio source on the camera
                GameObject.Find("Main Camera").GetComponent<AudioSource>().clip = songArchive[_cursor].songAudio;

                //Restore volume
                GameObject.Find("Main Camera").GetComponent<AudioSource>().volume = 1f;

                //Play the new song
                GameObject.Find("Main Camera").GetComponent<AudioSource>().PlayScheduled(songArchive[_cursor].songPreviewStartTime);

                //Reset the switch
                _songSwitch = false;
            }
        }

        //Loop the song preview
        if (GameObject.Find("Main Camera").GetComponent<AudioSource>().isPlaying)
        {
            if (GameObject.Find("Main Camera").GetComponent<AudioSource>().time > songArchive[_cursor].songPreviewStartTime + 10f)
            {
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
            }
        }
    }

    //This function generates a list of the songs and related files in the song directory
    void ScanFiles()
    {
        //Counts up for each new song added to the songArchive
        int _count = 0;

        //Make a record of the paths to all files in the Song directory
        string[] _tempArchive = Directory.GetDirectories("C:/Users/Nicholas Street/Unity Projects/GitarooProtoype/Assets/Resources/Songs");
        {
            //For each Song Pack folder found in the Songs directory...
            foreach (string songPack in _tempArchive)
            {
                //Search for individual song folders within the Song Packs
                _tempArchive = Directory.GetDirectories(songPack);
                {
                    //For each song folder found in the song pack...
                    foreach (string song in _tempArchive)
                    {
                        //Create a directory reference for the complete path to the song's files
                        DirectoryInfo songFolder = new DirectoryInfo(song);
                        
                        //Add a new song package file to the song archive
                        songArchive.Add(new SongPackage());

                        //Create a temporary array to store search results from GetFiles
                        FileInfo[] temp = new FileInfo[1];

                        //Fill out the contents of the song package for the current song
                        temp = songFolder.GetFiles("*.mp3");
                        songArchive[_count].songPath = temp[0];
                        if (temp[0] == null)
                        {
                            Debug.Log("Audio File not found for song " + song.ToString());
                        }

                        temp = songFolder.GetFiles("*.png");
                        songArchive[_count].imagePath = temp[0];
                        if (temp[0] == null)
                        {
                            Debug.Log("Song Art not found for song " + song.ToString());
                        }

                        temp = songFolder.GetFiles("*.txt");
                        songArchive[_count].chartPath = temp[0];
                        if (temp[0] == null)
                        {
                            Debug.Log("Chart not found for song " + song.ToString());
                        }

                        //Count one additional song added to the songArchive
                        _count++;
                    }
                }
            }
        }
    }
    
    //This function loads the metadata for each song discovered by ScanFiles
    void BuildSongPackages(SongPackage sP)
    {
        int _count = 0;
        int _switch = 0;
        bool _ignore = false;

        //Go through each chart file and extract the related song's data
        foreach (string line in System.IO.File.ReadLines(sP.chartPath.ToString()))
        {
            //Fill in song metadata, then fill in the rest of the file to the events list
            if (_count < 7)
            {
                switch (_count)
                {
                    //AUTHOR - The chart author
                    case 0:
                        sP.chartAuthor = line.Remove(0,8);
                        break;
                    //NOTES - Total number of notes in the chart
                    case 1:
                        sP.totalNotes = int.Parse(line.Remove(0,7));
                        break;
                    //TITLE - Title of the song
                    case 2:
                        sP.songTitle = line.Remove(0,7);
                        break;
                    //SUBTITLE - Subtitle of the song
                    case 3:
                        sP.songSubtitle = line.Remove(0,10);
                        break;
                    //ARTIST - Artist of the song
                    case 4:
                        sP.songArtist = line.Remove(0,8);
                        break;
                    //BPM - BPM of the song
                    case 5:
                        sP.songBPM = int.Parse(line.Remove(0,5));
                        break;
                    //PREVIEW - Preview start time of the song (in seconds)
                    case 6:
                        sP.songPreviewStartTime = int.Parse(line.Remove(0,9));
                        break;
                }
            }
            else
            {
                //Define which event log to log the song events list into
                switch (line.Remove(0,1))
                {
                    case "P1-Standard":
                        _switch = 1;
                        _ignore = true;
                        break;
                    case "P1-Master":
                        _switch = 2;
                        _ignore = true;
                        break;
                    case "P2-Standard":
                        _switch = 3;
                        _ignore = true;
                        break;
                    case "P2-Master":
                        _switch = 4;
                        _ignore = true;
                        break;
                }

                //Add the events to the appropriate chart event log
                if (_ignore == false)
                {
                    switch (_switch)
                    {
                        case 1:
                            sP.songP1Events_standard.Add(line.Remove(0, 1));
                            break;
                        case 2:
                            sP.songP1Events_master.Add(line.Remove(0, 1));
                            break;
                        case 3:
                            sP.songP2Events_standard.Add(line.Remove(0, 1));
                            break;
                        case 4:
                            sP.songP2Events_master.Add(line.Remove(0, 1));
                            break;
                    }
                }

                _ignore = false;
            }

            _count++;
        }
    }

}