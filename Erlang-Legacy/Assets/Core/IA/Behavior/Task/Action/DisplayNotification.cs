

using BehaviorDesigner.Runtime;
using Core.IA.Bahavior.SharedVariable;

namespace Core.IA.Task.Action
{
    public class DisplayNotification : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedString title;
        public SharedString description;
        public SharedSpriteRenderer sprite;

        public override void OnStart()
        {
            base.OnStart();
        }
    }
}