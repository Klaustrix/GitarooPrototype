using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    private int _textTimer = 0;
    private int _currentMisses = 0;
    private int _currentHits = 0;

    private int _score = 0;
    public Text scoreText;

    private int _p1CharName;
    public Text p1CharNameText;

    private int _p2CharName;
    public Text p2CharNameText;

    private int _accuracy;
    public Text accuracyText;

    void Update()
    {
        scoreText.text = "" + _score.ToString("0000000");

        if (_score < Activator.songScore)
        {
            _score = Activator.songScore;
        }

        p1CharNameText.text = "Klaustrix";
        p2CharNameText.text = "Nutmeg";

        if (_currentHits < Activator.greatNotes)
        {
            accuracyText.text = "HIT!";
            _currentHits = Activator.greatNotes;
            _textTimer = 100;
        }
        
        if (_currentMisses < Activator.missedNotes)
        {
            accuracyText.text = "MISS!";
            _currentMisses = Activator.missedNotes;
            _textTimer = 100;
        }
        
        if (_textTimer > 0)
        {
            _textTimer--;
        }
        if (_textTimer == 0)
        {
            accuracyText.text = "";
        }
    }
}
