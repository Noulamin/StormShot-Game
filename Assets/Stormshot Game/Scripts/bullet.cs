using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;

using UnityEngine;

public class bullet : MonoBehaviour
{
    public int the_winner = 2;
    public float DestroyAfter = 10;
 
    void Awake()
    {
        Destroy(gameObject, DestroyAfter);
    }
 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            Destroy(gameObject.GetComponent<CircleCollider2D>());
            GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().game_over = true;
            collision.gameObject.GetComponent<Animator>().SetBool("Death",true);
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            collision.gameObject.GetComponent<AudioSource>().Play();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Win();
            Destroy(gameObject);

        }

        if(collision.collider.tag == "Player")
        {
            Destroy(gameObject.GetComponent<CircleCollider2D>());
            GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().game_over = true;
            collision.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Death",true);
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>().SetBool("Dance",true);
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<RigBuilder>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Lose_();
            Destroy(gameObject);
        }
    }
}
