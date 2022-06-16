using UnityEngine;

namespace Core.ScriptableEffect
{
    public abstract class Effect : ScriptableObject
    {
        public virtual void Apply(GameObject other) { }

        public virtual void Apply(GameObject self, GameObject other) { }
    }
}
