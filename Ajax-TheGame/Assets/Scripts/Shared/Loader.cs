using System.Collections;
using Core.Shared.Enum;
using Core.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Shared
{
    public static class Loader
    {

        public static IEnumerator LoadWithDelay(SceneID scene, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Load(scene.ToString());
        }

        private static void Load(string sceneName)
        {
            BeforeLoadingScene();
            LoadingMenu.sceneName = sceneName;
            SceneManager.LoadScene(SceneID.LoadingScene.ToString());
        }

        private static void BeforeLoadingScene()
        {
            DOTween.KillAll(false);
        }


    }
}

