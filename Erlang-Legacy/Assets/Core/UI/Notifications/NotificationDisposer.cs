using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI.Notifications
{
    public class NotificationDisposer : MonoBehaviour
    {
        [SerializeField] GameObject notification;

        public static NotificationDisposer Instance;

        //pre: --
        //post: if these is no NotificationDisposer this becomes the one
        //      else it destroys itself
        private void Awake()
        {
            var matches = FindObjectsOfType<NotificationDisposer>();

            if (matches.Length > 1)
                Destroy(gameObject);
            else Instance = this;
        }

        //pre: --
        //post: instanciates a new notification 
        public void NewNotification(string title, string description, Sprite sprite, float seconds)
        {
            var currentNotification = Instantiate(notification, this.transform);
            NotificationBehaviour notificationBehaviour = currentNotification.GetComponent<NotificationBehaviour>();
            if (notificationBehaviour != null)
            {
                StartCoroutine(notificationBehaviour.DisplayNotification(
                    title,
                    description,
                    sprite,
                    seconds
                ));
            }
            else
            {
                Destroy(currentNotification);
            }
        }
    }
}