using UnityEngine;

namespace Core.Util
{
    // description:
    //  manage state if action can be done over game object
    public class Protectable : MonoBehaviour
    {
        [SerializeField] float protectionDuration;
        private float hitProtectionTimer;
        public bool CanBeHit => hitProtectionTimer <= 0;
        public bool IsProtected => !CanBeHit;
        public float ProtectionDuration
        {
            get => protectionDuration;
            set { protectionDuration = value; }
        }

        public void Update()
        {
            if (hitProtectionTimer > 0)
                hitProtectionTimer -= Time.deltaTime;
        }

        public void ResetProtection()
        {
            this.hitProtectionTimer = ProtectionDuration;
        }

        public void ResetProtection(float duration)
        {
            this.hitProtectionTimer = duration;
        }
    }
}

