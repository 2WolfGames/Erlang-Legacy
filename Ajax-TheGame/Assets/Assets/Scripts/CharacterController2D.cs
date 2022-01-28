using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] Transform groundedDetectorCenter;

    [SerializeField] Vector2 groundedDetectorDimentions;

    [SerializeField] int xOrientation = 1;

    Rigidbody2D rigidbody2d;


    bool grounded = false;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ListenOrientationChanges();
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
        UpdateOrientation(xOrientation);
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
    void UpdateOrientation(int orientation)
    {
        if (orientation != -1 && orientation != 1) return;

        if (orientation == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }

    // It should only update orientation
    // when there is a movement in horizontal axis
    void ListenOrientationChanges()
    {
        if (Mathf.Abs(rigidbody2d.velocity.x) > 0.001)
        {
            xOrientation = Mathf.RoundToInt(Mathf.Clamp01(rigidbody2d.velocity.x) * 2 - 1);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundedDetectorCenter.position, groundedDetectorDimentions);
    }
}
