using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Core.IA.Bahavior.SharedVariable
{
    [System.Serializable]
    public class SharedSpriteRenderer : SharedVariable<SpriteRenderer>
    {
        public static implicit operator SharedSpriteRenderer(SpriteRenderer value) { return new SharedSpriteRenderer { Value = value }; }
    }
}

