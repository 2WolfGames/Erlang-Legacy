using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Bahavior.SharedVariable;
using UnityEngine;


namespace Core.IA.Behavior.Plant
{
    public class InstantiateProjectile : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedBehaviorTree projectilePrefab;
        public SharedTransform origin;

        public override void OnStart()
        {
            if (origin.Value == null)
                origin.Value = transform;
            BehaviorTree prefab = GameObject.Instantiate(projectilePrefab.Value, origin.Value.position, Quaternion.identity);
            prefab.SetVariableValue("vectorUp", transform.up.normalized);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

    }
}