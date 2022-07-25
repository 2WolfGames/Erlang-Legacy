using Gamekit2D;
using UnityEngine;

public class BackgroundMusicPlayerUpdater : MonoBehaviour
{
    [SerializeField] AudioClip musicAudioClip;
    [SerializeField] AudioClip ambientAudioClip;

    private void Start()
    {
        UpdateMusicClip();
        UpdateAmbientClip();
    }

    private void UpdateMusicClip()
    {
        if (musicAudioClip != null)
        {
            if (BackgroundMusicPlayer.Instance == null)
            {
                Debug.LogError("BackgroundMusicPlayer is not found");
                return;
            }
            BackgroundMusicPlayer.Instance.ChangeAmbient(musicAudioClip);
        }
    }

    private void UpdateAmbientClip()
    {
        if (ambientAudioClip != null)
        {
            if (BackgroundMusicPlayer.Instance == null)
            {
                Debug.LogError("BackgroundMusicPlayer is not found");
                return;
            }
            BackgroundMusicPlayer.Instance.ChangeAmbient(ambientAudioClip);
        }
    }
}
