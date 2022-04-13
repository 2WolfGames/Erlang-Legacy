using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] Core.Shared.Loader.Scene scene;
     
     private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ajax")){
            Core.Shared.Loader.Load(scene);
        }
     }
}
