using System;

[Serializable]
public class PlayerData
{
    public int HP; // current life
    public int maxHP; // max life
    public float recoverDuration; // default recover time at collisions
    public float raySpeed; // default ray ability speed
    public float rayDuration; // default ray abilty duration before it get's destroyed 
    public float dashSpeed; // player speed at dashing
    public float basicSpeed; // basic player speed movement
    public float dashDuration; // how it takes to dash animation  finish
}
