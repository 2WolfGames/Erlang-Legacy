
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Environment;
using Core.IA.Bahavior.SharedVariable;

namespace Core.AI.Task.FalseKnight
{  
    public class OpenDoors  : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedGenericList<Door> doors;
        public SharedFloat speed = 1f;

        public override TaskStatus OnUpdate()
        {
            foreach (Door door in doors.Value)
            {
                door.Speed = speed.Value;
                door.Open();
            }
            return TaskStatus.Success;
        }
    }
}

