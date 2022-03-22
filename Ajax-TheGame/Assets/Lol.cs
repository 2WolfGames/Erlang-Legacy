using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Shared;

public class Lol : MonoBehaviour
{
    [SerializeField] Transform origin;
    [SerializeField] LayerMask playerMask;
    [Range(0, 360)][SerializeField] float visualAngle = 3f;
    [SerializeField] float visualAcuity = 20f;
    [Range(1, 20)][SerializeField] int density = 10;

    // Update is called once per frame
    void FixedUpdate()
    {
        Function.LookAround(
            origin,
            Vector2.right * transform.localScale.x,
            visualAcuity,
            visualAngle,
            density,
            playerMask);
    }
}
