using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player.Ability
{
    public class VengefulRay : MonoBehaviour
    {
        [Header("Configurations")]
        [Range(10, 1000)][SerializeField] int damage = 100;
        [Range(10, 100)][SerializeField] float velocity = 10f;

        [Tooltip("Delay for auto destroying")]
        [Range(1, 100)][SerializeField] float countDown = 10f;

        [Header("Others")]
        [SerializeField] Rigidbody2D ownRigidbody;
        // [SerializeField] Collider2D c2d;

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
            ownRigidbody.gravityScale = 0;
            ownRigidbody.velocity = Vector2.zero;
            StartCoroutine(Suicide(this.countDown));
        }

        IEnumerator Suicide(float countDown)
        {
            yield return new WaitForSeconds(countDown);
            Destroy(gameObject);
        }

        void FixedUpdate()
        {
            ownRigidbody.velocity = orientation * velocity;
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
}

