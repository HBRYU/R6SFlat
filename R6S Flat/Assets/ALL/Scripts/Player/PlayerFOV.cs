using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFOV : MonoBehaviour
{
    public LayerMask hitLayers;
    public Transform targetPlayer;
    private Mesh mesh;
    Vector3 origin;
    public float fov = 120f;
    float startingAngle;
    public float viewDistance = 15f;
    public int rayCount = 100;
    public float angleOffset;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        SetOrigin(targetPlayer.position);
        SetAimDirection((UTIL.MousePos() - targetPlayer.position).normalized);
    }


    void LateUpdate()
    {
        SetOrigin(targetPlayer.position);
        SetAimDirection((UTIL.MousePos() - targetPlayer.position).normalized);

        
        origin = GameObject.FindGameObjectWithTag("Player").transform.position;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        


        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        


        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D hit = Physics2D.Raycast(origin, UTIL.GetVectorFromAngle(angle), viewDistance, hitLayers);
            if (hit.collider == null)
            {
                vertex = origin + UTIL.GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = hit.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 dir)
    {
        startingAngle = UTIL.GetAngleFromVectorFloat(dir) + fov / 2f - 180f;
    }
}
