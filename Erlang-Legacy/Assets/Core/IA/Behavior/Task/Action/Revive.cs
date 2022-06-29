using BehaviorDesigner.Runtime.Tasks;
using Core.Combat.Projectile;
using UnityEngine;


namespace Core.Combat.IA.Action
{
    public class Revive : EnemyAction
    {
        public override TaskStatus OnUpdate()
        {
            destroyable.Revive();
            return TaskStatus.Success;
        }
    }
}
