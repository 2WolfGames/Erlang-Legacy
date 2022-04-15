﻿using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [Header("Life stats")]
    public int HP; // current life
    public int maxHP; // max life


    [Header("Movements stats")]
    public float basicSpeed; // basic player speed movement
    public float dashSpeed; // player speed at dashing
    public float jumpPower;
    [Range(0.1f, 1f)] public float airDrag;


    [Header("Events duration stats")]
    public float dashDuration; // how it takes to dash animation  finish
    public float holdingAfterJump; // how much time can you press jump key to continue jumping


    [Header("Damage stats")]
    public float dashDamage;
    public float rayDamage; // how much damage ray does
    public float punchDamage;


    [Header("Cooldown stats")]
    public float recoverCooldown;
    public float dashCooldown; // how much it takes to trigger dash ability again
    public float rayCooldown; // how much it takes to through a ray again

    [Header("Ray Projectile stats")]
    public Transform rayOrigin; // from which point rays are triggered
    public float raySpeed; // default ray ability speed
    public float rayLifetime; // default ray abilty duration before it get's destroyed 

}
