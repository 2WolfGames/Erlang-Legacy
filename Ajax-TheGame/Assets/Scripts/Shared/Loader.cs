using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Shared
{
    public static class Loader 
    {
        public enum Scene{
            StartMenu, LoadingScene, lvl1, FirstIsland
        }

        public static void Load(Scene scene){
            LoadingMenu.sceneName = scene.ToString();
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

        public static void Load(string sceneName){
            LoadingMenu.sceneName = sceneName;
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

    }
}

