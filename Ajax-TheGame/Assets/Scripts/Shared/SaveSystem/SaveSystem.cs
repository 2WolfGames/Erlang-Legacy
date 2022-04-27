using UnityEngine;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string dataPath = Application.persistentDataPath;
    public const string playerStateFileName = "/player_stats.bin";

    public static bool SaveGameExists()
    {
        Debug.Log(dataPath);
        return File.Exists(dataPath + playerStateFileName);
    }

    public static void SavePlayerState(PlayerState playerData)
    {
        Aes aes = Aes.Create();
        aes.Key = new byte();
        aes.IV = yourByteArrayIV;

        string path = dataPath + playerStateFileName;

        // Save
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                new BinaryFormatter().Serialize(fs, playerData);
            }
            fs.Close();
        }

    }

    public static PlayerState LoadPlayerState()
    {
        string path = Application.persistentDataPath + "/player_stats.bin";
        if (File.Exists(path))
        {
            Aes aes = Aes.Create();
            aes.Key = new byte();
            aes.IV = yourByteArrayIV;
            PlayerState playerData;

            // Load
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Read))
                {
                    playerData = (PlayerState)new BinaryFormatter().Deserialize(cs);
                }
                fs.Close();
            }
            return playerData;
        }
        else
        {
            Debug.LogError("Data files not found.");
            throw new System.Exception("Data files not found. Unable to load game.");
        }
    }

    public static void InitializeGame()
    {
        SavePlayerState(PlayerStateDefaultValues());
    }

    private static PlayerState PlayerStateDefaultValues()
    {
        return new PlayerState((int)SceneID.FirstIsland,
                                3,
                                3,
                                new Vector3(-23.75f, -1.57f, 0));
    }

}
