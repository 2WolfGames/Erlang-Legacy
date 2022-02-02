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

    BoxCollider2D boxCollider2D;

    float jumpTimeCounter = 0.2f;

    bool isJumping = false;

    bool dashing = false;

    float defaultGravityScale = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
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
        bool isGrounded = IsGrounded();

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

    bool IsGrounded()
    {
        float extra = 0.25f;
        RaycastHit2D ray = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0, Vector2.down, extra, whatIsGround);
        bool grounded = ray.collider != null;
        Color rayColor = grounded ? Color.green : Color.red;

        Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extra), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extra), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + extra), Vector2.right * (2 * boxCollider2D.bounds.extents.x), rayColor);

        return grounded;
    }
}
