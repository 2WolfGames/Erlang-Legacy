using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AjaxMovement : MonoBehaviour
{
    [SerializeField] LayerMask whatIsGround;

    [SerializeField] float speed;

    [SerializeField] float dashSpeed;

    [SerializeField] float xOrientation = 1;

    [SerializeField] float jumpForce = 2;

    [SerializeField] float jumpTime = 0.3f;

    Rigidbody2D rb;

    BoxCollider2D boxCollider2D;

    float jumpTimeCounter = 0.2f;

    bool isJumping = false;

    bool hasJumped = false;

    bool dashing = false;

    bool impulsed = false;

    float gravityScale = 1;

    AjaxFX ajaxFX;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        ajaxFX = GetComponent<AjaxFX>();

        gravityScale = this.rb.gravityScale;
    }

    void Update()
    {
        xOrientation = Input.GetAxisRaw("Horizontal");
        if (!dashing)
        {
            SmoothJump();
        }
        // HandleLocalScale();
    }

    void FixedUpdate()
    {
        if (!dashing)
        {
            // when Ajax is at the air, we let him take certain control of it's movement
            float vx = impulsed ? rb.velocity.x + xOrientation * speed * 0.05f : xOrientation * speed;
            rb.velocity = new Vector2(vx, rb.velocity.y);
            ajaxFX.SetRunFX(Mathf.Abs(rb.velocity.x) > Mathf.Epsilon);

            if (hasJumped && IsGrounded())
            {
                ajaxFX.TriggerLandFX();
                hasJumped = false;
            }
        }
    }

    /// <sumary>
    /// freze normal control for a certain time applying the impulse.
    /// if anything had change its gravity, 
    // the methods recover its firt local gravity scale
    /// <sumary>
    public void ImpulseUp(float force)
    {
        this.rb.gravityScale = gravityScale;
        impulsed = true;
        Freeze();
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    /// <sumary>
    /// freze normal control for a certain time applying the impulse.
    /// if anything had change its gravity, 
    // the methods recover its firt local gravity scale
    /// <sumary>
    public void Impulse(Vector2 impulse)
    {
        this.rb.gravityScale = gravityScale;
        impulsed = true;
        Freeze();
        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    // direction only support { -1, 1 }, meaning { left, right }
    public void Dash(int direction, float duration, System.Action onComplete = null)
    {
        if (direction != 1 && direction != -1) return;
        StartCoroutine(IDash(direction, duration, onComplete));
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

    // Method thought to be calle throw @Dash fn
    IEnumerator IDash(int direction, float duration, System.Action onComplete = null)
    {
        dashing = true;
        float gravityScale = this.rb.gravityScale;
        Freeze();
        this.rb.gravityScale = 0;
        ajaxFX.TriggerDashFX(duration);
        this.rb.AddForce(new Vector2(dashSpeed * direction, 0f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);

        // avoids stack when you had dash
        // and them Ajax was trigger by impulse effect
        if (!impulsed)
        {
            Freeze();
        }
        this.rb.gravityScale = gravityScale;
        dashing = false;
        if (onComplete != null) onComplete();
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

    void OnCollisionEnter2D(Collision2D other)
    {
        // trigger effect ends when you had collide with something
        if (impulsed)
        {
            impulsed = false;
        }

    }

}
