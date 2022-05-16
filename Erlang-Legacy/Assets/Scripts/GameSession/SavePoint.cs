using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameSession
{
    public class SavePoint : MonoBehaviour
    {
        //pre: GameSessionController.Instance != null
        //post: currentplayer save point is this transform.position
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                GameSessionController.Instance.SavePlayerCurrentPoint(transform);
            }
        }

    }
}
