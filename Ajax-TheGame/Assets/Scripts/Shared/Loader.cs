using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Shared
{
    public static class Loader 
    {
        public enum Scene{
            StartMenu, LoadingScene, lvl1, FirstIsland
        }

        
        public enum Entrance{
            E1, E2, E3, E4, E5
        }

        public static void Load(Scene scene){
            DOTween.KillAll(false);
            LoadingMenu.sceneName = scene.ToString();
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static void Load(string sceneName){
            LoadingMenu.sceneName = sceneName;
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static IEnumerator LoadWithDelay(string sceneName, float seconds){
            yield return new WaitForSeconds(seconds);
            Load(sceneName);
        }

        public static IEnumerator LoadWithDelay(Scene scene, float seconds){
            yield return new WaitForSeconds(seconds);
            Load(scene);
        }

    }
}

