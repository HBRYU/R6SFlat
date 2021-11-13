using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV_set : MonoBehaviour
{
    public GameObject FOV;
    public float viewAngle;
    public float viewDistance;
    public float meshVerticesCount;
    public float angleOffset;
    private PlayerFOV fov;

    // Start is called before the first frame update
    void Start()
    {
        fov = Instantiate(FOV, Vector3.zero, Quaternion.identity).GetComponent<PlayerFOV>();
        fov.targetPlayer = transform;
        fov.SetOrigin(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        fov.viewDistance = viewDistance;
        fov.fov = viewAngle;
        fov.angleOffset = angleOffset;
    }
}
