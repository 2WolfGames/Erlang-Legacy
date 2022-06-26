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

        //pre: seconds >= 0
        //post: Loads scene after seconds
        public static IEnumerator LoadWithDelay(SceneID scene, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Load(scene.ToString());
        }

        //pre: --
        //post: changes scene to LoadingScene and sets the new scene to charge 
        private static void Load(string sceneName)
        {
            KillActiveTweens();
            LoadingMenu.sceneName = sceneName;
            SceneManager.LoadScene(SceneID.LoadingScene.ToString());
        }

        //pre: --
        private static void KillActiveTweens()
        {
            DOTween.KillAll(false);
        }
    }
}

