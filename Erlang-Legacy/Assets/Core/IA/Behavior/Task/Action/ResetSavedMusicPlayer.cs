using BehaviorDesigner.Runtime.Tasks;
using Core.IA.Bahavior.SharedVariable;
using DG.Tweening;
using Gamekit2D;
using UnityEngine;

namespace Core.IA.Behavior.Task.Action
{
    public class ResetSavedMusicPlayer : BehaviorDesigner.Runtime.Tasks.Action
    {
        public SharedAudioClip baseMusicAudioClip;
        public SharedAudioClip baseAmbientAudioClip;
        public float musicAudioTimeout = 0f;
        public float ambientAudioTimeout = 0f;

        private AudioClip musicAudioClip;
        private AudioClip ambientAudioClip;

        public override void OnStart()
        {
            ResetMusicPlayer();
            DOVirtual.DelayedCall(musicAudioTimeout, UpdateMusicClip);
            DOVirtual.DelayedCall(ambientAudioTimeout, UpdateAmbientClip);
        }

        private void ResetMusicPlayer()
        {
            if (BackgroundMusicPlayer.Instance == null)
            {
                Debug.LogError("BackgroundMusicPlayer is not found");
                return;
            }
            musicAudioClip = baseMusicAudioClip.Value;
            ambientAudioClip = baseAmbientAudioClip.Value;
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
