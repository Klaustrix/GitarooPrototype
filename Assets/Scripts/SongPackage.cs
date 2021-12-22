using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPackage
{
    public FileInfo songPath { get; set; }
    public FileInfo imagePath { get; set; }
    public FileInfo chartPath { get; set; }
    public string chartAuthor { get; set; }
    public int totalNotes { get; set; }
    public string songTitle { get; set; }
    public string songSubtitle { get; set; }
    public string songArtist { get; set; }
    public int songBPM { get; set; }
    public int songPreviewStartTime { get; set; }
    public AudioClip songAudio { get; set; }

    public List<string> songP1Events_standard = new List<string>();
    public List<string> songP1Events_master = new List<string>();
    public List<string> songP2Events_standard = new List<string>();
    public List<string> songP2Events_master = new List<string>();

    public IEnumerator GetEnumerator()
    {
        return (IEnumerator)this;
    }
}