using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.U2D;

public class Demo_AttachObjectToSplineNode : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    public int index;
    public bool useNormals = false;
    public bool runtimeUpdate = false;
    public float yOffset = 0.0f;
    public bool localOffset = false;

    private UnityEngine.U2D.Spline _spline;
    private int lastSpritePointCount;
    private bool lastuseNormals;
    private Vector3 lastPosition;


    // Start is called before the first frame update
    void Start()
    {
        _spline = spriteShapeController.spline;
    }

    // Update is called once per frame
    void Update()
    {
        if ((_spline.GetPointCount() != 0) && (lastSpritePointCount !=0))
        {
            index = Mathf.Clamp(index, 0, _spline.GetPointCount() - 1);
            if (_spline.GetPointCount() != lastSpritePointCount)
            {
                index += _spline.GetPointCount() - lastSpritePointCount;
            }
            if ((index <= _spline.GetPointCount() - 1) && (index >= 0))
            {
                if (useNormals)
                {
                    if (_spline.GetTangentMode(index) != ShapeTangentMode.Linear)
                    {
                        Vector3 lt = Vector3.Normalize(_spline.GetLeftTangent(index) - _spline.GetRightTangent(index));
                        Vector3 rt = Vector3.Normalize(_spline.GetRightTangent(index) - _spline.GetLeftTangent(index));

                        float a = Angle(Vector3.left, lt);
                        float b = Angle(Vector3.right, rt);
                        float c = a + (b * 0.5f);

                        if (b > c)
                            c = (180 + c);

                        transform.rotation = Quaternion.Euler(0, 0, c);
                    }
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                Vector3 offsetVector;
                if (localOffset)
                {
                    offsetVector = (Vector3)Rotate(Vector2.up, transform.localEulerAngles.z) * yOffset;
                }
                else
                {
                    offsetVector = Vector2.up * yOffset;
                }
                transform.position = spriteShapeController.transform.position + _spline.GetPosition(index) + offsetVector;
                lastPosition = _spline.GetPosition(index);
            }
        }
        lastSpritePointCount = _spline.GetPointCount();
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
