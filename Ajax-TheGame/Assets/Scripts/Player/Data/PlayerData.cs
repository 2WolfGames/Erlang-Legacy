using System;
using UnityEngine;

namespace Core.Player.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] ParticleSystem jumpParticles;
        [SerializeField] TrailRenderer dashTrailRender;
        [SerializeField] Health health;
        [SerializeField] Stats stats;

        public Health Health { get => health; set => health = value; }
        public Stats Stats { get => stats; set => stats = value; }
        public ParticleSystem JumpParticles { get => jumpParticles; set => jumpParticles = value; }
        public TrailRenderer DashTrailRender { get => dashTrailRender; set => dashTrailRender = value; }
    }
}


