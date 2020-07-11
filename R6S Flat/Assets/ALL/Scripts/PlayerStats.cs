using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ammo
{
    public string name;
    public float count;
}

public class PlayerStats : MonoBehaviour
{
    [Header("REFs-------")]
    public string userName;
    public Transform barrelEnd;
    public Transform bulletSpawnPoint;
    public int ID;
    [Header("VARs-------")]
    public float health;
    public float maxHealth;
    public float healFactor;    
    [Range(0.0f, 1.0f)]
    public float armour;

    [Header("Ammo-------")]
    public List<Ammo> ammo;

    [Header("Testing----")]
    public bool debugColors;
    public List<GameObject> corpse = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Die();
        }

        Heal(healFactor * Time.deltaTime);

        if (debugColors)
            GetComponent<SpriteRenderer>().color = new Color(0, health / maxHealth, 0);
    }

    public void TakeDamage(float damage)
    {
        health -= damage * (1 - armour);
    }

    public void Heal(float heal)
    {
        health = health >= (maxHealth - heal) ? maxHealth : health + heal; //I really don't know what the fuck '?' does but we'll see
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
