using UnityEngine;

namespace Core.UI
{
    public class PowersPanelManager : MonoBehaviour
    {
        [SerializeField] UIPowerTimer dashTimer;
        [SerializeField] UIPowerTimer rayTimer;
        private ManagePowersVisibility managePowersVisibility => GetComponent<ManagePowersVisibility>();

        public static PowersPanelManager Instance { get; private set; }

        //pre: --
        //post: if these is no gamesessioncontroller this becomes the one
        //      else it destroys itself
        private void Awake()
        {
            if (PowersPanelManager.Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            ManagePowersVisibility();
        }

        public void ManagePowersVisibility()
        {
            managePowersVisibility.ManageAdquiredPowersVisibility();
        }

        public UIPowerTimer GetDashTimer()
        {
            return dashTimer;
        }

        public UIPowerTimer GetRayTimer()
        {
            return rayTimer;
        }

    }
}