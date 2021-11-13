using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

[System.Serializable]
public class GunStats
{
    [Header("Gun ID---------")]
    public string ID;
    [Tooltip("SMG, Rifle, Pistol, MG, Shotgun, Electric, None")]
    public string ammoType;
    [Header("Stats (Empty: default stats from ItemList.cs applied)")]

    public GameObject bulletLR;
    public GameObject muzzleFlash;
    public GameObject[] bulletHit;

    public Color bulletColor;
    [HideInInspector]
    public Transform barrelEnd, bulletSpawnPoint;
    [Tooltip("데미지가 거리에 반비례하는가")]
    public bool damageProportionalToProximity;
    public float DPTP_minRange;
    public float DPTP_maxRange;
    public float DPTP_minDamage;
    public float damage;
    public float accuracyOffset;
    public float fireRate;
    [Tooltip("Must be long enought to hit at least something")]
    [HideInInspector]
    public float distance = 99;
    public LayerMask hitLayers;
    //public LayerMask damageLayers;
    [HideInInspector]
    public float fireRate_timer;
    public int magSize;
    [HideInInspector]
    public int magSize_counter;
    [Tooltip("(SECONDS)")]
    public float reloadSpeed;
    [HideInInspector]
    public float reloadSpeed_timer;
    public bool fullAuto = true;
    public int multiShotCount = 1;
    [Header("fullAuto == false")]
    public float cameraShakeDuration;
}

public class Gun : MonoBehaviour
{
    [Header("Load existing gun preset: P90, Desert Eagle etc")]
    public string loadPreset;
    [Header(" ")]
    public GunStats STATS;
    [HideInInspector]
    public bool reloading;
    // Start is called before the first frame update

    private void Awake()
    {
        foreach(GunStats stat in UTIL.GetGM().GetComponent<ItemList>().guns)
        {
            if (stat.ID == loadPreset)
                STATS = stat;
        }
    }

    void Start()
    {
        STATS.magSize_counter = STATS.magSize;
        STATS.reloadSpeed_timer = STATS.reloadSpeed;
        STATS.barrelEnd = transform.parent.GetComponent<PlayerStats>().barrelEnd;
        STATS.bulletSpawnPoint = transform.parent.GetComponent<PlayerStats>().bulletSpawnPoint;
        if (STATS.bulletHit.Length == 0)
            STATS.bulletHit = UTIL.GetGM().GetComponent<ItemList>().guns_default_bulletHit;
        if (STATS.bulletLR == null)
            STATS.bulletLR = UTIL.GetGM().GetComponent<ItemList>().guns_default_bulletLR;
        if (STATS.muzzleFlash == null)
            STATS.muzzleFlash = UTIL.GetGM().GetComponent<ItemList>().guns_default_muzzleFlash;
        if(STATS.hitLayers == 0)
        {
            //Debug.Log("applying default hitLayers");
            STATS.hitLayers = UTIL.GetGM().GetComponent<ItemList>().guns_default_hitLayers;
        }
    }

    private void Update()
    {
        if (STATS.magSize_counter <= 0 || Input.GetKeyDown("r"))
            reloading = true;

        if (reloading)
        {
            STATS.reloadSpeed_timer -= Time.deltaTime;
            if (STATS.reloadSpeed_timer <= 0)
            {
                STATS.magSize_counter = STATS.magSize;
                STATS.reloadSpeed_timer = STATS.reloadSpeed;
                reloading = false;
            }
        }

        

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            UTIL.GetPlayer().GetComponent<Animator>().SetBool("Aim", true);
        else
            UTIL.GetPlayer().GetComponent<Animator>().SetBool("Aim", false);

        bool shoot = false;

        foreach (Ammo a in UTIL.GetPlayer().GetComponent<PlayerStats>().ammo)
        {
            if (a.name == STATS.ammoType && a.count > 0)
            {
                shoot = true;
            }
        }

        if (STATS.fullAuto)
        {
            if (Input.GetMouseButton(0) && STATS.fireRate_timer <= 0 && !reloading && shoot)
            {
                Shoot();
                ShakeCamera(STATS.cameraShakeDuration);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && STATS.fireRate_timer <= 0 && !reloading & shoot)
            {
                Shoot();
                ShakeCamera(STATS.cameraShakeDuration);
            }
        }
        STATS.fireRate_timer -= Time.deltaTime;

    }

