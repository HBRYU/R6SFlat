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
    [Header("REFs---------")]
    public string userName;
    public Transform barrelEnd;
    public Transform bulletSpawnPoint;
    public int ID;
    [Header("VARs---------")]
    public float health;
    public float maxHealth;
    public float healFactor;
    public float healFactor_maxHealth;
    [Range(0.0f, 1.0f)]
    public float armour;

    [Header("INFECTION----")]
    public float infection;
    public float maxInfection;
    public float infection_healthDrainDelta;
    public float infection_minHealth;

    [Header("Ammo---------")]
    public List<Ammo> ammo;

    [Header("Grenades-----")]
    public List<Ammo> grenades;

    [Header("Medicals--------")]
    public List<Ammo> medicals;

    public float antizenCureFactor;
    public float bandageHealFactor;

    void Update()
    {
        if(health <= 0)
        {
            Die();
            health = 0;
        }

        if(health < healFactor_maxHealth)
        {
            if (health + healFactor * Time.deltaTime > healFactor_maxHealth)
                health = healFactor_maxHealth;
            else
                health += healFactor * Time.deltaTime;
        }

        if(health > infection_minHealth)
        {
            if (health - infection * infection_healthDrainDelta * Time.deltaTime < infection_minHealth)
                health = infection_minHealth;
            else
                health -= infection * infection_healthDrainDelta * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && medicals[0].count > 0)
        {
            infection -= antizenCureFactor;
            if (infection < 0)
                infection = 0;
            medicals[0].count -= 1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && medicals[1].count > 0)
        {
            Heal(bandageHealFactor);
            medicals[1].count -= 1;
        }


        //Debug.Log(health);
    }

    public void TakeDamage(float damage)
    {
        health -= damage * (1 - armour);
    }

    public void Heal(float heal)
    {
        health = health >= (maxHealth - heal) ? maxHealth : health + heal; //I really don't know what the fuck '?' does but we'll see
    }

    public void Infect(float infect)
    {
        infection = infection + infect >= maxInfection ? maxInfection : infection + infect;
    }

    public void Die()
    {
        GetComponent<BasicMovement>().enabled = false;
        GetComponent<WeaonManager>().activeWeapon.SetActive(false);
        GetComponent<WeaonManager>().enabled = false;
        GetComponent<GrenadeManager>().enabled = false;
    }
}
