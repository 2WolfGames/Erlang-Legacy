
using System;
using Core.Combat;
using UnityEngine;


namespace Core.Character.Data
{

    [Serializable]
    public class DamageAreaData
    {
        [SerializeField] HitArea dash;
        [SerializeField] HitArea punch; // basic attack damage area

        public HitArea Dash { get => dash; set => dash = value; }
        public HitArea Punch { get => punch; set => punch = value; }
    }

}