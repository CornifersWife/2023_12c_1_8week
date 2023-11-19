using System;
namespace EventSystem 
{
    public static class SaveEventSystem
    {
        public static event Action<SaveData> OnSaveGame;
        public static event Action<SaveData> OnLoadGame;

        public static void SaveGame(SaveData data)
        {
            OnSaveGame?.Invoke(data);
        }

        public static void LoadGame(SaveData data)
        {
            OnLoadGame?.Invoke(data);
        }
    }
}
