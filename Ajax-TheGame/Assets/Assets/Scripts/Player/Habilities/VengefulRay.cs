using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulRay : MonoBehaviour
{
    [Header("Configurations")]
    [Range(10, 1000)] [SerializeField] float damage = 100;
    [Range(10, 100)] [SerializeField] float velocity;

    [Tooltip("Delay for auto destroying")]
    [Range(1, 100)] [SerializeField] float countDown = 10f;

    [Header("Others")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CapsuleCollider2D capsule;

    ////////////////////////////////////////////////////////////////////////////////////////////////

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

    ////////////////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        StartCoroutine(Suicide(this.countDown));
    }

    IEnumerator Suicide(float countDown)
    {
        yield return new WaitForSeconds(countDown);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rb.velocity = orientation * velocity;
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

            var velocityX = rb.velocity.normalized.x;

            var centerX = capsule.bounds.center.x;
            var centerY = capsule.bounds.center.y;
            // var extentsX = capsule.bounds.extents.x;
            // // var hit = Instantiate(hitParticle, new Vector2(centerX + (velocityX >= 0 ? 1.25f * extentsX : -1.25f * extentsX), centerY), velocityX >= 0 ? Quaternion.identity : Quaternion.Euler(0, -180, 0));

            // Destroy(hit.gameObject, 1f);
            Destroy(gameObject);
        }

    }
}
