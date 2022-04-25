
using System;
using Core.Combat;
using UnityEngine;


namespace Core.Player.Data
{

    [Serializable]
    public class DamageAreaData
    {
        [SerializeField] DamageArea dash;
        [SerializeField] DamageArea punch; // basic attack damage area

        public DamageArea Dash { get => dash; set => dash = value; }
        public DamageArea Punch { get => punch; set => punch = value; }
    }

}