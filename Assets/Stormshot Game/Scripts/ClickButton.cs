using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ClickButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => {
            if(!gameObject.transform.GetChild(0).gameObject.activeSelf)
            {
                gameObject.GetComponent<AudioSource>().Play();
                return;
            }
            SceneManager.LoadScene(int.Parse(gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text));
        });
    }
}
