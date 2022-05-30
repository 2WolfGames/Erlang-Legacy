using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.Combat.IA.Behavior.Action
{
    public class Recoil : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedFloat recoilScale = 0f;
        public SharedVector2 recoilDirection = Vector2.zero;
        private Rigidbody2D body => GetComponent<Rigidbody2D>();

        public override void OnStart()
        {
            ApplyRecoil(recoilDirection.Value);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        private void ApplyRecoil(Vector2 direction)
        {
            Vector2 recoilForce = direction.normalized * recoilScale.Value;
            // Debug.Log(recoilForce);
            body.bodyType = RigidbodyType2D.Dynamic;
            body.AddForce(recoilForce, ForceMode2D.Impulse);
        }
    }
}
