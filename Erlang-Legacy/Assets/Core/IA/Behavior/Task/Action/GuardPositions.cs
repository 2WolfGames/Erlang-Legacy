

using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.IA.Behavior.Action
{
    public class GuardPositions : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedTransform positionsWrapper;
        public SharedFloat movementSpeed = 1f;
        public SharedBool loop = false;
        public SharedInt currentIndex = 0;
        public SharedFloat threshold = 0.1f;
        public SharedBool forgetPosition = false;
        private List<Transform> transforms;

        public override void OnStart()
        {
            transforms = GetTransforms();
        }

        public override TaskStatus OnUpdate()
        {
            if (Completed())
                return TaskStatus.Success;
            Guard();
            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            if (forgetPosition.Value)
                currentIndex.Value = 0;
        }

        private bool Completed()
        {
            return !loop.Value && OneCycleDone();
        }

        private void Guard()
        {
            var currentPoint = transforms[currentIndex.Value];
            if (OnPoint(currentPoint))
                currentIndex = Clamp(currentIndex.Value + 1);
            var nextPoint = transforms[currentIndex.Value];
            MoveForward(nextPoint);
        }

        private bool OnPoint(Transform point)
        {
            return Vector2.Distance(point.position, transform.position) < threshold.Value;
        }

        private int Clamp(int index)
        {
            return index >= transforms.Count ? 0 : index;
        }

        private void MoveForward(Transform point)
        {
            transform.position = Vector2.MoveTowards(transform.position, point.position, movementSpeed.Value * Time.deltaTime);
        }

        private bool OneCycleDone()
        {
            return currentIndex.Value == transforms.Count;
        }

        private List<Transform> GetTransforms()
        {
            if (positionsWrapper.Value == null)
                return new List<Transform>();
            else
            {
                var positions = positionsWrapper.Value.GetComponentsInChildren<Transform>();
                var transforms = new List<Transform>();
                foreach (var position in positions)
                    if (position != positionsWrapper.Value)
                        transforms.Add(position);
                return transforms;
            }
        }
    }
}
