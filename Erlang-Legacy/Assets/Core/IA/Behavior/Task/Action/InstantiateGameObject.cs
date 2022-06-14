using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.IA.Behavior.Task.Action
{
    public class InstantiateGameObject : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedGameObject prefab;
        public SharedTransform origin;

        public override void OnStart()
        {
            if (origin.Value == null)
                origin.Value = transform;
            GameObject.Instantiate(prefab.Value, origin.Value.position, Quaternion.identity);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

    }
}