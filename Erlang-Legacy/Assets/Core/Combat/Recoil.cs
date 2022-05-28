
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Core.Combat
{
    public class Recoil : MonoBehaviour
    {
        public float recoilScale = 0f;
        private BehaviorTree behaviorTree => GetComponent<BehaviorTree>();
        private Rigidbody2D body => GetComponent<Rigidbody2D>();

        public void OnRecoil(Vector2 direction)
        {
            Vector2 norm = direction.normalized;
            if (behaviorTree) 
                NotifyRecoilEvent(norm);
            else ApplyRecoil(norm);
        }

        private void ApplyRecoil(Vector2 direction)
        {
            Vector2 recoilForce = direction.normalized * recoilScale;
            body.AddForce(recoilForce, ForceMode2D.Impulse);
        }

        private void NotifyRecoilEvent(Vector2 direction)
        {
            Debug.Log("NotifyRecoilEvent");
            behaviorTree.SendEvent("recoil"); // TODO: create static class to manage behavior events
        }

    }
}
