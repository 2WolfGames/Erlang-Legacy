using System.Collections;
using Core.GameSession;
using Core.Player.Controller;
using UnityEngine;


// TODO: rename this class
namespace Core.Hazard
{
    public class DeadlyObject : MonoBehaviour
    {
        [SerializeField] int damage = 1;
        const float cWaitTime = 0.5f;
        bool playerIn = false;

        //pre: --
        //post: if object != player enters to water it's destroyed
        //      if player enters, takes one life and if player lifes > 0
        //      brings it back to savePoint 
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!playerIn && other.gameObject.tag == "Player")
            {
                playerIn = true;
                StartCoroutine(ResetSavePoint());
            }
            else if (other.gameObject.tag != "Player")
            {
                Destroy(other.gameObject);
            }
        }

        //pre: PlayerController.Instance != null
        //     GameSessionController.Instance != null
        //post: post afer waitTime hurts player and if is not death 
        //      returns player to its last saved position
        private IEnumerator ResetSavePoint()
        {
            PlayerController player = PlayerController.Instance;
            bool playerSurvives = player.PlayerData.Health.HP - damage > 0;

            player.Hurt(damage, gameObject);

            yield return new WaitForSeconds(cWaitTime);

            if (playerSurvives)
                player.transform.position = GameSessionController.Instance.currentSavePos;

            playerIn = false;
        }
    }
}
