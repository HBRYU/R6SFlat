using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lecture2 : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;

    private float timer;

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            GameObject thisBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            thisBullet.GetComponent<Lecture2Bullet>().speed = bulletSpeed;
            timer = 0;
        }
    }
}