    void Shoot()
    {
        float temp_ao = STATS.accuracyOffset * UTIL.FastDist(transform.position, UTIL.MousePos(), 0.1f);
        Vector3 targetPos = UTIL.MousePos() + new Vector3(Random.Range(-temp_ao, temp_ao), Random.Range(-temp_ao, temp_ao));
        Vector3 fireDir = targetPos - transform.position;
        RaycastHit2D shot = Physics2D.Raycast(STATS.bulletSpawnPoint.position, fireDir, STATS.distance, STATS.hitLayers);

        List<RaycastHit2D> shots = new List<RaycastHit2D>();

        for (int i = 0; i < STATS.multiShotCount; i++)
        {
            temp_ao = STATS.accuracyOffset * UTIL.FastDist(transform.position, UTIL.MousePos(), 0.1f);
            targetPos = UTIL.MousePos() + new Vector3(Random.Range(-temp_ao, temp_ao), Random.Range(-temp_ao, temp_ao));
            fireDir = targetPos - transform.position;
            shot = Physics2D.Raycast(STATS.bulletSpawnPoint.position, fireDir, STATS.distance, STATS.hitLayers);
            if(shot.collider != null)
                shots.Add(shot);
        }
        foreach(RaycastHit2D thisShot in shots)
        {
            float appliedDamage = STATS.damage;
            float dist = UTIL.FastDist(STATS.barrelEnd.position, thisShot.point, 0.1f);
            if (STATS.damageProportionalToProximity && dist >= STATS.DPTP_minRange)
            {
                appliedDamage = STATS.damage * (STATS.DPTP_maxRange - dist) / (STATS.DPTP_maxRange - STATS.DPTP_minRange);
                if (appliedDamage < STATS.DPTP_minDamage)
                    appliedDamage = STATS.DPTP_minDamage;
            }


            if (thisShot.collider.gameObject.GetComponent<ZombieStats>() != null)
            {
                Instantiate(STATS.bulletHit[1], thisShot.point, Quaternion.identity);
                thisShot.collider.gameObject.GetComponent<ZombieStats>().TakeDamage(STATS.damage);
            }
            else
                Instantiate(STATS.bulletHit[0], thisShot.point, Quaternion.identity);
            if (thisShot.collider.gameObject.CompareTag("Breakable"))
            {
                thisShot.collider.gameObject.GetComponent<ObjectStats>().TakeDamage(STATS.damage);
            }
            //Debug.Log("hit");
            LineRenderer lr = Instantiate(STATS.bulletLR, STATS.bulletSpawnPoint.position, Quaternion.identity).GetComponent<LineRenderer>();
            lr.SetPosition(0, STATS.bulletSpawnPoint.transform.position);
            lr.SetPosition(1, thisShot.point);

            Instantiate(STATS.muzzleFlash, STATS.barrelEnd.position, UTIL.GetPlayer().transform.rotation);
        }            
        STATS.fireRate_timer = STATS.fireRate;
        STATS.magSize_counter -= 1;

        foreach (Ammo a in UTIL.GetPlayer().GetComponent<PlayerStats>().ammo)
        {
            if(a.name == STATS.ammoType)
            {
                a.count -= 1;
            }
        }       
    }

    void ShakeCamera(bool shake)
    {
        if (shake)
        {
            UTIL.GetPlayer().GetComponent<CameraShake>().Shake(true);
            UTIL.GetPlayer().GetComponent<CameraShake>().Shake(-1);
        }
        else
        {
            UTIL.GetPlayer().GetComponent<CameraShake>().Shake(false);
            UTIL.GetPlayer().GetComponent<CameraShake>().Shake(0f);
        }
    }

    void ShakeCamera(float duration)
    {
        UTIL.GetPlayer().GetComponent<CameraShake>().Shake(duration);
    }
}
