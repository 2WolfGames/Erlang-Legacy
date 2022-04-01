using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;

public class LoadingMenu : MonoBehaviour
{
    [SerializeField] SkeletonGraphic skeletonGraphic;
    [SerializeField] CanvasGroup canvasGroup;
    AsyncOperation loadingOperation;

    void Start()
    {
        canvasGroup.alpha = 0;
        skeletonGraphic.AnimationState.SetAnimation(1,"animation",true);
        StartCoroutine(FadeLoadingScreen(2));
        StartCoroutine(nextScene("lvl1"));
        
    }

    private void Update() {
        if (loadingOperation != null){
            if (loadingOperation.progress > 0.9f){
                
            }
            Debug.Log(loadingOperation.progress);
        }
    } 

    IEnumerator FadeLoadingScreen(float duration)
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

    IEnumerator nextScene(string sceneName){
        yield return new WaitForSeconds(20);
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
    }

}
