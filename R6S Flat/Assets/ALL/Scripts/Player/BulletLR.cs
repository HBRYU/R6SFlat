using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLR : MonoBehaviour
{
    public float visibleTime;
    public float initialSubtraction;
    public float decayTime;
    public float fadeDelta;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        visibleTime -= Time.deltaTime;
        if(visibleTime <= 0)
        {
            if(lr.endColor.a > 1-initialSubtraction)
            {
                lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, Mathf.Clamp01(lr.endColor.a - initialSubtraction));
                lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, Mathf.Clamp01(lr.startColor.a - initialSubtraction));
            }
            Color start_none = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, 0);
            Color end_none = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, 0);
            lr.startColor = Color.Lerp(lr.startColor, start_none, fadeDelta * Time.deltaTime);
            lr.endColor = Color.Lerp(lr.endColor, end_none, fadeDelta * Time.deltaTime);
            decayTime -= Time.deltaTime;
            if (decayTime <= 0)
                Destroy(gameObject);
        }
    }
}
