using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Attack : MonoBehaviour
{
    public bool active = true;

    [Header("Important for lumps")]
    public int ID_UQNIQUE = 0;
    public float HP = 1f;
    public GameObject lumpPrefab;
    public GameObject projectileSpawn;

    [Range(0,100)]
    public float lumpSpeed = 13f;
    [Range(0, 50)]
    public float lumpSpread = 23f;

    List<GameObject> targets = new List<GameObject>();
    GameObject closestTarget;

    [Header("Attack Parameters")]
    public float attackDistanceMeters = 6f;
    public float metersToCurrDistRatio = 1f;
    public float attackEach = 3f;
    float currentAttackTime = 0f;

    [Header("Tank tower and params")]
    public GameObject tankTower;
    public float towerRotateSpeed = 4f;

    public GameObject destroyedParticles;


    private void FixedUpdate()
    {
        SubAttack();
    }

    void SubAttack()
    {
        if (!active) return;

        SetTargets();
        closestTarget = GetClosestTarget();
        GunRotateToEnemy(closestTarget);
        Shoot(closestTarget);

    }

    void SetTargets()
    {
        targets = new List<GameObject>();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("AI_Agressive");
        foreach (GameObject a in enemies)
        {
            targets.Add(a);
        }
    }
    public GameObject GetClosestTarget()
    {
        GameObject closestTarget = null;

        foreach (GameObject a in targets)
        {
            if (closestTarget == null)
            {
                if (a != null && a.GetComponent<AI_Attack>().ID_UQNIQUE != ID_UQNIQUE)
                    closestTarget = a;
            }

            if (a != null && gameObject != null && closestTarget != null)
            {
                if (Vector3.Distance(gameObject.transform.position, a.transform.position) < Vector3.Distance(gameObject.transform.position, closestTarget.transform.position))
                {
                    if (a != null && a.GetComponent<AI_Attack>().ID_UQNIQUE != ID_UQNIQUE)
                        closestTarget = a;
                }
            }

        }
        return closestTarget;
    }

    void GunRotateToEnemy(GameObject makeAttackOn)
    {
        if (makeAttackOn == null) return;

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(makeAttackOn.transform.position.x, tankTower.transform.position.y, makeAttackOn.transform.position.z) - tankTower.transform.position);
        float str = Mathf.Min(towerRotateSpeed * Time.deltaTime, 1);
        tankTower.transform.rotation = Quaternion.Lerp(tankTower.transform.rotation, targetRotation, str);
    }


    bool IsAimed(Vector3 enemy)
    {
        Quaternion targetRotation = Quaternion.LookRotation(enemy - transform.position);

        float a = Math.Abs(tankTower.transform.rotation.eulerAngles.y - targetRotation.eulerAngles.y);
        if (a <= 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsCloseEnough(Vector3 enemy)
    {
        if (Vector3.Distance(transform.position, enemy) <= (attackDistanceMeters * metersToCurrDistRatio))
        {
            return true;
        }

        return false;
    }

    void Shoot(GameObject a)
    {
        if (a == null) return;

        Vector3 enemy = a.transform.position;
        currentAttackTime += Time.deltaTime;

        if (currentAttackTime >= attackEach)
        {
            if (IsAimed(enemy) && IsCloseEnough(enemy))
            {
                ProjectilesSpawn();
                currentAttackTime = 0;
            }
        }
    }

    void ProjectilesSpawn()
    {
        int lumpCount = 10;

        float spreadFactor = 0.3f;
        

        GameObject lumpObj;
        Rigidbody lumpRb;

        for (var i = 0; i < lumpCount; i++)
        {
            Vector3 spread = new Vector3(UnityEngine.Random.Range(-spreadFactor, spreadFactor), UnityEngine.Random.Range(-spreadFactor, spreadFactor), UnityEngine.Random.Range(-spreadFactor, spreadFactor));
            var lumpRot = transform.rotation;
            
            Vector3 littleRotation = new Vector3(UnityEngine.Random.Range(-lumpSpread, lumpSpread), UnityEngine.Random.Range(-lumpSpread, lumpSpread), UnityEngine.Random.Range(-lumpSpread, lumpSpread));

            lumpObj = Instantiate(lumpPrefab, projectileSpawn.transform.position + spread, projectileSpawn.transform.rotation);
            lumpObj.transform.Rotate(littleRotation);
            lumpObj.GetComponent<PaperLump>().SetOwnerId(ID_UQNIQUE);
            lumpRb = lumpObj.GetComponent<Rigidbody>();
            lumpRb.velocity = lumpObj.transform.forward * lumpSpeed;
        }
    }

    public void GetHit()
    {
        HP -= 1;
        if (HP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>().killedTanks++;
        Instantiate(destroyedParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
