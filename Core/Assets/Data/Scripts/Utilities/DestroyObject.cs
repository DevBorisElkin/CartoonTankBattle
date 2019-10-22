using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float DestroyAfter = 0;
    void Start()
    {
        Invoke("DestroyThis", DestroyAfter);
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}