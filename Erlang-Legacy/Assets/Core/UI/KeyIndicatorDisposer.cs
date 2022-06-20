using System.Collections;
using Core.Shared.Enum;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Core.UI
{
    public class KeyIndicatorDisposer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI function;
        //[SerializeField] TextMeshProUGUI key;
        CanvasGroup canvasGroup => GetComponentInChildren<CanvasGroup>();
        [SerializeField] GameObject largeKey;
        [SerializeField] GameObject defaultKey;
        bool showing = false;
        TextMeshProUGUI currentKey;

        public static KeyIndicatorDisposer Instance { get; private set; }

        //pre: --
        //post: if these is no KeyIndicatorDisposer this becomes the one
        //      else it destroys itself
        private void Awake()
        {
            if (KeyIndicatorDisposer.Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        //pre: -- 
        //post: tutorial starts invisible, it's shown only when needed
        void Start()
        {
            canvasGroup.alpha = 0;
        }

        //pre: -- 
        //post: we show the key gameKey and the instructions funcionallity in screen
        public void ShowTutorial(GameKey gameKey, string functionallity)
        {
            if (gameKey == GameKey.Tab || gameKey == GameKey.Space)
            {
                largeKey.GetComponent<CanvasGroup>().alpha = 1f;
                defaultKey.GetComponent<CanvasGroup>().alpha = 0f;
                currentKey = largeKey.GetComponentInChildren<TextMeshProUGUI>();
            }
            else
            {
                largeKey.GetComponent<CanvasGroup>().alpha = 0f;
                defaultKey.GetComponent<CanvasGroup>().alpha = 1f;
                currentKey = defaultKey.GetComponentInChildren<TextMeshProUGUI>();
            }
            function.text = functionallity;
            currentKey.text = gameKey.ToString().Substring(0, 1).ToUpper() + gameKey.ToString().Substring(1).ToLower(); //ToUpper();
            showing = true;
            canvasGroup.DOFade(1, 0.25f);
        }

        //pre: -- 
        //post: we hide tutorial
        public void HideTutorial()
        {
            if (!showing)
                return;
            showing = false;
            canvasGroup.DOFade(0, 0.25f);
        }
    }
}