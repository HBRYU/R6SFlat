using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
//using UnityScript.Steps;

[System.Serializable]
public class ExplosiveStats
{
    public string ID;
    public float delay;
    public float throwVelocity;
    public GameObject pelletLR;
    public GameObject explosion;
    public LayerMask hitLayers;
    public LayerMask wallLayers;

    public GameObject[] pelletHit;

    [Header("Pellet Damage ----------------")]
    public int pelletCount;
    public float pelletDamage;
    [Tooltip("Damage Proportional To Proximity. See Gun.cs")]
    public bool pellet_DPTP;
    public float DPTP_minRange;
    public float DPTP_maxRange;
    public float DPTP_minDamage;

    [Header("Base Damage ------------------")]
    public float damageRange;
    public float damage;
    public bool base_DPTP;

    public bool cameraShake;
    public float cameraShakeDuration;
}


public class Grenade : MonoBehaviour
{
    public ExplosiveStats STATS;

    private void Awake()
    {
        if (STATS.pelletHit.Length == 0)
            STATS.pelletHit = UTIL.GetGM().GetComponent<ItemList>().guns_default_bulletHit;
        if(STATS.hitLayers == 0)
            STATS.hitLayers = UTIL.GetGM().GetComponent<ItemList>().explosives_default_hitLayers;
    }

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * STATS.throwVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (STATS.delay <= 0)
            Explode();
        STATS.delay -= Time.deltaTime;
    }
    
    public void Explode()
    {
        //BASE DAMAGE
        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, STATS.damageRange, STATS.hitLayers);
        foreach(Collider2D obj in objs)
        {

            if(Physics2D.Raycast(transform.position, obj.transform.position - transform.position, UTIL.FastDist(transform.position, obj.transform.position, 0.05f), STATS.wallLayers).collider == null)
            {
                float appliedDamage = STATS.damage;
                if (STATS.base_DPTP)
                {
                    appliedDamage = STATS.damage * (STATS.damageRange - UTIL.FastDist(transform.position, obj.transform.position, 0.1f));
                    if (appliedDamage <= 0)
                        appliedDamage = 0;
                }

                //Debug.Log("Applied base damage: " + appliedDamage);

                if (obj.GetComponent<ZombieStats>() != null)
                    obj.GetComponent<ZombieStats>().TakeDamage(appliedDamage);
                if (obj.GetComponent<ObjectStats>() != null)
                    obj.GetComponent<ObjectStats>().TakeDamage(appliedDamage);
                if (obj.GetComponent<PlayerStats>() != null)
                    obj.GetComponent<PlayerStats>().TakeDamage(appliedDamage);
            }  
        }

        //PELLET DAMAGE
        for(int i = 0; i < STATS.pelletCount; i++)
        {
            Vector3 targetPos = new Vector3(transform.position.x + UnityEngine.Random.Range(-10, 10), transform.position.y + UnityEngine.Random.Range(-10, 10), 0);
            Vector3 fireDir = targetPos - transform.position;
            RaycastHit2D shot = Physics2D.Raycast(transform.position, fireDir, UTIL.FastDist(transform.position, targetPos, 0.1f), STATS.hitLayers);

            float appliedDamage = STATS.pelletDamage;
            float dist = UTIL.FastDist(transform.position, shot.point, 0.1f);
            if (STATS.pellet_DPTP && dist >= STATS.DPTP_minRange)
            {
                appliedDamage = STATS.damage * (STATS.DPTP_maxRange - dist) / (STATS.DPTP_maxRange - STATS.DPTP_minRange);
                if (appliedDamage < STATS.DPTP_minDamage)
                    appliedDamage = STATS.DPTP_minDamage;
            }

            if (shot.collider != null)
            {
                if (shot.collider.GetComponent<ZombieStats>() != null)
                {
                    Instantiate(STATS.pelletHit[1], shot.point, Quaternion.identity);
                    shot.collider.GetComponent<ZombieStats>().TakeDamage(appliedDamage);
                    //Debug.Log("Applied pellet damage: " + appliedDamage);
                }
                if (shot.collider.GetComponent<ObjectStats>() != null)
                {
                    Instantiate(STATS.pelletHit[0], shot.point, Quaternion.identity);
                    shot.collider.GetComponent<ObjectStats>().TakeDamage(appliedDamage);
                }
                if (shot.collider.GetComponent<PlayerStats>() != null)
                {
                    Instantiate(STATS.pelletHit[1], shot.point, Quaternion.identity);
                    shot.collider.GetComponent<PlayerStats>().TakeDamage(appliedDamage);
                }

                LineRenderer lr = Instantiate(STATS.pelletLR, transform.position, Quaternion.identity).GetComponent<LineRenderer>();
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, shot.point);

            }
        }

        Instantiate(STATS.explosion, transform.position, Quaternion.identity);

        if (STATS.cameraShake)
            ShakeCamera(STATS.cameraShakeDuration);

        Destroy(gameObject);
    }

    void ShakeCamera(float duration)
    {
        UTIL.GetPlayer().GetComponent<CameraShake>().Shake(duration);
    }
}
