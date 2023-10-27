using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public BulletShoot bulletShoot;
    public GameObject Target_aim;
    public GameObject win_panel;
    public RigBuilder rig;
    public GameObject Aim_target;
    public GameObject Weapon;

    private bool is_win;
    // Start is called before the first frame update
    public void Win()
    {
        Weapon.SetActive(false);
        gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Dance", true);
        rig.enabled = false;
        is_win = true;
        StartCoroutine(windelay());
    }
    IEnumerator windelay()
    {
        yield return new WaitForSeconds(2);
        if (SceneManager.GetActiveScene().name == "50")
        {
            SceneManager.LoadScene(0);
        }

        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameLoader>().GetLevel() <= int.Parse(SceneManager.GetActiveScene().name))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<SaveGame>().saveGameData.Level = int.Parse(SceneManager.GetActiveScene().name) + 1;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameLoader>().SetLevel(int.Parse(SceneManager.GetActiveScene().name) + 1);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<SaveGame>().SaveToFile();
        }
        win_panel.SetActive(true);
    }

    void Update()
    {
        if (is_win)
        {
            gameObject.transform.GetChild(0).gameObject.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        else
        {
            if (gameObject.transform.GetChild(0).transform.position.x < Target_aim.transform.position.x)
            {
                gameObject.transform.GetChild(0).gameObject.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
            }
            else
            {
                gameObject.transform.GetChild(0).gameObject.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
            }
        }
    }

    public void Lose_()
    {
        gameObject.GetComponent<AudioSource>().Play();
        bulletShoot.Lose();
    }
}
