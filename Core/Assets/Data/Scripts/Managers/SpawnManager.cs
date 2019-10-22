using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject tankPrefab;

    int id = 0;
    [Header("Params")]
    public float spawnEach = 6f;
    float currentTime = 4;

    float spawnBorder = 25f;

    [Header("UI")]
    public Text timeToNext;
    public Text tanksKilled;

    [HideInInspector]
    public int killedTanks = 0;


    private void FixedUpdate()
    {
        currentTime += Time.deltaTime;

        if (currentTime > +spawnEach)
        {
            currentTime = 0;
            SpawnTheTank();
        }
    }

    public void SpawnTheTank()
    {
        GameObject tank = Instantiate(tankPrefab, new Vector3(Random.Range(-spawnBorder,spawnBorder), 0 , Random.Range(-spawnBorder, spawnBorder)), Quaternion.identity) ;
        tank.GetComponent<AI_Attack>().ID_UQNIQUE = id;
        id++;
    }


    void Update()
    {
        timeToNext.text = (float)System.Math.Round((spawnEach - currentTime), 2)  + "";
        tanksKilled.text = killedTanks + "";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnTheTank();
        }
    }
}
