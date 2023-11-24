using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem
{
    public static class SimpleSaveSystem
    {

        public const string saveFileName = "/save.suffering";
        public const string saveFileDirectory = "/../Saves";

        public static void SaveBinary()
        {
            BinaryFormatter serializer = new BinaryFormatter();

            if(!Directory.Exists(Application.dataPath + saveFileDirectory)) 
            { 
                Directory.CreateDirectory(Application.dataPath + saveFileDirectory);
            }         

            FileStream stream = new FileStream(GetSaveLocation(), FileMode.Create);

            SaveData data = new SaveData();
            EventSystem.SaveEventSystem.SaveGame(data);
            serializer.Serialize(stream, data);

            stream.Close();
        }

        public static void LoadBinary()
        {

            //if (!Directory.Exists(Application.dataPath + saveFileDirectory)) { return; }

            BinaryFormatter serializer = new BinaryFormatter();

            FileStream stream = new FileStream(GetSaveLocation(), FileMode.Open);

            SaveData data = serializer.Deserialize(stream) as SaveData;
            EventSystem.SaveEventSystem.LoadGame(data);

            stream.Close();
        }

        public static string GetSaveLocation() 
        {
            return Application.dataPath + saveFileDirectory + saveFileName;
        }

    }
}