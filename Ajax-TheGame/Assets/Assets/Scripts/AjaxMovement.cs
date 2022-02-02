using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxMovement : MonoBehaviour
{
    [SerializeField] Transform feetRef;

    [SerializeField] Vector2 feetDimentions;

    [SerializeField] LayerMask whatIsGround;

    [SerializeField] float speed;

    [SerializeField] float dashSpeed;

    [SerializeField] float dashDuration;

    [SerializeField] float xOrientation = 1;

    [SerializeField] float jumpForce = 2;

    [SerializeField] float jumpTime = 0.3f;

    Rigidbody2D rb;
    float jumpTimeCounter = 0.2f;

    bool isGrounded = true;

    bool isJumping = false;

    bool dashing = false;

    float defaultGravityScale = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;
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
        }
    }

    void SmoothJump()
    {
        isGrounded = Physics2D.OverlapBox(feetRef.position, feetDimentions, 0, whatIsGround);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    // Ajax can dash by now can dash left or right
    // left: -1, right: 1
    public void Dash(int direction, System.Action callback = null)
    {
        if (direction != 1 && direction != -1) return;
        Debug.Log("Dashing..." + (direction == 1 ? " right" : " left"));

        StartCoroutine(IDash(direction, callback));

        if (callback != null)
        {
            callback();
        }
    }

    // Method thought to be calle throw @Dash fn
    IEnumerator IDash(int direction, System.Action callback = null)
    {
        dashing = true;
        Freeze();
        this.rb.gravityScale = 0;
        if (direction == -1)
        {
            this.rb.velocity = Vector2.left * dashSpeed;
        }
        else
        {
            this.rb.velocity = Vector2.right * dashSpeed;
        }
        yield return new WaitForSeconds(dashDuration);
        Freeze();
        this.rb.gravityScale = defaultGravityScale;
        dashing = false;
        if (callback != null) callback();
    }

    void Freeze()
    {
        this.rb.velocity = Vector2.zero;
    }
}
