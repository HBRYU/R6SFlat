using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    public string ID;
    public float health;

    [Header("On Break()-------------")]
    public List<GameObject> onBreakSpawnObjs;
    public float objSpawnOffset;

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void Update()
    {
        if (health <= 0)
            Break();
    }

    public void Break()
    {
        foreach(GameObject g in onBreakSpawnObjs)
        {
            Vector2 spawnPos = transform.position;
            float i = objSpawnOffset;
            spawnPos += new Vector2(Random.Range(-i, i), Random.Range(-i, i));
            Instantiate(g, spawnPos, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
