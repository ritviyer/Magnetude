using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointer : MonoBehaviour
{
    public float speed =0.05f;
    bool isActive = true;

    private void Start()
    {
        InvokeRepeating("FlickerPointer",0.5f,0.5f);
    }
    void FixedUpdate()
    {
        //transform.position += Vector3.up * speed;
    }
    void FlickerPointer()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
}
