using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CompositeMap : MonoBehaviour
{
    public GameObject collisionMap;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(collisionMap, transform).GetComponent<TilemapCollider2D>().usedByComposite = true;
    }
}
