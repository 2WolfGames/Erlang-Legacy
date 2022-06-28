using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Core.UI.Notifications
{
    public class NotificationBehaviour : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI description;

        //pre: --
        //post: It displays a notification for few seconds and then destroy it. 
        public IEnumerator DisplayNotification(string title, string description, Sprite sprite, float seconds)
        {
            CanvasGroup cg = GetComponent<CanvasGroup>();
            this.image.sprite = sprite;
            this.title.text = title;
            this.description.text = description;
            cg.DOFade(1, 0.2f);
            yield return new WaitForSeconds(seconds);
            cg.DOFade(0, 0.5f).OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }
    }
}