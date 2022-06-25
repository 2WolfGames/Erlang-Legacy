using System.Collections;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class LoadingMenu : MonoBehaviour
    {
        [SerializeField] SkeletonGraphic skeletonGraphic;
        [SerializeField] CanvasGroup canvasGroup;
        public static string sceneName;
        AsyncOperation asyncOperation;

        private void Awake()
        {
            canvasGroup.alpha = 0;
        }

        void Start()
        {
            skeletonGraphic.AnimationState.SetAnimation(1, "animation", true);
            StartCoroutine(LightLoadingScreen(2));
            StartCoroutine(LoadNextScene());
        }
        private IEnumerator LoadNextScene()
        {
            yield return new WaitForSeconds(3);
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        private void Update()
        {
            if (asyncOperation != null)
            {
                if (asyncOperation.progress > 0.9f)
                {
                    StartCoroutine(FadeLoadingScreen(2));
                }
            }
        }

        IEnumerator LightLoadingScreen(float duration)
        {
            float startValue = canvasGroup.alpha;
            float time = 0;
            while (time < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(startValue, 1, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1;
        }

        IEnumerator FadeLoadingScreen(float duration)
        {
            float startValue = canvasGroup.alpha;
            float time = 0;
            while (time < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(startValue, 0, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 0;
        }

    }
}
