using UnityEngine;
using System.Text;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

public static class SaveSystem
{
    public static string dataPath = Application.persistentDataPath;
    public const string playerStateFileName = "/player_stats.bin";
    private const string key = "5ai2M8OF19lfMV590dvy8iU1a23ZXa21";
    private static byte[] keyInBytes = System.Text.Encoding.ASCII.GetBytes(key);
    private const string iv = "a57T0s5UOp1wS4x9";
    private static byte[] ivInBytes = System.Text.Encoding.ASCII.GetBytes(iv);


    public static bool SaveGameExists()
    {
        Debug.Log(dataPath);
        return File.Exists(dataPath + playerStateFileName);
    }

    public static void SavePlayerState(PlayerState playerData)
    {
        string path = dataPath + playerStateFileName;

        Aes aes = Aes.Create();
        aes.Key = keyInBytes;
        aes.IV = ivInBytes;

        var encryptor = aes.CreateEncryptor();
        MemoryStream ms = new MemoryStream();
        byte[] data = ObjectToByteArray(playerData);

        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            cs.Write(data, 0, data.Length);
        }

        System.IO.File.WriteAllBytes(path, ms.ToArray());

    }

    public static PlayerState LoadPlayerState()
    {
        string path = Application.persistentDataPath + "/player_stats.bin";
        if (File.Exists(path))
        {
            try
            {
                
                Aes aes = Aes.Create();
                aes.Key = keyInBytes;
                aes.IV = ivInBytes;

                byte[] data = FileToByteArray(path);

                var decryptor = aes.CreateDecryptor();
                MemoryStream ms = new MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }


                return (PlayerState) ByteArrayToObject(ms.ToArray());

            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }
        else
        {
            Debug.LogError("Data files not found.");
            throw new System.Exception("Data files not found. Unable to load game.");
        }
    }

    public static byte[] FileToByteArray(string FileName)
    {
        byte[] fileBytes = null;

        if (File.Exists(FileName))
        {
            fileBytes = System.IO.File.ReadAllBytes(FileName);
        }
        else
        {
            Console.WriteLine("File '{0}' not found", FileName);
        }

        return fileBytes;
    }

    private static byte[] ObjectToByteArray(System.Object obj)
    {
        if (obj == null)
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, obj);

        return ms.ToArray();
    }

    private static System.Object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();
        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);
        System.Object obj = (System.Object) binForm.Deserialize(memStream);

        return obj;
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
