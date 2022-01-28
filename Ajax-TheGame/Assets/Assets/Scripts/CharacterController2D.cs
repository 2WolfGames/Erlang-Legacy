using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .2f)] [SerializeField] float movementSmothing = 0.05f;

    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] Transform groundedDetectorCenter;

    [SerializeField] Vector2 groundedDetectorDimentions;

    Rigidbody2D rigidbody2d;

    Vector2 horientation = Vector2.zero;

    bool grounded = false;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ListenHorientation();
    }

    void FixedUpdate()
    {
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            groundedDetectorCenter.position,
            groundedDetectorDimentions,
            0,
            groundLayerMask);

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject != gameObject)
            {
                grounded = true;
                break;
            }
        }

        if (Mathf.Abs(horientation.x) > Mathf.Epsilon)
        {
            ChangeHorientation(horientation.x <= Mathf.Epsilon ? -1 : 1);
        }

    }

    // This method updates current velocity state
    // we only update velocity.y when abs(yval) > 0
    // and main character is touching the ground
    public void Move(float xvel, float yvel)
    {
        rigidbody2d.velocity = new Vector2(xvel, rigidbody2d.velocity.y);

        if (Mathf.Abs(yvel) <= Mathf.Epsilon) return;

        if (!IsGrounded()) return;

        rigidbody2d.velocity = rigidbody2d.velocity + new Vector2(0, yvel);
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    // -1 left, 1 right 
    void ChangeHorientation(int horientation)
    {
        if (horientation != -1 && horientation != 1) return;

        if (horientation == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }

    void ListenHorientation()
    {
        if (rigidbody2d.velocity != Vector2.zero)
        {
            horientation = rigidbody2d.velocity.normalized;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundedDetectorCenter.position, groundedDetectorDimentions);
    }
}
