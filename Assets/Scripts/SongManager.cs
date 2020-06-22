using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    //Prefabs
    public GameObject traceLine;
    public GameObject note;

    //Variables that control song data
    public string songTitle = "TEST";           //Song title
    public string songArtist = "Klaustrix";     //Song artist

    //Sound file
    public static float songBPM = 120;          //Song BPM
    public static float moveSpeed = 0;          //The calculated movement speed relative to BPM

    //Varibles that control Trace Lines
    public int activePhrase = 0;                //The currently active phrase
    public int activeTraceLine = 0;             //The currently active trace line

    // Start is called before the first frame update
    void Start()
    {
        //The speed things should move per square (100x100px) to be in time with this songs bpm
        moveSpeed = (songBPM / 60) / 4;

        //DEBUG - Initialise the song objects, real game should load an associate file with this data
        
    }

    // Update is called once per frame
    void Update()
    {
        //Need to know how to save to and read from a file then convert that info into assignable data
        //that instantiates traceLine/note objects in groups at the right time
    }
}
