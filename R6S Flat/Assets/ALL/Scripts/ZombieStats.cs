using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : MonoBehaviour
{
    public string ID;
    public float maxHealth;
    public float health;
    public bool regen;
    public float regenSpeed;

    private GameObject player;
    public GameObject sprite;

    [Header("On Death()-------------")]
    public List<GameObject> onDeathSpawnObjs;
    public float objSpawnOffset;

    private void Start()
    {
        player = UTIL.GetPlayer();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void Update()
    {
        sprite.GetComponent<ShowInSight>().target = player.transform;

        if (health <= 0)
            Die();

        if (regen)
            health = maxHealth - health >= Time.deltaTime * regenSpeed ? health + Time.deltaTime * regenSpeed : maxHealth;
    }

    public void Die()
    {
        foreach (GameObject g in onDeathSpawnObjs)
        {
            Vector2 spawnPos = transform.position;
            float i = objSpawnOffset;
            spawnPos += new Vector2(Random.Range(-i, i), Random.Range(-i, i));
            Instantiate(g, spawnPos, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
