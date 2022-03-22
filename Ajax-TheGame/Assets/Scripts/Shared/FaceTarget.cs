using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleAccordingToSpeed : MonoBehaviour
{
    Rigidbody2D body;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Debug.Log(body.velocity);
        if (Mathf.Abs(body.velocity.x) > 0)
        {
            transform.DOScaleX(body.velocity.x > 0 ? 1 : -1, 0f);
        }
    }

}
