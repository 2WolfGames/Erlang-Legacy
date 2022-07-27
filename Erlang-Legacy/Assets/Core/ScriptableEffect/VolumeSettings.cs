using UnityEngine;

namespace Core.ScriptableEffect
{
    [CreateAssetMenu(menuName = "Settings/Volume")]
    public class VolumeSettings : ScriptableObject
    {
        [Range(0f, 1f)] [SerializeField] float musicVolume = 1f;
        [Range(0f, 1f)] [SerializeField] float soundValume = 1f;

        public float MusicVolume { get => musicVolume; set => musicVolume = value; }
        public float SoundVolume { get => soundValume; set => soundValume = value; }
    }
}
