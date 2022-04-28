using System.Collections;
using Core.Player.Controller;
using UnityEngine;

public class DeadlyObject : MonoBehaviour
{
    const float cWaitTime = 0.5f;
    bool playerIn = false;

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

    private IEnumerator ResetSavePoint()
    {
        yield return new WaitForSeconds(cWaitTime);

        PlayerController playerController = PlayerController.Instance;

        playerController.Hurt(1, gameObject);
        if (playerController.PlayerData.Health.HP != 0)
            playerController.transform.position = GameSessionController.Instance.GetCurrentPoint();
        playerIn = false;
    }

}
