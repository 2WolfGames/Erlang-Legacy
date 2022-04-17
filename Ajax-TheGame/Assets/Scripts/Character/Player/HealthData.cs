using UnityEngine;
using System;

namespace Core.Character.Player
{
    [Serializable]
    public class HealthData
    {
        [SerializeField] float hp;
        [SerializeField] float maxHP;

        public float HP { get => hp; set => hp = value; }
        public float MaxHP { get => maxHP; set => maxHP = value; }
    }
}

