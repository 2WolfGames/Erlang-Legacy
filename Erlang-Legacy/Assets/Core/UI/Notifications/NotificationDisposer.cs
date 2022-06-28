using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI.Notifications
{
    public class NotificationDisposer : MonoBehaviour
    {
        [SerializeField] GameObject notification;

        public static NotificationDisposer Instance;

        private void Awake()
        {
            var matches = FindObjectsOfType<NotificationDisposer>();

            if (matches.Length > 1)
                Destroy(gameObject);
            else Instance = this;
        }

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