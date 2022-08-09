


using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Environment;
using Core.IA.Bahavior.SharedVariable;
using UnityEngine;

namespace Core.AI.Task.FalseKnight
{
    public class CloseDoors : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedGameObjectList doors;
        public SharedFloat speed = 1f;

        public override TaskStatus OnUpdate()
        {
            foreach (GameObject obj in doors.Value)
            {
                Door door = obj.GetComponent<Door>();
                door.Speed = speed.Value;
                door.Close();
            }
            return TaskStatus.Success;
        }
    }
}
