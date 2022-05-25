using UnityEngine;

namespace Core.Utility
{
    public class Impulse : MonoBehaviour
    {
        public float power = 5f;
        public Vector2 direction = Vector2.up;

        public void Awake()
        {
            if (!GetComponent<Rigidbody2D>())
            {
                gameObject.AddComponent<Rigidbody2D>();
            }
        }

        public void Start()
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.AddForce(direction.normalized * power, ForceMode2D.Impulse);
        }
    }
}