using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public abstract class Effect : ScriptableObject
    {
        public abstract void Apply(GameObject target);
    }
}
