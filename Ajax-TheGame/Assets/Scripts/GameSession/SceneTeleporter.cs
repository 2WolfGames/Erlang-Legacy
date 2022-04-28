using Core.Shared;
using Core.Shared.Enum;
using UnityEngine;

namespace Core.GameSession
{
    public class SceneTeleporter : MonoBehaviour
    {
        [SerializeField] SceneID scene;
        [SerializeField] EntranceID entranceTag;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                GameSessionController.Instance.SearchCurrentPoint(entranceTag);
                StartCoroutine(Loader.LoadWithDelay(scene, 0));
            }
        }
    }
}
