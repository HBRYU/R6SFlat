using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public GameObject[] guns_default_bulletHit;
    public GameObject guns_default_bulletLR;
    public GameObject guns_default_muzzleFlash;
    public Color guns_default_bulletColor;
    public LayerMask guns_default_hitLayers;

    public LayerMask explosives_default_hitLayers;

    public List<GunStats> guns;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
