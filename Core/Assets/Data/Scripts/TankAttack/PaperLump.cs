using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperLump : MonoBehaviour
{
    public int ID_HOLDER = 0;

    float timeToExpire = 1.5f;
    float currentTime = 0f;

    public GameObject destroyParticles;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        TimerCheck();
    }

    void TimerCheck()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeToExpire)
        {
            Explode();
        }
    }

    void Explode()
    {
        //Instantiate(destroyParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI_Agressive")
        {
            AI_Attack target = other.gameObject.GetComponent<AI_Attack>();
            if (target.ID_UQNIQUE != ID_HOLDER)
            {
                target.GetHit();
                Explode();
            }
        }
    }

    public void SetOwnerId(int id)
    {
        ID_HOLDER = id;
    }
}
