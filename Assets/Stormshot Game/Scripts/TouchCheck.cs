using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class TouchCheck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMP_Text Level_display;
    public AudioSource audioSource;
    public BulletShoot bulletShoot;
    public Animator Player_animator;
    public GameObject Lizer;
    public GameObject Target_icon;

    void Start()
    {
        Level_display.text = "Level " + SceneManager.GetActiveScene().name;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Lizer.SetActive(true);
        Target_icon.SetActive(true);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        Lizer.SetActive(false);
        Target_icon.SetActive(false);
        Player_animator.SetTrigger("Fire");
        audioSource.Play();
        bulletShoot.Shoot();
    }

    public void back_main_menu()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().game_over = false;
        SceneManager.LoadScene("Menu");
    }

    public void next_level()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().game_over = false;
        SceneManager.LoadScene(int.Parse(SceneManager.GetActiveScene().name) + 1);
    }
}