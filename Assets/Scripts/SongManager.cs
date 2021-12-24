using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongManager : MonoBehaviour
{
    //The song archive is a list of SongPackage files, containing references to related files for each song
    public List<SongPackage> songArchive = new List<SongPackage>();
    public bool songsLoaded = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //First scan the songs folder and create a song package object for each song found
        ScanFiles();

        //For each song package, instantiate the song's information and files into its song package & create a UI Tile for it
        foreach (SongPackage newPackage in songArchive)
        {
            //Instantiate each song member
            BuildSongPackages(newPackage);
        }

        //Save the collected song data to an xml file
        Save(songArchive);

        songsLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (songsLoaded == true)
        {
            Loader.Load(Loader.Scene.TitleScreen);
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

        //Instantiate the song files so they can be played later
        //Trim the path string
        string _tempString = sP.songPath.ToString().Remove(0, 73);
        _tempString = _tempString.Remove((_tempString.Length - 4), 4);
        //Debug.Log(_tempString);

        //Load the audio clip from the file
        AudioClip _tempClip = Resources.Load<AudioClip>(_tempString);

        //Assign the loaded clip to the song's audio member
        sP.songAudio = _tempClip;

        //Output the current song name to the UI
        GameObject.Find("Data").GetComponent<TextMeshProUGUI>().text = sP.songTitle + " Loaded!";
    }

    //Save the song archive data to an xml file
    public void Save(List<SongPackage> archive)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<SongPackage>));
        TextWriter writer = new StreamWriter("C:/Users/Nicholas Street/Unity Projects/GitarooProtoype/Assets/Resources/Songs/songarchive.xml");

        serializer.Serialize(writer, archive);
    }

    //Load the song archive data from an xml file
    public List<SongPackage> Load()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<SongPackage>));
        serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
        serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

        FileStream fs = new FileStream("C:/Users/Nicholas Street/Unity Projects/GitarooProtoype/Assets/Resources/Songs/songarchive.xml", FileMode.Open);
        List<SongPackage> songArchive;

        songArchive = (List<SongPackage>)serializer.Deserialize(fs);

        return songArchive;
    }

    //Handles unknown node errors from the xml file reader
    private void serializer_UnknownNode
    (object sender, XmlNodeEventArgs e)
    {
        Debug.Log("Unknown Node:" + e.Name + "\t" + e.Text);
    }

    //Handles unknown attribute errors from the xml file reader
    private void serializer_UnknownAttribute
    (object sender, XmlAttributeEventArgs e)
    {
        System.Xml.XmlAttribute attr = e.Attr;
        Debug.Log("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
    }
}