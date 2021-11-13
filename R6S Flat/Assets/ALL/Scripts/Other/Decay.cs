using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decay : MonoBehaviour
{
    public float lifeSpan;
    [Tooltip("lifeSpan += Random.Range(0, randomRange);")]
    public float randomRange;

    // Start is called before the first frame update
    void Start()
    {
        lifeSpan += Random.Range(0, randomRange);
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
            Destroy(gameObject);
    }
}
