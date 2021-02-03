using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    bool toMove = false;
    public float startBoundary;
    public float endBoundary;
    public float direction;
    public float length;
    float speed = 0.035f;

    Camera cam;
    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(transform.position);
        bool notOnScreen = screenPoint.y < -1;
        if (notOnScreen)
            Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (toMove)
            MoveMe();
    }
    public void MoveObstacle(float startPoint, float endPoint, float len, float dir)
    {
        toMove = true;
        startBoundary = startPoint;
        endBoundary = endPoint;
        direction = dir;
        length = len;
    }
    void MoveMe()
    {
        Vector3 finalPos = transform.position + (Vector3.right * direction * speed);
        if ( finalPos.x + (length/2) >= endBoundary || finalPos.x - (length/2) <= startBoundary)
            direction = direction * -1;
        transform.Translate(Vector3.right * direction * speed);
    }
}
