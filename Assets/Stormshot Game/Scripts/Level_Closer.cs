using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Level_Closer : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Level_tab;
    private int Current_level = 1;


    public Sprite Already_played_sprite;
    public Sprite On_played_sprite;
    public Sprite Locked_sprite;

    // pages
    public GameObject page_1;
    public GameObject page_2;
    public GameObject page_3;

    private int Current_page = 1;


    public GameObject[] levels;

    void Start()
    {
        if(Current_page == 1)
        {
            page_1.SetActive(true);
            page_2.SetActive(false);
            page_3.SetActive(false);
        }

        if(Current_page == 2)
        {
            page_1.SetActive(false);
            page_2.SetActive(true);
            page_3.SetActive(false);
        }

        if(Current_page == 3)
        {
            page_1.SetActive(false);
            page_2.SetActive(false);
            page_3.SetActive(true);
        }

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameLoader>().LoadFromFile();
        level_checker();
    }

    public void Start_button()
    {
        //Handheld.Vibrate();
        Menu.SetActive(false);
        Level_tab.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
        level_checker();
    }

    public void Next_page()
    {
        if(Current_page != 3)
        {
            Current_page++;
            check_pages_again();
        }
    }

    public void Last_page()
    {
        if(Current_page != 1)
        {
            Current_page--;
            check_pages_again();
        }
    }

    void check_pages_again()
    {
        if(Current_page == 1)
        {
            page_1.SetActive(true);
            page_2.SetActive(false);
            page_3.SetActive(false);
        }

        if(Current_page == 2)
        {
            page_1.SetActive(false);
            page_2.SetActive(true);
            page_3.SetActive(false);
        }

        if(Current_page == 3)
        {
            page_1.SetActive(false);
            page_2.SetActive(false);
            page_3.SetActive(true);
        }
    }

    public void back_button()
    {
        Menu.SetActive(true);
        Level_tab.SetActive(false);
    }

    
    public void SetLevel(int index)
    {
        Current_level = index;
        check_pages_again();
        level_checker();
    }

    void level_checker()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if(int.Parse(levels[i].transform.GetChild(0).GetComponent<TMP_Text>().text) == Current_level)
            {
                levels[i].GetComponent<Image>().sprite = On_played_sprite;
                levels[i].transform.GetChild(0).gameObject.SetActive(true);
                levels[i].transform.GetChild(1).gameObject.SetActive(false);
            }

            if(int.Parse(levels[i].transform.GetChild(0).GetComponent<TMP_Text>().text) < Current_level)
            {
                levels[i].GetComponent<Image>().sprite = Already_played_sprite;
                levels[i].transform.GetChild(0).gameObject.SetActive(true);
                levels[i].transform.GetChild(1).gameObject.SetActive(false);
            }

            if(int.Parse(levels[i].transform.GetChild(0).GetComponent<TMP_Text>().text) > Current_level)
            {
                levels[i].GetComponent<Image>().sprite = Locked_sprite;
                levels[i].transform.GetChild(0).gameObject.SetActive(false);
                levels[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }


}
