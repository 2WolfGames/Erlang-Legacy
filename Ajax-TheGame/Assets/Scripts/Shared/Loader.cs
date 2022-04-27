using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Shared
{
    public static class Loader
    {
        public enum Scene
        {
            StartMenu, LoadingScene, lvl1
        }

        public static void Load(Scene scene)
        {
            LoadingMenu.sceneName = scene.ToString();
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

    }
}

