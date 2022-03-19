using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player.Ability
{
    public class Dash : MonoBehaviour
    {
        [Tooltip("Amount of life to take to enemies")]
        [Range(10, 1000)][SerializeField] int damage = 100;

        BoxCollider2D boxCollider;
        HashSet<GameObject> distinct = new HashSet<GameObject>();

        void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        public IEnumerator AttackCoroutine(float time)
        {
            boxCollider.enabled = true;
            distinct.Clear();
            yield return new WaitForSeconds(time);
            boxCollider.enabled = false;
            distinct.Clear();
        }

        /**
            configure using game properties
            those layers that can interact with this
            `Dash` object
        */
        private void OnTriggerEnter2D(Collider2D other)
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
