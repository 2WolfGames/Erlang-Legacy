
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.IA;
using Core.IA.Bahavior.SharedVariable;
using Core.Manager;
using UnityEngine;

namespace Core.IA.Task.Action
{
    public class RamWall : EnemyAction
    {
        public SharedCollider2D wallDetector;
        public SharedLayerMask wallLayer;
        public SharedFloat ramSpeed = 10f;
        private Vector2 direction = Vector2.zero;
        public SharedBool shakeCameraOnRam = true;
        public SharedFloat shakeCameraIntensity = 5f;

        public override void OnStart()
        {
            direction = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        }

        public override TaskStatus OnUpdate()
        {
            if (wallDetector.Value.IsTouchingLayers(wallLayer.Value))
            {
                if (shakeCameraOnRam.Value)
                {
                    CameraManager.Instance?.ShakeCamera(shakeCameraIntensity.Value);
                    if (CameraManager.Instance == null)
                    {
                        Debug.LogWarning("CameraManager is null");
                    }
                }
                return TaskStatus.Success;
            }
            body.velocity = direction * ramSpeed.Value;
            return TaskStatus.Running;
        }
    }
}