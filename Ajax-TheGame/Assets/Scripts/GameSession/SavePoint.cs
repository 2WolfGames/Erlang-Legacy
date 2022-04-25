using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    GameSessionController gameSessionController;
    
    private void Awake() {
        gameSessionController = FindObjectOfType<GameSessionController>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")){
            gameSessionController.SavePlayerCurrentPoint(transform);
        }
    }

    
}
