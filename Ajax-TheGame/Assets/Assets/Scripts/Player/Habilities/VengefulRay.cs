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
    }

    public Vector2 orientation
    {
        set; get;
    }

    HashSet<GameObject> distinct = new HashSet<GameObject>();

    void Update()
    {
        Debug.Log(orientation);
        rb.velocity = orientation * velocity;
    }

    void FixedUpdate()
    {
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
