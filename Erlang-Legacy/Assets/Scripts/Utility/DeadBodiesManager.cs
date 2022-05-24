using Core.Shared.Enum;
using DG.Tweening;
using UnityEngine;

namespace Core.Utility
{
    public class DeadBodiesManager : MonoBehaviour
    {
        [SerializeField] bool cleanBodiesCollected = false;

        public static DeadBodiesManager Instance;

        public void Awake()
        {
            Instance = this;
        }

        public void Spawn(SpriteRenderer sprite, Vector2 position)
        {
            var instance = Instantiate(sprite, position, Quaternion.identity);
            AddToBodiesCollector(instance.gameObject);
        }

        public void Spawn(SpriteRenderer sprite, Vector2 position, Face facing)
        {
            var instance = Instantiate(sprite, position, facing == Face.Left ? Quaternion.Euler(0, -180, 0) : Quaternion.identity);
            AddToBodiesCollector(instance.gameObject);
        }

        private void AddToBodiesCollector(GameObject other)
        {
            other.transform.SetParent(transform);
        }

        public void OnDestroy()
        {
            if (cleanBodiesCollected)
            {
                var children = GetComponentInChildren<Transform>();
                foreach (Transform child in children)
                {
                    if (child)
                    {
                        Destroy(child);
                    }
                }
            }
        }
    }
}

