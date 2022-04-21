using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{

    public static void SavePlayer(PlayerData playerData){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player_stats.bin";
        FileStream fileStream = new FileStream(path,FileMode.Create);

        formatter.Serialize(fileStream,playerData);
        fileStream.Close();
    }

    public static PlayerData LoadPlayerStatus(){
        PlayerData playerData;
        string path = Application.persistentDataPath + "/player_stats.bin";
        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path,FileMode.Open);

            playerData = formatter.Deserialize(fileStream) as PlayerData;
            fileStream.Close();
            return playerData;
        } else {
            //default vaules ? TODO
            playerData = new PlayerData(SceneManager.GetSceneByName(Core.Shared.Loader.Scene.FirstIsland.ToString()).buildIndex, 
                                        3, new Vector3(-23.75f,-1.57f,0));
            return playerData;
        }
    }

}
