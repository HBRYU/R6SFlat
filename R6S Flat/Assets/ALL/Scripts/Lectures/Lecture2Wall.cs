using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lecture2Wall : MonoBehaviour
{
    public float maxHealth;
    public float health;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = new Color(1, 1, 1, health / maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
