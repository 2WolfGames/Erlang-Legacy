using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameSession
{
    public class SavePoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                GameSessionController.Instance.SavePlayerCurrentPoint(transform);
            }
        }

    }
}
