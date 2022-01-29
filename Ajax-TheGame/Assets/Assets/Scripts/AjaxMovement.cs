using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxMovement : MonoBehaviour
{
    [SerializeField] Transform feetRef;

    [SerializeField] Vector2 feetDimentions;

    [SerializeField] LayerMask whatIsGround;

    [SerializeField] float speed;

    [SerializeField] float xOrientation = 1;

    [SerializeField] float jumpForce = 2;

    [SerializeField] float jumpTime = 0.3f;

    Rigidbody2D rb;
    float jumpTimeCounter = 0.2f;

    bool isGrounded = true;

    bool isJumping = false;

    bool shouldJump = false;

    bool holdingJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        xOrientation = Input.GetAxisRaw("Horizontal");
        SmoothJump();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(xOrientation * speed, rb.velocity.y);
    }

    void SmoothJump()
    {
        isGrounded = Physics2D.OverlapBox(feetRef.position, feetDimentions, 0, whatIsGround);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(feetRef.position, feetDimentions);
    }
}
