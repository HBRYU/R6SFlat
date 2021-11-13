using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBTesting : MonoBehaviour
{
    public Rigidbody2D rb;

    public float floatDistance;
    public float floatForce;

    public Transform ground;


    void Update()
    {
        float height = transform.position.y - ground.position.y;
        float force = 0;

        if(floatDistance >= height)
            force = floatForce * (floatDistance - height);

        rb.velocity = new Vector2(0, rb.velocity.y + force);
    }
}
