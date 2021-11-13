using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [Header("Current speed: walk, sprint etc")]
    public float speed;
    public float speed_walk;
    public float speed_crouch;
    public float speed_sprint;
    public float speed_dying;
    [Tooltip("Used for diagonal movement")]
    public float diag_movementDelta = 0.75f; //루트(0.5) = 0.707~~
    public bool smoothMovement;
    
    public float rotSpeed;

    [Header("Stamina (체력)")]
    public float stamina;
    public float maxStamina;
    public float staminaUsageDelta;
    public float staminaIncrease;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void LookAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotSpeed);
    }


    void FixedUpdate()
    {
        float x = !smoothMovement ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        float y = !smoothMovement ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftControl))
            speed = speed_crouch;
        else if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            speed = speed_sprint;
        else
            speed = speed_walk;

        if (x * y != 0)
            speed *= diag_movementDelta;

        Vector2 movePos = transform.position;
        movePos.x += x * speed;
        movePos.y += y * speed;

        rb.MovePosition(movePos);
        LookAtMouse();

        if(Input.GetKey(KeyCode.LeftShift) && (x != 0 || y != 0))
            stamina = stamina - speed * staminaUsageDelta <= 0 ? 0 : stamina - speed * staminaUsageDelta;
        else
            stamina = stamina + staminaIncrease >= maxStamina ? maxStamina : stamina + staminaIncrease;
    }
}
