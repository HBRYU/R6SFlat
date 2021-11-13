using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInSight : MonoBehaviour
{
    private SpriteRenderer sr;

    public bool inSight;
    
    public bool playerTarget;
    public Transform target;
    public LayerMask wallLayers;
    public LayerMask wallLayersMinusFOVBlocker;
    public float maxDist;
    public float showDist;
    public bool useAnim;
    private Animator anim;

    public bool selfRaycast;
    public Transform[] raycastPos;

    // Start is called before the first frame update
    void Start()    //----처음에 한번 실행----
    {
        Physics2D.queriesStartInColliders = false;

        sr = GetComponent<SpriteRenderer>();
        if (useAnim)
            anim = GetComponent<Animator>();
        if (playerTarget)
            target = UTIL.GetPlayer().transform;
    }

    // Update is called once per frame
    void Update()   //----매 프레임마다 한번씩 실행----
    {
        inSight = false;
        /*
        bool 타입 변수 inSight
        inSight = false: 시야에 안보임, inSight = true: 보임
        */
        if (!selfRaycast)
        {
            foreach (Transform pos in raycastPos)   //raycastPos라는 리스트 안에 있는 모든 Transform 타입 변수(pos)에 대해
            {
                RaycastHit2D cast = Physics2D.Raycast(pos.position, target.position - pos.position, UTIL.FastDist(target.position, pos.position, 0.05f), wallLayers);
                /*
                RaycastHit2D 타입 변수 cast
                pos의 위치에서 Transform 타입 변수 target(플레이어)의 방향으로 target(플레이어) 까지의 거리만큼
                레이저를 wallLayers(벽 레이어)에서 발사
                 */
                if ((cast.collider == null && UTIL.FastDist(target.position, pos.position, 0.05f) <= maxDist) || UTIL.FastDist(target.position, pos.position, 0.05f) <= showDist)
                    inSight = true;
                /*
                만약 레이저에 아무것도 닿지 안았고 발사 지점과 너무 멀지 않았다면:
                inSight를 참으로 설정한다 => 보이는 거임
                 */
            }
        }
        else
        {
            RaycastHit2D cast = Physics2D.Raycast(transform.position, target.position - transform.position, UTIL.FastDist(target.position, transform.position, 0.05f), wallLayers);
            RaycastHit2D cast2 = Physics2D.Raycast(transform.position, target.position - transform.position, UTIL.FastDist(target.position, transform.position, 0.05f), wallLayersMinusFOVBlocker);
            if ((cast.collider == null && UTIL.FastDist(target.position, transform.position, 0.05f) <= maxDist) || (cast2.collider == null && UTIL.FastDist(target.position, transform.position, 0.05f) <= showDist))
                inSight = true;
        }
        


        //-------애니메이션(사라지기/보이기 효과)---------
        if (useAnim)
        {
            if (inSight)
                anim.SetBool("InSight", true);
            else
                anim.SetBool("InSight", false);
        }
        else
        {
            if (inSight)
                sr.enabled = true;
            else
                sr.enabled = false;
        }
        //Debug.Log("InSight: " + inSight);
    }
}
