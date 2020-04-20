using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public float maxSpeed;
    public float acceleration;
    public float decceleration;

    public float Speed;
    Vector2 moveDirection;
    bool isFacingRight;
    
    public Transform Graph;
    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection = moveDirection.normalized;

        if(moveDirection.x > 0.1f || moveDirection.x < -0.1f || moveDirection.y > 0.1f || moveDirection.y < -0.1f)
        {
            Speed += acceleration * Time.deltaTime ;
            Speed = Mathf.Clamp(Speed, 0, maxSpeed);
            anim.SetBool("IsMoving", true);
        }
        else
        {
            Speed = Mathf.MoveTowards(Speed, 0, decceleration);
            anim.SetBool("IsMoving", false);
        }

        CheckIsFacingRight();
    }

    private void FixedUpdate()
    {
        //rb.MovePosition((Vector2)transform.position + moveDirection * Speed * Time.deltaTime);
        rb.velocity = moveDirection * Speed * Time.deltaTime;
    }

    void CheckIsFacingRight()
    {
        if(moveDirection.x > 0.1f)
        {
            isFacingRight = true;
        }
        else if (moveDirection.x < -0.1f)
        {
            isFacingRight = false;
        }

        //if(isFacingRight)
        //{
        //    Graph.localScale = new Vector3(1, 1, 1);
        //}else
        //{
        //    Graph.localScale = new Vector3(-1, 1, 1);
        //}
    }
}
