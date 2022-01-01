using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSystem : MonoBehaviour
{
    public SongPackage currentSongData;

    // Start is called before the first frame update
    void Start()
    {
        //Load the song data
        currentSongData = GameObject.Find("SongManager").GetComponent<SongManager>().songArchive[PlayerPrefs.GetInt("CursorPos")];
    }

    // Update is called once per frame
    void Update()
    {
        //Pan the screen with the analogue
        transform.position += new Vector3(GameObject.Find("SceneInput").GetComponent<ProcessInput>().stickInput.x *
            GameObject.Find("SceneInput").GetComponent<ProcessInput>().smoothInputSpeed, 
            GameObject.Find("SceneInput").GetComponent<ProcessInput>().stickInput.y *
            GameObject.Find("SceneInput").GetComponent<ProcessInput>().smoothInputSpeed, 
            0);

    }
}
