using System.Collections.Generic;
using BehaviorDesigner.Runtime;

namespace Core.IA.Bahavior.SharedVariable
{
    [System.Serializable]
    public class SharedGenericList<T> : SharedVariable<List<T>>
    {
        public static implicit operator SharedGenericList<T>(List<T> value) { return new SharedGenericList<T> { Value = value }; }
    }
}

