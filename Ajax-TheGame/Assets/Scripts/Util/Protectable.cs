using UnityEngine;

namespace Core.Util
{
    // description:
    //  manage state if action can be done over game object
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
    }
}

