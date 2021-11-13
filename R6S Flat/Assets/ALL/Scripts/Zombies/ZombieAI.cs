using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Threading;
using System.Linq;
using System.Linq.Expressions;
using UnityEditor;
//using System;

public class ZombieAI : MonoBehaviour
{
    [Header("0: Idle, 1: Alerted, 2: Engaged")]
    public int alertMode;
    public bool usePathCollider;
    public GameObject pathCollider;
    public float staticVelSum;
    private Vector2 lastPos;

    [Range(0, 1)]
    public float speedVariation;
    private Rigidbody2D rb;
    public bool seperateSpriteRotation;
    public GameObject sprite;
    private ZombieStats stats;
    private GameObject player;
    public Transform movePosition;
    private float lastHealth;

    [Header("IDLE---------------")]
    public float idleSpeed;
    public float idleDistance;
    public float idleDelay;
    private float idleDelay_timer;
    public float idleDelay_randomRange;
    private Vector2 idlePosition;
    public float idleTimeout;
    private float idleTimeout_timer;

    [Header("ALERTED------------")]
    public bool seekNoise;
    public float alertDist;
    [Range(0, 0.5f)]
    public float min_alertDist;
    public float hitAlertDist;
    public float outOfSightTime;
    private float outOfSightTime_timer;

    public float alertedSpeed;
    public Vector2 targetPosition;
    public bool broadcastTarget;
    public float broadcastTarget_radius;
    public float broadcastTarget_interval;
    private float broadcastTarget_interval_timer;

    [Header("ENGAGED------------")]
    public bool enable_attackAnimation;
    public Animator anim;
    public float attackDistance;
    public float attackInterval;
    private float attackInterval_timer;
    public float damage;
    public float rotSpeed;
    public float infect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //sprite = transform.GetChild(0).gameObject;  //this might cause errors later on but do i look like i give a shit
        stats = GetComponent<ZombieStats>();
        player = UTIL.GetPlayer();

        idleSpeed *= Random.Range(speedVariation, 1);
        alertedSpeed *= Random.Range(speedVariation, 1);

        SetNewIdlePos();
        lastHealth = stats.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (seperateSpriteRotation)
        {
            sprite.transform.position = transform.position;
            if (alertMode != 2)
                sprite.transform.rotation = Quaternion.Lerp(sprite.transform.rotation, transform.rotation, rotSpeed * Time.deltaTime);
        }


        if (usePathCollider)
        {
            if (UTIL.FastDist(lastPos, transform.position, 0.01f) <= staticVelSum)
            {
                pathCollider.SetActive(true);
                //Debug.Log(UTIL.FastDist(lastPos, transform.position, 0.01f));
            }
            else
            {
                pathCollider.SetActive(false);
                //Debug.Log(UTIL.FastDist(lastPos, transform.position, 0.01f));
            }
        }

        lastPos = transform.position;

        //!!!IMPORTANT NOTICE--------------------!!!
        ///I placed the main code in "void Update()", so I'll be using Time.deltaTime; that way it'll be easier to manage time based variables.
        //Usually I place movement based codes in "void FixedUpdate()", but that's not the case 
        //since all the movement physics are managed in a seperate code: "AIPath.cs"


        Radar();

