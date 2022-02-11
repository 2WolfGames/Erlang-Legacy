﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxMovement : MonoBehaviour
{
    [SerializeField] LayerMask whatIsGround;

    [SerializeField] float speed;

    [SerializeField] float dashSpeed;

    [SerializeField] float dashDuration;

    [SerializeField] float xOrientation = 1;

    [SerializeField] float jumpForce = 2;

    [SerializeField] float jumpTime = 0.3f;

    Rigidbody2D rb;

    BoxCollider2D boxCollider2D;

    float jumpTimeCounter = 0.2f;

    bool isJumping = false;

    bool hasJumped = false;

    bool dashing = false;

    AjaxFX ajaxFX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        ajaxFX = GetComponent<AjaxFX>();
    }

    void Update()
    {
        xOrientation = Input.GetAxisRaw("Horizontal");
        if (!dashing)
        {
            SmoothJump();
        }
    }

    void FixedUpdate()
    {
        if (!dashing)
        {
            rb.velocity = new Vector2(xOrientation * speed, rb.velocity.y);
            ajaxFX.SetRunFX(rb.velocity.x != 0);

            if (hasJumped && IsGrounded())
            {
                ajaxFX.TriggerLandFX();
                hasJumped = false;
            }
        }
    }

    void SmoothJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            this.ajaxFX.TriggerJumpFX();
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                hasJumped = true;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            hasJumped = true;
        }

    }

    // direction only support { -1, 1 }, meaning { left, right }
    public void Dash(int direction, System.Action onFinish = null)
    {
        if (direction != 1 && direction != -1) return;
        StartCoroutine(IDash(direction, onFinish));
    }

    // Method thought to be calle throw @Dash fn
    IEnumerator IDash(int direction, System.Action onFinish = null)
    {
        dashing = true;
        float gravityScale = this.rb.gravityScale;
        Freeze();
        this.rb.gravityScale = 0;
        ajaxFX.TriggerDashFX(dashDuration);
        this.rb.AddForce(new Vector2(dashSpeed * direction, 0f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDuration);
        Freeze();
        this.rb.gravityScale = gravityScale;
        dashing = false;
        if (onFinish != null) onFinish();
    }

    void Freeze()
    {
        this.rb.velocity = Vector2.zero;
    }

    bool IsGrounded()
    {
        float extra = 0.1f;
        RaycastHit2D ray = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, extra, whatIsGround);
        bool grounded = ray.collider != null;
        Color rayColor = grounded ? Color.green : Color.red;

        Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extra), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extra), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + extra), Vector2.right * (2 * boxCollider2D.bounds.extents.x), rayColor);

        return grounded;
    }

}
