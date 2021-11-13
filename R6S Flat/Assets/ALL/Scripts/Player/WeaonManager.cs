using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaonManager : MonoBehaviour
{
    public GameObject activeWeapon;
    public List<GameObject> weapons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (activeWeapon != weapons[weapons.Count - 1])
                activeWeapon = weapons[weapons.IndexOf(activeWeapon) + 1];
            else
                activeWeapon = weapons[0];
            activeWeapon.SetActive(true);
        }
        foreach(GameObject weapon in weapons)
        {
            if (weapon != activeWeapon)
                weapon.SetActive(false);
        }
    }
}
