using System;
using UnityEngine;
using System.Collections.Generic;
using Core.Player;

namespace Core.Shared.SaveSystem
{
    [Serializable]
    public class PlayerState
    {
        public int scene;
        public float[] position;
        public int health;
        public int max_health;
        public Dictionary<Ability, bool> abilitiesAdquired;

        public PlayerState(int scene, int health, int max_health, Vector3 position, Dictionary<Ability, bool> abilitiesAdquired)
        {
            this.scene = scene;
            this.health = health;
            this.max_health = max_health;

            this.position = new float[3];
            this.position[0] = position.x;
            this.position[1] = position.y;
            this.position[2] = position.z;

            this.abilitiesAdquired = abilitiesAdquired;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(position[0], position[1], position[2]);
        }

    }
}
