using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    public GameObject activeGrenade;
    public List<GameObject> grenades;

    private PlayerStats stats;

    public float coolDown;
    private float coolDown_timer;

    // Start is called before the first frame update
    void Start()
    {
        coolDown_timer = coolDown;
        stats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        coolDown_timer -= Time.deltaTime;
        if((Input.GetKeyDown(KeyCode.G) || Input.GetMouseButtonDown(2)) && coolDown_timer <= 0)
        {
            for(int i = 0; i < stats.grenades.Count; i++)
            {
                if (stats.grenades[i].name == activeGrenade.GetComponent<Grenade>().STATS.ID && stats.grenades[i].count > 0)
                {
                    stats.grenades[i].count -= 1;
                    Instantiate(activeGrenade, transform.position, transform.rotation);
                }
            }
            coolDown_timer = coolDown;
        }
    }
}
