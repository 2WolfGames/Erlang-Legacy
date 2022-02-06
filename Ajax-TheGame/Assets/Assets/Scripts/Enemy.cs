using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Harakiri(Vector2 orientation)
    {
        Vector3 force = new Vector2(10 * orientation.x, 0 * orientation.y);
        rb.velocity = rb.velocity + new Vector2(force.x, force.y);
        StartCoroutine(
            OnHarakiri(() =>
            {
                Destroy(gameObject);
            })
        );
    }

    IEnumerator OnHarakiri(System.Action callback)
    {
        yield return new WaitForSeconds(0.5f);
        callback();
    }


}
