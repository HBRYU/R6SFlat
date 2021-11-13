using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform target;
    public float speed;

    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movePos = Vector3.Lerp(transform.position, target.position, speed);
        movePos.z = -10;
        transform.position = movePos;
    }
}
