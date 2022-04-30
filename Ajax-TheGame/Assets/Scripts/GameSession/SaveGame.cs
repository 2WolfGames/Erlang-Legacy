﻿using UnityEngine;

namespace Core.GameSession
{
    public class SaveGame : MonoBehaviour
    {
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
                    GameSessionController.Instance.SavePlayerState(transform);
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
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                canBeSaved = false;
            }
        }
    }
}
