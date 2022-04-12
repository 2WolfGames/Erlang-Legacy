using UnityEngine;

namespace Core.Util
{
    // description:
    //  manage state if action can be done over game object
    public class Protectable : MonoBehaviour
    {
        [SerializeField] float hitProtectionDuration;

        float hitProtectionTimer;

        public bool CanBeHit => hitProtectionTimer <= 0;

        public bool IsProtected => !CanBeHit;

        public void Update()
        {
            if (hitProtectionTimer > 0)
                hitProtectionTimer -= Time.deltaTime;
        }

        public void ResetProtection()
        {
            this.hitProtectionTimer = hitProtectionDuration;
        }

        public void ResetProtection(float duration)
        {
            this.hitProtectionTimer = duration;
        }
    }
}

