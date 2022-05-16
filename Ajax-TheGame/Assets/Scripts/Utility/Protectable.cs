using UnityEngine;

namespace Core.Utility
{
    public enum ProtectionType
    {
        INFINITE,
        NONE
    }

    public class Protectable : MonoBehaviour
    {
        [SerializeField] float protectionDuration;
        public bool CanBeHit => hitProtectionTimer <= 0;
        public bool IsProtected => !CanBeHit;
        private float hitProtectionTimer;
        public float ProtectionDuration { get => protectionDuration; private set => protectionDuration = value; }

        public void Update()
        {
            if (hitProtectionTimer > 0)
                hitProtectionTimer -= Time.deltaTime;
        }

        public void ResetProtection()
        {
            hitProtectionTimer = ProtectionDuration;
        }

        public void SetProtection(float duration)
        {
            hitProtectionTimer = duration;
        }

        public void SetProtection(ProtectionType protectionType)
        {
            if (protectionType == ProtectionType.INFINITE)
                hitProtectionTimer = float.PositiveInfinity;
            else
                hitProtectionTimer = 0f;
        }
    }
}

