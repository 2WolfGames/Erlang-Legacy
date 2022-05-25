using Core.Shared;
using Core.Shared.Enum;
using UnityEngine;

namespace Core.GameSession
{
    public class SceneTeleporter : MonoBehaviour
    {
        [SerializeField] SceneID scene;
        [SerializeField] EntranceID entranceTag;

        //pre: GameSessionController.Instance != null
        //post: changes current scene to sceneID
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                GameSessionController.Instance.NextSceneEntrance(entranceTag);
                StartCoroutine(Loader.LoadWithDelay(scene, 0));
            }
        }
    }
}