        switch (alertMode)
        {
            case 0:
                sprite.GetComponent<Animator>().SetInteger("alertMode", 0);
                Idle();
                break;
            case 1:
                //sprite.GetComponent<Animator>().SetInteger("alertMode", 1);
                Alerted();
                break;
            case 2:
                Engaged();
                break;
        }
    }

    /// <summary>
    /// Used For Idle State------------------------
    /// </summary>
    void Idle()
    {
        attackInterval_timer = attackInterval;

        if (UTIL.FastDist(transform.position, idlePosition, 0.1f) > GetComponent<AIPath>().endReachedDistance)
        {
            movePosition.position = idlePosition;
            GetComponent<AIPath>().maxSpeed = idleSpeed;
        }
        else
        {
            idleDelay_timer += Time.deltaTime;
            if (idleDelay_timer >= idleDelay)
            {
                SetNewIdlePos();
                idleDelay_timer = 0;
                idleTimeout_timer = 0;
            }
        }
        idleTimeout_timer += Time.deltaTime;
        if (idleTimeout_timer >= idleTimeout)
        {
            idleDelay_timer += Time.deltaTime;
            if (idleDelay_timer >= idleDelay)
            {
                SetNewIdlePos();
                idleDelay_timer = 0;
                idleTimeout_timer = 0;
            }
        }

    }
    void SetNewIdlePos()
    {
        idlePosition = transform.position;
        idlePosition += new Vector2(Random.Range(-idleDistance, idleDistance), Random.Range(-idleDistance, idleDistance));

        int a = 0; //FAILSAFE
        while (a < 100)
        {
            a++;
            idlePosition = transform.position;
            idlePosition += new Vector2(Random.Range(-idleDistance, idleDistance), Random.Range(-idleDistance, idleDistance));
            if (Physics2D.Raycast(transform.position, idlePosition - new Vector2(transform.position.x, transform.position.y), UTIL.FastDist(transform.position, idlePosition, 0.05f), UTIL.GetSUWL()).collider == null)
            {
                break;
            }
        }
    }

    /// <summary>
    /// Used For Alerted State-------------------------
    /// </summary>
    void Alerted()
    {
        targetPosition = player.transform.position;
        movePosition.position = targetPosition;
        GetComponent<AIPath>().maxSpeed = alertedSpeed;
    }

    void Radar()
    {
        if (lastHealth > stats.health)
        {
            alertDist = hitAlertDist;
        }
        lastHealth = stats.health;


        RaycastHit2D wallInSight = Physics2D.Raycast(transform.position, player.transform.position - transform.position, UTIL.FastDist(transform.position, player.transform.position, 0.05f), UTIL.GetSUWL());
        if ((wallInSight.collider == null && UTIL.CompareDist(transform.position, player.transform.position, alertDist) <= 0) && UTIL.CompareDist(transform.position, player.transform.position, alertDist) <= 0)
        {
            if (broadcastTarget)
            {
                Collider2D[] zs = Physics2D.OverlapCircleAll(transform.position, broadcastTarget_radius, LayerMask.GetMask("Zombie"));
                foreach (Collider2D z in zs)
                {
                    RaycastHit2D wallInSight1 = Physics2D.Raycast(transform.position, z.transform.position - transform.position, UTIL.FastDist(transform.position, z.transform.position, 0.05f), UTIL.GetSUWL());
                    if (wallInSight1.collider == null)
                    {
                        z.GetComponent<ZombieAI>().alertMode = 1;
                        break;
                    }
                }
            }
            outOfSightTime_timer = outOfSightTime;
            alertMode = 1;
        }
        else
        {
            Collider2D[] zs = Physics2D.OverlapCircleAll(transform.position, broadcastTarget_radius, LayerMask.GetMask("Zombie"));
            //Debug.Log(zs.Length);
            bool flag = false;
            foreach (Collider2D z in zs)
            {
                RaycastHit2D wallInSight1 = Physics2D.Raycast(z.transform.position, player.transform.position - z.transform.position, UTIL.FastDist(player.transform.position, z.transform.position, 0.05f), UTIL.GetSUWL());
                if (wallInSight1.collider == null)
                {
                    flag = true;
                    outOfSightTime_timer = outOfSightTime;
                    break;
                }
            }

            outOfSightTime_timer -= Time.deltaTime;

            if (!flag && outOfSightTime_timer <= 0)
            {
                alertMode = 0;
                outOfSightTime_timer = outOfSightTime;
            }
                
        }

        if ((alertMode == 1) && UTIL.CompareDist(transform.position, player.transform.position, attackDistance) <= 0)
        {
            alertMode = 2;
        }
    
    }

    void Engaged()
    {
        if (seperateSpriteRotation)
        {
            Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.TransformDirection(Vector3.up));
            sprite.transform.rotation = Quaternion.Lerp(sprite.transform.rotation, new Quaternion(0, 0, rotation.z, rotation.w), rotSpeed * Time.deltaTime);
        }

        attackInterval_timer += Time.deltaTime;
        if (attackInterval_timer >= attackInterval)
        {
            player.GetComponent<PlayerStats>().TakeDamage(damage);
            player.GetComponent<PlayerStats>().Infect(infect);
            if (enable_attackAnimation)
                anim.SetTrigger("Attack");
            attackInterval_timer = 0;
        }
        
    }
}
