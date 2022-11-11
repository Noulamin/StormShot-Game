using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTowardReflection : MonoBehaviour
{
    private float bias = 0.5f;

    [SerializeField]
    private Vector3 initialVelocity;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float bounceVelocity = 10f;

    private Vector3 lastFrameVelocity;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVelocity;
    }

    private void Update()
    {
        lastFrameVelocity = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().game_over)
        {
            if(collision.gameObject.tag != "Enemy")
            {
                gameObject.GetComponent<AudioSource>().Play();
            }

            if(collision.gameObject.tag != "Player")
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
        
        Bounce(collision.contacts[0].normal);
    }
    
    private void Bounce(Vector3 collisionNormal)
    {
        var speed = lastFrameVelocity.magnitude;
        var bounceDirection = Vector3.Reflect(lastFrameVelocity * 0.2f, collisionNormal);
        var directionToPlayer = playerTransform.position - transform.position;

        var direction = Vector3.Lerp(bounceDirection, directionToPlayer, bias);

        rb.velocity = direction * bounceVelocity;
    }
}