using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Bahavior.SharedVariable;
using Core.UI.Notifications;
using UnityEngine;

namespace Core.IA.Task.Action
{
    public class DisplayNotification : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedString title = "This a notification";
        public SharedString description = "You can see this notification";
        public SharedSprite sprite;
        public SharedFloat notificationDelay = 0;

        public override void OnStart()
        {
            if (NotificationManager.Instance == null)
            {
                Debug.LogError("NotificationManager not found");
                return;
            }
            NotificationManager.Instance.PostNotificationWithDelay(title.Value, description.Value, sprite.Value, notificationDelay.Value);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}