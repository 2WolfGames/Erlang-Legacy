using System.Collections;
using System.Collections.Generic;
using Core.Character.Enemy;
using UnityEngine;


namespace Core.Character.Player.Ability
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

        // pre: --
        // post: search enemy controller in parent because of mechanism to 
        //        pass throught enemies without colliding
        private void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = other.gameObject.GetComponentInParent<BaseEnemy>();
            if (enemy == null) return;
            if (distinct.Contains(enemy.gameObject)) return;
            distinct.Add(enemy.gameObject);
            enemy.Hurt(damage);
        }
    }
}
