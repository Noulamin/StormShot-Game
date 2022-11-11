using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Enemy_Shoot enemy_Shoot;
    public float bulletSpeed = 10;
    private int Bullet_limit = 4;

    public GameObject[] Bullets;
    public GameObject Lose_panel;

    public void Shoot()
    {
        if(Bullet_limit != 0)
        {
            Bullet_limit--;
            Bullets[Bullet_limit].SetActive(false);
            if(Bullet_limit == 2)
            {
                enemy_Shoot.Start_Shooting();
            }
            if(Bullet_limit == 0)
            {
                Lose();
            }
            var bullet = Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = gameObject.transform.up * bulletSpeed;
        }
    }

    public void Lose()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().game_over = true;
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>().SetBool("Dance",true);
        Lose_panel.SetActive(true);
    }
}