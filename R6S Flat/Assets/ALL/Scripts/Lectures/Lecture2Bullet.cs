using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lecture2Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public float damage;

    public List<float> floats1; //list
    public float[] floats2;     //array

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
        
    void Update()
    {
        rb.velocity = new Vector2(speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Lecture2Wall>() != null)
        {
            collision.GetComponent<Lecture2Wall>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
