using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open;
    public float accessDistance;
    public Transform doorPos;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if((doorPos != null) && (UTIL.FastDist(UTIL.GetPlayer().transform.position, doorPos.position, 0.1f) <= accessDistance) && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("open", !open);
            open = !open;
        }
    }
}
