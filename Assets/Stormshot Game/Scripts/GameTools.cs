using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameTools : MonoBehaviour
{
    public Transform Aim_target;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(Aim_target);
    }

    public void reload_scene()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().game_over = false;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
