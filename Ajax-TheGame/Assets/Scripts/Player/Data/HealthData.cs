using System;
using UnityEngine;

namespace Core.Player.Data
{
    [Serializable]
    public class HealthData
    {
        [SerializeField] int hp;
        [SerializeField] int maxHP;

        public int HP { get => hp; set => hp = value; }
        public int MaxHP { get => maxHP; set => maxHP = value; }
    }
}

