using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Core.Combat.IA.Behavior.Action
{
    // description: moves current IA to point
    public class Recoil : BehaviorDesigner.Runtime.Tasks.Action
    {
        public float recoilScale = 0f;
        public SharedVector2 recoilDirection = Vector2.zero;
        private Rigidbody2D body => GetComponent<Rigidbody2D>();

        public override void OnStart()
        {
            ApplyRecoil(recoilDirection.Value);
        }

        private void ApplyRecoil(Vector2 direction)
        {
            Vector2 recoilForce = direction.normalized * recoilScale;
            body.bodyType = RigidbodyType2D.Dynamic;
            body.AddForce(recoilForce, ForceMode2D.Impulse);
        }
    }
}
