using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Image HP;
    public Image STM;
    public TextMeshProUGUI HP_text;
    public TextMeshProUGUI STM_text;
    public TextMeshProUGUI weapon_name;
    public TextMeshProUGUI ammo_text;
    public Image ammo;
    public Image reload;
    public Image ARMR;
    public Image INFCTN;
    public TextMeshProUGUI ARMR_text;
    public TextMeshProUGUI INFCTN_text;
    public TextMeshProUGUI grenade_name;
    public TextMeshProUGUI grenade_count;

    public TextMeshProUGUI antizen_count;
    public TextMeshProUGUI bandage_count;

    private GameObject player;
    private GameObject gun;
    private Gun gun_script;
    private int ammo_mag;
    private int ammo_left;

    private float health;
    private float lastHealth;
    private float maxHealth;
    private float stamina;
    private float maxStamina;
    private PlayerStats stats;
    private BasicMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        player = UTIL.GetPlayer();
        stats = player.GetComponent<PlayerStats>();
        movement = player.GetComponent<BasicMovement>();
        lastHealth = stats.health;
    }

    // Update is called once per frame
    void Update()
    {
        gun = player.GetComponent<WeaonManager>().activeWeapon;
        gun_script = gun.GetComponent<Gun>();


        //Variables set
        health = stats.health;
        maxHealth = stats.maxHealth;

        stamina = movement.stamina;
        maxStamina = movement.maxStamina;

        foreach(Ammo a in stats.ammo)
        {
            if (a.name == gun_script.STATS.ammoType)
                ammo_left = Mathf.RoundToInt(a.count);
        }
        ammo_mag = gun_script.STATS.magSize_counter;

        float x = ammo_mag;
        if (x > ammo_left)
            x = ammo_left;
        float y = gun_script.STATS.magSize;
        //Image
        HP.fillAmount = health / maxHealth;
        if(health < lastHealth)
            HP_text.text = Mathf.Round(health) + "(-) / " + maxHealth;
        else if (health > lastHealth)
            HP_text.text = Mathf.Round(health) + "(+) / " + maxHealth;
        else
            HP_text.text = Mathf.Round(health) + " / " + maxHealth;
        lastHealth = health;
        STM.fillAmount = stamina / maxStamina;
        STM_text.text = Mathf.Round(stamina) + " / " + maxStamina;
        weapon_name.text = "[" + gun_script.STATS.ID + "]";
        ammo_text.text = " (" + gun_script.STATS.ammoType + ") / " + Mathf.Round(x) + " / " + ammo_left;
        ammo.fillAmount = x / y;
        reload.fillAmount = gun_script.STATS.reloadSpeed_timer / gun_script.STATS.reloadSpeed;
        ARMR.fillAmount = stats.armour;
        ARMR_text.text = (stats.armour*100).ToString() + "%";
        grenade_name.text = "[" + player.GetComponent<GrenadeManager>().activeGrenade.GetComponent<Grenade>().STATS.ID + "] [G]";
        foreach(Ammo grenade in stats.grenades)
        {
            if (grenade.name == player.GetComponent<GrenadeManager>().activeGrenade.GetComponent<Grenade>().STATS.ID)
                grenade_count.text = "× " + grenade.count.ToString();
        }
        INFCTN_text.text = ((stats.infection / stats.maxInfection) * 100).ToString() + "%";
        INFCTN.fillAmount = stats.infection / stats.maxInfection;

        antizen_count.text = "× " + stats.medicals[0].count.ToString();
        bandage_count.text = "× " + stats.medicals[1].count.ToString();

    }
}
