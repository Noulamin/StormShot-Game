using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public SaveGameData saveGameData;

    public void SaveToFile()
    {
        // To save in a directory, it must be created first
        if (!Directory.Exists(Application.persistentDataPath))
            Directory.CreateDirectory(Application.persistentDataPath);

        // The formatter will convert our unity data type into a binary file
        BinaryFormatter formatter = new BinaryFormatter();

        // Choose the save location
        FileStream saveFile = File.Create(Application.persistentDataPath + "/Saves.data");

        // Write our C# Unity game data type to a binary file
        formatter.Serialize(saveFile, saveGameData);

        saveFile.Close();

        // Success message
        //print("Game Saved to " + Directory.GetCurrentDirectory().ToString() + "/Saves/" + saveName + ".bin");
    }
}
