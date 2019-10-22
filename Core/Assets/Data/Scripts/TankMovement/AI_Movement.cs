using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement : MonoBehaviour
{

    public bool active = true;

    float spawnRange = 25f;

    Vector3 destination;

    [Header("Movement speed")]
    public float moveSpeed = 2f;
    public float moveRotateSpeed = 0.3f;
    public float currentSpeed = 0;

    [Header("Rotation related")]
    public float maxAngleForFullSpeed = 4f;
    public float rotateStrength = 1.2f;

    [Header("SpawnParticles")]
    public GameObject SpawnParticles;


    void Start()
    {
        Instantiate(SpawnParticles, transform.position, transform.rotation);
        SetRandomDestination();
    }

   
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    void SetRandomDestination()
    {
        float x = UnityEngine.Random.Range(-spawnRange+1, spawnRange-1);
        float z = UnityEngine.Random.Range(-spawnRange+1, spawnRange-1);

        destination = new Vector3(x,transform.position.y,z);
    }

    void Move()
    {
        if (!active) return;

        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * GetCurrentSpeed(destination));

        if (transform.position == destination)
        {
            SetRandomDestination();
        }
        RotateTowards(destination);
    }

    void RotateTowards(Vector3 rotateTo)
    {
        Quaternion targetRotation = Quaternion.LookRotation(rotateTo - transform.position);
        float str = Mathf.Min(rotateStrength * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
    }


    float GetCurrentSpeed(Vector3 dest)
    {
        float currentSpeed = 0;
        Quaternion targetRotation = Quaternion.LookRotation(dest - transform.position);

        float a = Math.Abs(transform.rotation.eulerAngles.y - targetRotation.eulerAngles.y);

        if (a <= maxAngleForFullSpeed)
        {
            currentSpeed = moveSpeed;
        }
        else
        {
            currentSpeed = moveRotateSpeed;
        }

        return currentSpeed;
    }
}
