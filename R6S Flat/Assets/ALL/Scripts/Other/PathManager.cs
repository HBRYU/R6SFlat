using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathManager : MonoBehaviour
{
    public float scanInterval;
    private float scanInterval_timer;


    void Update()
    {
        scanInterval_timer += Time.deltaTime;
        if(scanInterval_timer >= scanInterval)
        {
            GetComponent<AstarPath>().Scan();
            scanInterval_timer = 0;
        }
    }
}
