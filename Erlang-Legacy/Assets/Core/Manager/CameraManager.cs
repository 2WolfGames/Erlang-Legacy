using Cinemachine;
using Core.Player.Controller;
using DG.Tweening;
using UnityEngine;

namespace Core.Manager
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] bool followPlayerOnAwake = true;
        [SerializeField] float intensity;
        [SerializeField] float frequency;
        [SerializeField] float duration;

        public static CameraManager Instance;
        public bool Shaking { get; private set; }
        private PlayerController player => PlayerController.Instance;
        private CinemachineVirtualCamera virtualCamera => GetComponent<CinemachineVirtualCamera>();
        private CinemachineBasicMultiChannelPerlin virtualCameraNoise => virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            if (followPlayerOnAwake)
                FollowPlayer();
        }

        private void FollowPlayer()
        {
            virtualCamera.Follow = player.transform;
        }

        public void ShakeCamera()
        {
            ShakeCamera(intensity, frequency, duration);
        }

        public void ShakeCamera(float intensity = 1f, float frequency = 1f, float duration = 1.0f)
        {
            if (Shaking)
                return;

            Shaking = true;
            virtualCameraNoise.m_AmplitudeGain = intensity;
            virtualCameraNoise.m_FrequencyGain = frequency;

            DOVirtual.DelayedCall(duration, () =>
            {
                virtualCameraNoise.m_AmplitudeGain = 0;
                virtualCameraNoise.m_FrequencyGain = 0;
                Shaking = false;
            });
        }
    }
}
