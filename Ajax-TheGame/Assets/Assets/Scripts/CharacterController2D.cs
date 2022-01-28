using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .2f)] [SerializeField] float movementSmothing = 0.05f;

    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] Transform bodyCenter;

    [SerializeField] float bodyRadius;

    Rigidbody2D rigidbody2d;

    Vector2 velocity = Vector2.zero;

    bool grounded = false;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(bodyCenter.position, bodyRadius, groundLayerMask);

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject != gameObject)
            {
                grounded = true;
                break;
            }
        }
    }

    // This method updates current velocity state
    // we only update velocity.y when abs(yval) > 0
    // and main character is touching the ground
    public void Move(float xvel, float yvel)
    {
        rigidbody2d.velocity = new Vector2(xvel, rigidbody2d.velocity.y);

        if (Mathf.Abs(yvel) <= Mathf.Epsilon) return;

        if (!isGrounded()) return;

        rigidbody2d.velocity = rigidbody2d.velocity + new Vector2(0, yvel);
    }

    public bool isGrounded()
    {
        return grounded;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(bodyCenter.position, bodyRadius);
    }
}
