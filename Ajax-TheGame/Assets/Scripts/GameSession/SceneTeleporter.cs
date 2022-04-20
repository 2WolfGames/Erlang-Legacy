using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTeleporter : MonoBehaviour
{
    [SerializeField] Core.Shared.Loader.Scene scene;
    [SerializeField] Core.Shared.Loader.Entrance entranceTag;
     private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ajax")){
            FindObjectOfType<GameSessionController>()?.SearchCurrentPoint(entranceTag);
            Core.Shared.Loader.Load(scene);
        }
     }
}
