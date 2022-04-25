using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Shared.Enum;

public class SceneManagementFunctions : MonoBehaviour
{
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
        else
        { //lvl1
            return SceneID.lvl1;
        }
    }

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
