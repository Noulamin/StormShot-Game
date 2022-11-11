using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    private int Current_level;
    public string saveDirectory = "Saves";
    public string saveName = "savedGame";

    public void LoadFromFile()
    {
        // Converts binary file back into readable data for Unity game
        BinaryFormatter formatter = new BinaryFormatter();

        // Choosing the saved file to open
        try
        {
            FileStream saveFile = File.Open(saveDirectory + "/" + saveName + ".bin", FileMode.Open);
            // Convert the file data into SaveGameData format for use in game
            SaveGameData loadData = (SaveGameData) formatter.Deserialize(saveFile);

            saveFile.Close();
            Current_level = loadData.Level;
            gameObject.GetComponent<LevelManager>().SetLevel(loadData.Level);
        }
        catch (System.Exception)
        {
            //Debug.Log("Date not exist yet.");
        }
    }

    public void SetLevel(int index)
    {
        Current_level = index;
    }

    public int GetLevel()
    {
        return Current_level;
    }
}
