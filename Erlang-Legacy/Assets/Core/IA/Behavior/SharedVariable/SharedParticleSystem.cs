using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Core.IA.Bahavior.SharedVariable
{
    [System.Serializable]
    public class SharedParticleSystem : SharedVariable<ParticleSystem>
    {
        public static implicit operator SharedParticleSystem(ParticleSystem value) { return new SharedParticleSystem { Value = value }; }
    }
}

