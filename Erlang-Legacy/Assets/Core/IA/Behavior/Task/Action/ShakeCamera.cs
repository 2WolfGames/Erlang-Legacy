
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Manager;
using UnityEngine;

namespace Core.IA.Task.Action
{
    [TaskDescription("Shake the camera")]
    public class ShakeCamera : BehaviorDesigner.Runtime.Tasks.Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The intensity of the shake")]
        public SharedFloat intensity = 1f;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The frequency of the shake")]
        public SharedFloat frequency = 1f;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The duration of the shake")]
        public SharedFloat duration = 1f;
        public SharedBool forceShake = true;

        public override TaskStatus OnUpdate()
        {
            CameraManager.Instance?.ShakeCamera(intensity.Value, frequency.Value, duration.Value, forceShake.Value);
            if (CameraManager.Instance == null)
            {
                Debug.LogWarning("CameraManager is null");
            }
            return TaskStatus.Success;
        }
    }
}