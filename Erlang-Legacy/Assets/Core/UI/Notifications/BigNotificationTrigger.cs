using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigNotificationTrigger : MonoBehaviour
{

    [SerializeField] Sprite sprite;
    [SerializeField] string title;
    [SerializeField] string description;

    bool activated = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player" && !activated)
            {
                activated = true;
                BigNotificationManager.Instance?.ShowNotification(sprite,title,description);
            }
        }
}
