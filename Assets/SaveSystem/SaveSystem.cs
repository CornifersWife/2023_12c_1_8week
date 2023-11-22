using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem
{
    public static class SimpleSaveSystem
    {
        public static void SaveBinary()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + "/saves/save.suffering", FileMode.Create);

            SaveData data = new SaveData();
            EventSystem.SaveEventSystem.SaveGame(data);
            serializer.Serialize(stream, data);

            stream.Close();
        }

        public static void LoadBinary()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + "/saves/save.suffering", FileMode.Open);

            SaveData data = serializer.Deserialize(stream) as SaveData;
            EventSystem.SaveEventSystem.LoadGame(data);

            stream.Close();
        }
    }
}