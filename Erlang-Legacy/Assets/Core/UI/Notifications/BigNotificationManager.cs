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

        //pre: --
        //post: if these is no BigNotificationManager this becomes the one
        //      else it destroys itself
        void Awake()
        {
            var matches = FindObjectsOfType<BigNotificationManager>();

            if (matches.Length > 1)
                Destroy(gameObject);
            else Instance = this;
        }

        //pre: --
        //post: inits value
        private void Start()
        {
            info.color = Function.ColorVisible(false, info.color);
        }

        //pre: --
        //post: shows a big notifications that stops the gameplay 
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

        //pre:
        //post: if notification its displayed waits for a key input to hide it
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

        //pre: notification is displayed
        //post: hides notification 
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