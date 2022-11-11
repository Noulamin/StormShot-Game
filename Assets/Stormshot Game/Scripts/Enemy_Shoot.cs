using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;
    public GameObject Weapon;
    public Animator Enemy;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    public void Start_Shooting()
    {
        Enemy.SetBool("Aim",true);
        boxCollider2D.offset = new Vector2(0.01703317f,0.82f);
        Weapon.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
        InvokeRepeating("Shooting",1.5f,1.5f);
    }

    void Update()
    {
        
    }

    void Shooting()
    {
        if(!GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().game_over)
        {
            var bullet = Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = gameObject.transform.up * bulletSpeed;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
