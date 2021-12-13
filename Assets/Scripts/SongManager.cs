using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public class SongPackage
    {
        //Song file paths
        public FileInfo songPath { get; set; }
        public FileInfo imagePath { get; set; }
        public FileInfo chartPath { get; set; }

        //High Level Song Metadata
        public string chartAuthor { get; set; }
        public int totalNotes { get; set; }
        public string playerMode { get; set; }

        //Chart Metadata
        public string songTitle { get; set; }
        public string songSubtitle { get; set; }
        public string songArtist { get; set; }
        public int songBPM { get; set; }
        public int songPreviewStartTime { get; set; }

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

    // Start is called before the first frame update
    void Start()
    {
        //First scan the songs folder and create a song package object for each song found
        ScanFiles();

        //For each song package instantiate the song's information and files into its song package
        foreach (SongPackage newPackage in songArchive)
        {
            ReadSongMetaData(newPackage);
        }

        //DEBUG OUTPUT
        foreach (SongPackage sP in songArchive)
        {
            Debug.Log(sP.songPath);
            Debug.Log(sP.imagePath);
            Debug.Log(sP.chartPath);
            Debug.Log(sP.chartAuthor);
            Debug.Log(sP.totalNotes);
            Debug.Log(sP.songTitle);
            Debug.Log(sP.songSubtitle);
            Debug.Log(sP.songArtist);
            Debug.Log(sP.songBPM);
            Debug.Log(sP.songPreviewStartTime);
            
            foreach (string eventArray in sP.songP1Events_standard)
            {
                Debug.Log(eventArray);
            }
            foreach (string eventArray in sP.songP1Events_master)
            {
                Debug.Log(eventArray);
            }
            foreach (string eventArray in sP.songP2Events_standard)
            {
                Debug.Log(eventArray);
            }
            foreach (string eventArray in sP.songP2Events_master)
            {
                Debug.Log(eventArray);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This function generates a list of the songs and related files in the song directory
    void ScanFiles()
    {
        //Counts up for each new song added to the songArchive
        int _count = 0;

        //Make a record of the paths to all files in the Song directory
        string[] _tempArchive = Directory.GetDirectories("C:/Users/Nicholas Street/Unity Projects/GitarooProtoype/Assets/Songs");
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
    void ReadSongMetaData(SongPackage sP)
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
                    //AUTHOR - The chart author (8)
                    case 0:
                        //Debug.Log(line.Remove(0, 8));
                        sP.chartAuthor = line.Remove(0,8);
                        break;
                    //NOTES - Total number of notes in the chart (7)
                    case 1:
                        //Debug.Log(line.Remove(0,7));
                        sP.totalNotes = int.Parse(line.Remove(0,7));
                        break;
                    //TITLE - Title of the song (7)
                    case 2:
                        //Debug.Log(line.Remove(0,7));
                        sP.songTitle = line.Remove(0,7);
                        break;
                    //SUBTITLE - Subtitle of the song (10)
                    case 3:
                        //Debug.Log(line.Remove(0,10));
                        sP.songSubtitle = line.Remove(0,10);
                        break;
                    //ARTIST - Artist of the song (8)
                    case 4:
                        //Debug.Log(line.Remove(0,8));
                        sP.songArtist = line.Remove(0,8);
                        break;
                    //BPM - BPM of the song (5)
                    case 5:
                        //Debug.Log(line.Remove(0,5));
                        sP.songBPM = int.Parse(line.Remove(0,5));
                        break;
                    //PREVIEW - Preview start time of the song (in seconds) (9)
                    case 6:
                        //Debug.Log(line.Remove(0,9));
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