using Core.Shared.Enum;
using UnityEngine;

namespace Core.Utility
{
    public class CorpsesManager : MonoBehaviour
    {
        private enum SpawnType
        {
            Global,
            Local
        }

        [SerializeField] private SpawnType spawnType;
        
        [Tooltip("Only required if spawn type is setted to local")]
        [SerializeField] private Transform localSpawnPoint;

        public static CorpsesManager Instance;

        public void Awake()
        {
            Instance = this;
        }

        public void Spawn(SpriteRenderer sprite, Vector2 position)
        {
            var instance = Instantiate(sprite, position, Quaternion.identity);
            Attach(instance.gameObject);
        }

        public void Spawn(SpriteRenderer sprite, Vector2 position, Face facing)
        {
            var instance = Instantiate(sprite, position, facing == Face.Left ? Quaternion.Euler(0, -180, 0) : Quaternion.identity);
            Attach(instance.gameObject);
        }

        private void Attach(GameObject other)
        {
            switch (spawnType)
            {
                case SpawnType.Global:
                    other.transform.SetParent(null);
                    break;
                case SpawnType.Local:
                    if (!localSpawnPoint)
                    {
                        Debug.LogWarning("No local spawn point set, defaulting to global");
                        other.transform.SetParent(null);
                    }
                    else
                    {
                        other.transform.SetParent(localSpawnPoint);
                    }
                    break;
            }
        }
    }
}

