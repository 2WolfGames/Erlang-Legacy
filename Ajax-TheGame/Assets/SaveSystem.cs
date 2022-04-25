using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static string dataPath = Application.persistentDataPath;
    public const string playerStateFileName = "/player_stats.bin";

    public static bool SaveGameExists(){
        Debug.Log(dataPath);
        return File.Exists(dataPath + playerStateFileName);
    }

    public static void SavePlayerState(PlayerState playerData){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = dataPath + playerStateFileName;
        FileStream fileStream = new FileStream(path,FileMode.Create);

        formatter.Serialize(fileStream,playerData);
        fileStream.Close();
    }

    public static PlayerState LoadPlayerState(){
        PlayerState playerData;
        string path = Application.persistentDataPath + "/player_stats.bin";
        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path,FileMode.Open);

            playerData = formatter.Deserialize(fileStream) as PlayerState;
            fileStream.Close();
            return playerData;
        } else {
            Debug.LogError("Data files not found.");
            throw new System.Exception("Data files not found. Unable to load game.");
        }
    }

    public static void InitializeGame(){
        SavePlayerState(PlayerStateDefaultValues());
    }

    private static PlayerState PlayerStateDefaultValues(){
        return new PlayerState((int)SceneID.FirstIsland, 
                                3,
                                3,
                                new Vector3(-23.75f,-1.57f,0));
    }

}
