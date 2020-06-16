using System;
using UnityEngine;
using UnityEngine.U2D;

public class GameNote : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    public GameObject targetPrefab;
    public GameObject endPrefab;

    private UnityEngine.U2D.Spline _spline;
    private Vector3 _firstNode;
    private Vector3 _lastNode;
    private int _index;

    // Start is called before the first frame update
    void Start()
    {
        //Get the spline from the SpriteShape controller
        _spline = spriteShapeController.spline;

        //Set the first node as the back of the note
        _firstNode = _spline.GetPosition(0);

        //Set the last node as the front of the note
        _index = _spline.GetPointCount() - 1;
        _lastNode = _spline.GetPosition(_index);

        //Create the target circle and the cap
        MakeParts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeParts()
    {
        //Create an instance of the target circle
        GameObject target = Instantiate(targetPrefab) as GameObject;
        //Place the target circle at one end of the note
        target.transform.position = _lastNode;

        //Create an instance of the end cap
        GameObject endCap = Instantiate(endPrefab) as GameObject;
        //Place the end cap at the other end of the note
        endCap.transform.position = _firstNode;

        //Calculate the spline angle for the end cap's rotation
        float c = Mathf.Atan2((_spline.GetRightTangent(0).y), (_spline.GetRightTangent(0).x));
        c = c * Mathf.Rad2Deg;

        //Apply rotation (c) to the end cap's rotation transformation
        endCap.transform.rotation = Quaternion.Euler(0, 0, (c));
    }
}
