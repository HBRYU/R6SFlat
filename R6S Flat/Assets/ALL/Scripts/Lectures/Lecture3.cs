using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;

public class Lecture3 : MonoBehaviour
{
    public float x;
    private float y = 0;

    public bool c;

    public int[] d;

    void Start()
    {
        //int 정수 : 1, 5, 0...
        //float 실수 : 5.2f, 2.33333...f
        //string 문자열 : "Hello, world!" , "ㅇㅇ", "asd123" ...
        //char 문자 : "a", "b", ...
        //bool 참 거짓 : true, false

        Debug.Log("d: " + d);
    }

    void Update()
    {
        // 초당 60프레임 => 1초 후에는 x가 얼마?
        // 70f/s, 10f/s 1000f/s
        x += 1; // x = x + 1, x++;
        y += Time.deltaTime;
        //print(x)
        Debug.Log("x: " + x); //x: 10
        Debug.Log("y: " + y); //y: 1
    }
}
