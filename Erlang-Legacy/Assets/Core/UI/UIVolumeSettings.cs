using Core.ScriptableEffect;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UIVolumeSettings : MonoBehaviour
    {
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider soundSlider;
        [SerializeField] VolumeSettings volumeSettings;

        private void Update()
        {
            UpdateVolumeSettings();
        }

        private void UpdateVolumeSettings()
        {
            volumeSettings.MusicVolume = musicSlider.value;
            volumeSettings.SoundVolume = soundSlider.value;
        }
    }
}