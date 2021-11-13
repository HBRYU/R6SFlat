using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float magnitude;
    public Transform camPos;
    Vector2 initPos;

    //[HideInInspector]
    public bool shake;

    //[HideInInspector]
    public float shakeDuration;
    public float shakeDuration_default;
    // Start is called before the first frame update
    void Start()
    {
        initPos = camPos.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shake)
        {
            Vector2 movePos = initPos;

            float m = magnitude * UTIL.Qsqrt(shakeDuration, 0.15f);

            movePos += new Vector2(Random.Range(-m, m), Random.Range(-m, m));
            camPos.localPosition = movePos;
        }
        else
            camPos.localPosition = initPos;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            shake = true;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shake = false;
            shakeDuration = 0;
        }
    }

    public void Shake(bool on)
    {
        if (on)
        {
            shake = true;
        }
        else
            shake = false;
    }
    public void Shake(float duration)
    {
        shake = true;
        if (duration > 0)
            shakeDuration = duration;
        else if (duration < 0)
            shakeDuration = shakeDuration_default;
        else
        {
            shakeDuration = 0;
            shake = false;
        }
    }
}
