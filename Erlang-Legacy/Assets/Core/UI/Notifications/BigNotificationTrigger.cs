using DG.Tweening;
using UnityEngine;

namespace Core.UI.Notifications
{
    public class BigNotificationTrigger : MonoBehaviour
    {

        [SerializeField] Sprite sprite;
        [SerializeField] string title;
        [SerializeField] string description;
        [SerializeField] float delay = 0f;

        private bool activated = false;
        private Tween delayedCall;

        //pre: --
        //post: shows bug notification when player colides 
        //      only activates once.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player" && !activated)
            {
                activated = true;
                if (BigNotificationManager.Instance == null)
                {
                    Debug.LogWarning("BigNotificationManager is null");
                    return;
                }
                delayedCall = DOVirtual.DelayedCall(delay, () =>
                {
                    BigNotificationManager.Instance.ShowNotification(sprite, title, description);
                });
            }
        }
    }
}
