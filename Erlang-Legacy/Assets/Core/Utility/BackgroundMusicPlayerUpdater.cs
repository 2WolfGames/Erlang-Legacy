using Gamekit2D;
using UnityEngine;

namespace Core.Utility
{
    public class BackgroundMusicPlayerUpdater : MonoBehaviour
    {
        [SerializeField] AudioClip musicAudioClip;
        [SerializeField] AudioClip ambientAudioClip;

        private void Start()
        {
            Play();
        }

        public void Play()
        {
            UpdateMusicClip();
            UpdateAmbientClip();
        }

        private void UpdateMusicClip()
        {
            if (musicAudioClip == null)
            {
                Debug.LogWarning("Music audio clip is not set");
            }
            if (BackgroundMusicPlayer.Instance == null)
            {
                Debug.LogError("BackgroundMusicPlayer is not found");
                return;
            }
            BackgroundMusicPlayer.Instance.ChangeMusic(musicAudioClip);
            BackgroundMusicPlayer.Instance.PlayJustMusic();
        }

        private void UpdateAmbientClip()
        {
            if (ambientAudioClip == null)
            {
                Debug.LogWarning("Ambient audio clip is not set");
            }
            if (BackgroundMusicPlayer.Instance == null)
            {
                Debug.LogError("BackgroundMusicPlayer is not found");
                return;
            }
            BackgroundMusicPlayer.Instance.ChangeAmbient(ambientAudioClip);
            BackgroundMusicPlayer.Instance.PlayJustAmbient();
        }
    }
}
