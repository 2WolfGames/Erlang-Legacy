using Core.Shared;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using TMPro;

namespace Core.UI.Notifications
{
    public class BigNotificationManager : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI description;
        [SerializeField] TextMeshProUGUI info;
        CanvasGroup notificationCanvasGroup => GetComponent<CanvasGroup>();
        bool showingNotification;

        public static BigNotificationManager Instance { get; private set; }

        void Awake()
        {
            var matches = FindObjectsOfType<BigNotificationManager>();

            if (matches.Length > 1)
                Destroy(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            info.color = Function.ColorVisible(false, info.color);
        }

        public void ShowNotification(Sprite image, string title, string description)
        {
            Time.timeScale = 0;
            this.image.sprite = image;
            this.title.text = title;
            this.description.text = description;
            notificationCanvasGroup.DOFade(1, 0.3f).SetUpdate(true).OnComplete(
                () =>
                {
                    info.DOFade(1, 0.2f).SetUpdate(true).SetDelay(2f).OnComplete(() =>
                    {
                        showingNotification = true;
                    });
                }
            );
        }

        private void Update()
        {
            if (showingNotification)
            {
                if (Input.anyKey)
                {
                    HideNotification();
                }
            }
        }

        private void HideNotification()
        {
            showingNotification = false;
            notificationCanvasGroup.DOFade(0, 0.3f).SetUpdate(true).OnComplete(() =>
            {
                Time.timeScale = 1;
                info.color = Function.ColorVisible(false, info.color);
            });
        }
    }
}