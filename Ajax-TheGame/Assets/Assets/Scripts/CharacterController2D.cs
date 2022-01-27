using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .2f)] [SerializeField] float movementSmothing = 0.05f;
    Rigidbody2D rigidbody2d;
    Vector2 velocity = Vector2.zero;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    // xvel & yvel -> [0.0 - 1.0]
    public void Move(float xvel, float yvel)
    {
        Vector2 v = new Vector2(xvel, yvel);
        Vector2 cv = rigidbody2d.velocity;
        rigidbody2d.velocity = Vector2.SmoothDamp(cv, v, ref velocity, movementSmothing); // modifies @velocity
    }
}
