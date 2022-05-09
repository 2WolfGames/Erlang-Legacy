using UnityEngine;

namespace Core.Effect
{
    public abstract class Effect : ScriptableObject
    {
        public abstract void Apply(GameObject target);
    }
}
