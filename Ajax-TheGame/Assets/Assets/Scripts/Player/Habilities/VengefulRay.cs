using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulRay : MonoBehaviour
{
    [Range(10, 1000)] [SerializeField] float damage = 100;

    [Range(10, 100)] [SerializeField] float velocity;

    [SerializeField] Rigidbody2D rb;

    void Start()
    {
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
    }

    Vector2 _orientation = Vector2.zero;

    public Vector2 orientation
    {
        set
        {
            _orientation = value;
        }
        get
        {
            return _orientation;
        }
    }

    HashSet<GameObject> distinct = new HashSet<GameObject>();

    void Update()
    {
        // Debug.Log(rb.velocity);
    }

    void FixedUpdate()
    {
        rb.velocity = orientation * velocity;
        // rb.velocity = new Vector2(-30, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent(typeof(IEnemy)))
        {
            if (!distinct.Contains(other.gameObject))
            {
                distinct.Add(other.gameObject);
                IEnemy enemy = other.GetComponent<IEnemy>();
                enemy.OnHit(damage);
            }
        }
    }
}
