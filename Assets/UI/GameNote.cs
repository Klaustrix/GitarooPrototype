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

        MakeParts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeParts()
    {
        GameObject target = Instantiate(targetPrefab) as GameObject;
        target.transform.position = _lastNode;

        GameObject endCap = Instantiate(endPrefab) as GameObject;
        endCap.transform.position = _firstNode;

        Vector3 lt = Vector3.Normalize(_spline.GetLeftTangent(_index) - _spline.GetRightTangent(_index));
        Vector3 rt = Vector3.Normalize(_spline.GetLeftTangent(_index) - _spline.GetRightTangent(_index));

        float a = Angle(Vector3.left, lt);
        float b = Angle(Vector3.right, rt);
        float c = a + (b * 0.5f);

        if (b > c)
            c = (180 + c);

        endCap.transform.rotation = Quaternion.Euler(0, 0, (c * -1 - 90));
    }

    private float Angle(Vector3 a, Vector3 b)
    {
        float dot = Vector3.Dot(a, b);
        float det = (a.x * b.y) - (b.x * a.y);
        return Mathf.Atan2(det, dot) * Mathf.Rad2Deg;
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        float tx = v.x;
        float ty = v.y;
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

}
