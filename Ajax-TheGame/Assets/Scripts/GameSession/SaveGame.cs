using UnityEngine;

namespace Core.GameSession
{
    public class SaveGame : MonoBehaviour
    {
        bool canBeSaved = false;

        void Update()
        {
            if (canBeSaved)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    canBeSaved = false;
                    GameSessionController.Instance.SavePlayerState(transform);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                canBeSaved = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                canBeSaved = false;
            }
        }
    }
}
