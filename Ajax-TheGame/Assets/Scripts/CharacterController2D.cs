using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    This class is thought to be generic,
    any character should use this class, it takes cares of
        - character orientation
    
    In future it may handle other common character eassues
*/

public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] Vector2 orientation = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        orientation = rb.velocity.normalized;
    }

    void FixedUpdate()
    {
        HandleCharacterHorientation();
    }

    void HandleCharacterHorientation()
    {
        if (Mathf.Abs(orientation.x) > 0.001)
        {
            UpdateCharacterOrientation(orientation.x > Mathf.Epsilon ? 1 : -1);
        }
    }

    // -1 left, 1 right 
    void UpdateCharacterOrientation(int orientation)
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

}
