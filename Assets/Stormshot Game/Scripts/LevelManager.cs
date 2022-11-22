using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public bool game_over = false;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.GetComponent<GameLoader>().LoadFromFile();
    }
    
    public void SetLevel(int index)
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<Level_Closer>().SetLevel(index);
    }
}
