using UnityEngine;

namespace Core.GameSession
{
    public class SaveGame : MonoBehaviour
    {
        [SerializeField] Animator animator;
        bool canBeSaved = false;

        //pre: GameSessionController.Instance != null
        //post: saves player state when requested 
        void Update()
        {
            if (canBeSaved)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    canBeSaved = false;
                    GameSessionController.Instance?.SavePlayerState(transform);
                    if (!GameSessionController.Instance)
                    {
                        Debug.LogWarning("Game session controller not found in scene, can handle save player state");
                    }
                    animator.SetBool("saved",true);
                }
            }
        }

        //pre: --
        //post: if player is in range canBesaved = true
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                canBeSaved = true;
            }
        }

        //pre: --
        //post: if player exits range canBesaved = false
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                canBeSaved = false;
                animator.SetBool("saved",false);
            }
        }
    }
}
