using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [Header("Player Health")]
    public int HP; // current life
    public int maxHP; // max life


    [Header("Movements")]
    public float movementSpeed; // basic player speed movement
    public float dashSpeed; // player speed at dashing
    public float jumpPower;
    [Range(0.1f, 1f)] public float airDrag;


    [Header("Damage")]
    public float dashDamage;
    public float rayDamage; // how much damage ray does
    public float punchDamage;


    [Header("Cooldown or events duration")]
    public float recoverCooldown;
    public float dashCooldown; // how much it takes to trigger dash ability again
    public float rayCooldown; // how much it takes to through a ray again
    public float holdingAfterJump; // how much time can you press jump key to continue jumping


    [Header("VFX")]
    public ParticleSystem jumpParticles;
    public TrailRenderer dashTrailRender;


    [Header("Ray Projectile")]
    public Transform rayOrigin; // from which point rays are triggered
    public float raySpeed; // default ray ability speed
    public float rayLifetime; // default ray abilty duration before it get's destroyed 
}
