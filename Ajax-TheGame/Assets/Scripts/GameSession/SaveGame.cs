using UnityEngine;

namespace Core.GameSession
{
    public class SaveGame : MonoBehaviour
    {
        GameSessionController gameSessionController;
        bool canBeSaved = false;

        private void Awake()
        {
            gameSessionController = FindObjectOfType<GameSessionController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (canBeSaved)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    Debug.Log(transform);
                    canBeSaved = false;
                    gameSessionController.SavePlayerState(transform);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
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