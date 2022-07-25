

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player.Data
{
    [Serializable]
    public class SFX
    {
        public AudioClip abilityAdquired;
        public AudioClip[] punchSounds;
        public AudioClip[] dashSounds;
        public AudioClip healthSound;
        public AudioClip[] deathSounds;
        public AudioClip[] hurtSounds;
    }
}