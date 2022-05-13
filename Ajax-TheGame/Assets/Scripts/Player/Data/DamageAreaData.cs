
using System;
using Core.Combat;
using Core.Utility;
using UnityEngine;

namespace Core.Player.Data
{

    [Serializable]
    public class DamageAreaData
    {
        [SerializeField] InteractOnTrigger2D dash;
        [SerializeField] InteractOnTrigger2D punch; // basic attack damage area

        public InteractOnTrigger2D Dash { get => dash; set => dash = value; }
        public InteractOnTrigger2D Punch { get => punch; set => punch = value; }
    }

}
