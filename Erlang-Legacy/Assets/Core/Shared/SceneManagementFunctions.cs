using Core.Shared.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Shared
{
    public class SceneManagementFunctions : MonoBehaviour
    {
        //TODO: find better solution?
        //pre: --
        //post: Returns current scene as SceneID enum
        public static SceneID GetCurrentSceneEnum()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == SceneID.StartMenu.ToString())
            {
                return SceneID.StartMenu;
            }
            else if (currentScene == SceneID.LoadingScene.ToString())
            {
                return SceneID.LoadingScene;
            }
            else if (currentScene == SceneID.EndGameScene.ToString()){
                return SceneID.EndGameScene;
            }
            else if (currentScene == SceneID.OmedIsland_Zone1.ToString())
            {
                return SceneID.OmedIsland_Zone1;
            }
            else if (currentScene == SceneID.OmedIsland_Zone2.ToString())
            {
                return SceneID.OmedIsland_Zone2;
            }
            else if (currentScene == SceneID.OmedIsland_Zone3.ToString())
            {
                return SceneID.OmedIsland_Zone3;
            }
            else if (currentScene == SceneID.OmedIsland_Zone4.ToString())
            {
                return SceneID.OmedIsland_Zone4;
            }
            else if (currentScene == SceneID.OmedIsland_Zone5.ToString())
            {
                return SceneID.OmedIsland_Zone5;
            }
            else if (currentScene == SceneID.OmedIsland_Zone6.ToString())
            {
                return SceneID.OmedIsland_Zone6;
            }
            else if (currentScene == SceneID.MenIsland_Zone1.ToString())
            {
                return SceneID.MenIsland_Zone1;
            }
            else if (currentScene == SceneID.MenIsland_Zone2.ToString())
            {
                return SceneID.MenIsland_Zone2;
            }
            else if (currentScene == SceneID.MenIsland_Zone3.ToString())
            {
                return SceneID.MenIsland_Zone3;
            }
            else if (currentScene == SceneID.MenIsland_Zone4.ToString())
            {
                return SceneID.MenIsland_Zone4;
            }
            else if (currentScene == SceneID.MenIsland_Zone5.ToString())
            {
                return SceneID.MenIsland_Zone5;
            }
            else if (currentScene == SceneID.MenIsland_Zone6.ToString())
            {
                return SceneID.MenIsland_Zone6;
            }
            else if (currentScene == SceneID.MenIsland_Zone7.ToString())
            {
                return SceneID.MenIsland_Zone7;
            }
            else if (currentScene == SceneID.MenIsland_Zone8.ToString())
            {
                return SceneID.MenIsland_Zone8;
            }
            else if (currentScene == SceneID.MenIsland_Zone9.ToString())
            {
                return SceneID.MenIsland_Zone9;
            }
            else if (currentScene == SceneID.MenIsland_Zone10.ToString())
            {
                return SceneID.MenIsland_Zone10;
            }
            else
            { //lvl1
                throw new System.Exception("Scene ID not found.");
            }
        }

        //pre: --
        //post: returns true if current scene is playable, 
        //      false if it's some kind of menu or loading scene
        public static bool CurrentSceneIsGameplay()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == SceneID.StartMenu.ToString() ||
            currentScene == SceneID.LoadingScene.ToString() ||
            currentScene == SceneID.EndGameScene.ToString())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
