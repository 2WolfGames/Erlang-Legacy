using System;
using UnityEngine;

namespace Core.Player.Data
{
    [Serializable]
    public class PlayerData
    {

        [Header("VFX")]
        [SerializeField] ParticleSystem jumpParticles;
        [SerializeField] TrailRenderer dashTrailRender;
        [SerializeField] PlayerHealth health;
        [SerializeField] StatsData stats;

        public PlayerHealth Health { get => health; set => health = value; }
        public StatsData Stats { get => stats; set => stats = value; }
        public ParticleSystem JumpParticles { get => jumpParticles; set => jumpParticles = value; }
        public TrailRenderer DashTrailRender { get => dashTrailRender; set => dashTrailRender = value; }
    }
}


