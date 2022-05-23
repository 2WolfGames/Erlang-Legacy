using Core.Shared.Enum;
using UnityEngine;

namespace Core.Utility
{
    public class DeadBodySpawner : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sprite;

        public void Awake()
        {
            if (!sprite)
            {
                Debug.LogError("none sprite instance setted");
            }
        }

        public void Spawn()
        {
            Instantiate(sprite, transform.position, Quaternion.identity);
        }

        public void Spawn(Vector2 position)
        {
            Instantiate(sprite, position, Quaternion.identity);
        }

        public void Spawn(Vector2 position, Face facing)
        {
            Instantiate(sprite, position, facing == Face.Left ? Quaternion.Euler(0, -180, 0) : Quaternion.identity);
        }

        public void Spawn(Vector2 position, Face facing, Vector2 force)
        {
            var instance = Instantiate(sprite, position, facing == Face.Left ? Quaternion.Euler(0, -180, 0) : Quaternion.identity);
            if (!instance.GetComponent<Rigidbody2D>())
            {
                instance.gameObject.AddComponent<Rigidbody2D>();
            }
            var rb = instance.GetComponent<Rigidbody2D>();
            rb.AddForce(facing == Face.Left ? force * Vector2.left : force * Vector2.right, ForceMode2D.Impulse);
        }
    }
}

