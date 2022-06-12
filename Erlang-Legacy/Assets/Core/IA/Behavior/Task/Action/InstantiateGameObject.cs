using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace Core.IA.Behavior.Task.Action
{
    public class InstantiateGameObject : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedGameObject prefab;

        public override void OnStart()
        {
            GameObject.Instantiate(prefab.Value, transform.position, Quaternion.identity);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

    }
}