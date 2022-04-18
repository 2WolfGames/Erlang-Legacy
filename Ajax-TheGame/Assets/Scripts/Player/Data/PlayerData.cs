using UnityEngine;
using System;


namespace Core.Player.Data
{
    [Serializable]
    public class PlayerData
    {
        
        [Header("VFX")] 
        [SerializeField] ParticleSystem jumpParticles;
        [SerializeField] TrailRenderer dashTrailRender;
        [SerializeField] HealthData health;
        [SerializeField] StatsData stats;
        [SerializeField] ProjectileData projectile;
        [SerializeField] DamageAreaData damageArea;
        


        public ProjectileData Projectile { get => projectile; set => projectile = value; }
        public HealthData Health { get => health; set => health = value; }
        public StatsData Stats { get => stats; set => stats = value; }
        public ParticleSystem JumpParticles { get => jumpParticles; set => jumpParticles = value; }
        public TrailRenderer DashTrailRender { get => dashTrailRender; set => dashTrailRender = value; }
        public DamageAreaData DamageArea { get => damageArea; set => damageArea = value; }
    }
}
