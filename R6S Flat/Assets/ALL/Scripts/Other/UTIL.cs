using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class UTIL : MonoBehaviour
{
    public LayerMask StandardUnitWallLayers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject GetPlayer()
    {
        return (GameObject.FindGameObjectWithTag("Player"));
    }

    public static GameObject GetGM()
    {
        return (GameObject.FindGameObjectWithTag("GM"));
    }

    public static UTIL GetUTIL()
    {
        return (GameObject.FindGameObjectWithTag("GM").GetComponent<UTIL>());
    }

    public static float FastDist(Vector2 pos1, Vector2 pos2, float step)
    {
        float distSqr = (pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y);
        for(float f = 0f; f < 99f; f += step)
        {
            if (f*f >= distSqr)
                return (f);
        }
        return (-1);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <param name="sample"></param>
    /// <returns>1=Longer than sample, 0=Equals sample, -1=Shorter than sample</returns>
    public static int CompareDist(Vector2 pos1, Vector2 pos2, float sample)
    {
        float distSqr = (pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.y - pos2.y) * (pos1.y - pos2.y);
        if (distSqr > sample)
            return (1);
        else if (distSqr == sample)
            return (0);
        else
            return (-1);
    }

    public static Vector3 MousePos()
    {
        return (Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public static float Qsqrt(float num, float step)
    {
        float n = 0;
        if(num * num > 0)
        {
            while (n*n < num*num)
            {
                n += step;
            }
            return n;
        }
        return 0;
    }

    /// <summary>
    /// [Stanard Unit Wall Layers]
    /// Used by enemy units to identify walls
    /// </summary>
    /// <returns></returns>
    public static LayerMask GetSUWL()
    {
        LayerMask lm = GetUTIL().StandardUnitWallLayers;
        return (lm);
    }
}
