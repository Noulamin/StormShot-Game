using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public string saveName = "savedGame";
    public string directoryName = "Saves";
    public SaveGameData saveGameData;

    public void SaveToFile()
    {
        // To save in a directory, it must be created first
        if (!Directory.Exists(directoryName))
            Directory.CreateDirectory(directoryName);

        // The formatter will convert our unity data type into a binary file
        BinaryFormatter formatter = new BinaryFormatter();

        // Choose the save location
        FileStream saveFile = File.Create(directoryName + "/" + saveName + ".bin");

        // Write our C# Unity game data type to a binary file
        formatter.Serialize(saveFile, saveGameData);

        saveFile.Close();

        // Success message
        //print("Game Saved to " + Directory.GetCurrentDirectory().ToString() + "/Saves/" + saveName + ".bin");
    }
}
