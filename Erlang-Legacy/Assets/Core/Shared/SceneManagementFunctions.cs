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
            else if (currentScene == SceneID.FirstIsland.ToString())
            {
                return SceneID.FirstIsland;
            }
            else if (currentScene == SceneID.LoadingScene.ToString())
            {
                return SceneID.LoadingScene;
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
            else
            { //lvl1
                return SceneID.lvl1;
            }
        }

        //pre: --
        //post: returns true if current scene is playable, 
        //      false if it's some kind of menu or loading scene
        public static bool CurrentSceneIsGameplay()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == SceneID.StartMenu.ToString() ||
            currentScene == SceneID.LoadingScene.ToString())
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
