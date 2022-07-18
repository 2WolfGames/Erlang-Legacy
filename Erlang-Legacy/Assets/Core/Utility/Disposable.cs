using UnityEngine;

namespace Core.Utility
{
    public class Disposable : MonoBehaviour
    {
        public float lifetime = 1f;
        public float Timeout { get => lifetime; set => lifetime = value; }

        public void Start()
        {
            Destroy(gameObject, Timeout);
        }
    }
}
