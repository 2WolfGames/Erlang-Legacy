using Core.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Manager
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;

                s_Instance = FindObjectOfType<SoundManager>();

                return s_Instance;
            }
        }

        // Random pitch adjustment range.
        public float lowPitchRange = 0.95f;
        public float highPitchRange = 1.05f;

        public float Volume { get; set; } = 1.0f;
        private float musicBaseVolume = 0.3f;
        private AudioSource audioSource;
        private static SoundManager s_Instance;

        private void Awake()
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip, float volume = 1.0f, AudioSource src = null, bool randomizePitch = false)
        {
            float pitch = randomizePitch ? Random.Range(lowPitchRange, highPitchRange) : 1.0f;
            PlaySoundWithPitch(clip, pitch, volume, src);
        }

        public void PlayRandomSound(AudioClip[] clips, float volume = 1.0f, AudioSource src = null)
        {
            if (clips.Length == 0)
                return;

            AudioSource source = src ?? audioSource;

            int randomIndex = Random.Range(0, clips.Length);

            source.pitch = Random.Range(lowPitchRange, highPitchRange);
            source.PlayOneShot(clips[randomIndex], volume);
        }

        public void PlaySoundAtLocation(AudioClip clip, Vector3 position, float volume = 1.0f)
        {
            if (clip == null) return;

            var tempAudioSource = new GameObject("TempAudio");
            tempAudioSource.transform.position = position;
            var src = tempAudioSource.AddComponent<AudioSource>();
            src.clip = clip;
            src.volume = volume;
            src.spatialBlend = 1.0f;
            src.rolloffMode = AudioRolloffMode.Linear;
            src.minDistance = 5.0f;
            src.maxDistance = 15.0f;
            src.dopplerLevel = 0;
            tempAudioSource.Disposable(clip.length);
            src.Play();
        }

        public void PlaySoundWithPitch(AudioClip clip, float pitch, float volume = 1.0f, AudioSource src = null)
        {
            if (clip == null) return;
            /* El operador de uso combinado de NULL ?? 
            devuelve el valor del operando izquierdo si no es null; 
            en caso contrario, evalúa el operando derecho y devuelve su resultado. 
            El operador ?? no evalúa su operando derecho 
            si el operando izquierdo se evalúa como no NULL.*/
            AudioSource source = src ?? audioSource;
            source.pitch = pitch;
            source.PlayOneShot(clip, volume);
        }
    }
}
