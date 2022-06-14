using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Core.IA.Bahavior.SharedVariable
{
    [System.Serializable]
    public class SharedBehaviorTree : SharedVariable<BehaviorTree>
    {
        public static implicit operator SharedBehaviorTree(BehaviorTree value) { return new SharedBehaviorTree { Value = value }; }
    }
}

