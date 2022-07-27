using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Bahavior.SharedVariable;
using DG.Tweening;
using Gamekit2D;
using UnityEngine;

namespace Core.IA.Behavior.Task.Action
{
    public class BackgroundMusicPlayerUpdater : BehaviorDesigner.Runtime.Tasks.Action
    {
        [SerializeField] SharedAudioClip musicAudioClip;
        [SerializeField] float musicAudioTimeout = 0f;
        [SerializeField] AudioClip ambientAudioClip;
        [SerializeField] float ambientAudioTimeout = 0f;
        [SerializeField] bool saveBaseMusicPlayer = false;

        public SharedAudioClip baseMusicAudioClip;
        public SharedAudioClip baseAmbientAudioClip;

        public override void OnStart()
        {
            SavePreviousBackgroundMusicPlayer();
            DOVirtual.DelayedCall(musicAudioTimeout, UpdateMusicClip);
            DOVirtual.DelayedCall(ambientAudioTimeout, UpdateAmbientClip);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
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
            BackgroundMusicPlayer.Instance.ChangeMusic(musicAudioClip.Value);
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

        private void SavePreviousBackgroundMusicPlayer()
        {
            if (saveBaseMusicPlayer)
            {
                AudioClip currentMusicAudio = BackgroundMusicPlayer.Instance.musicAudioClip;
                AudioClip currentAmbientAudio = BackgroundMusicPlayer.Instance.ambientAudioClip;
                baseMusicAudioClip.Value = currentMusicAudio;
                baseAmbientAudioClip.Value = currentAmbientAudio;
            }
        }
    }
}
