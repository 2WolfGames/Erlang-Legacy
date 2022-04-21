using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int scene;
    public float[] position;
    public int health;

    public PlayerData(int scene, int health, Vector3 position){
        this.scene = scene;
        this.health = health;

        this.position = new float[3];
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;
    }

}
