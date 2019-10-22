using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    public float speedH = 1.7f;
    public float speedV = 1.7f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    void Update()
    {
        
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            yaw = Mathf.Clamp(yaw, -90f, 90f);
            pitch = Mathf.Clamp(pitch, -5f, 30f);

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        
       
    }
}