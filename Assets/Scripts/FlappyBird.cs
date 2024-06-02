using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBird : MonoBehaviour
{

    [SerializeField] private float flapForce = 1.0f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, rb.velocity.y * rotationSpeed);
    }

    public void Flap()
    {
        rb.velocity = Vector2.up* flapForce;
        transform.rotation= Quaternion.Euler(0, 0, rb.velocity.y * rotationSpeed);
    }

    public void FlyDown()
    {
        rb.AddForce(new Vector2(0.005f,0));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.name == "Line")
        {
            GameManager.instance.IncreaseScore(1);
        }
        else if(collision.gameObject.tag == "Coin")
        {
            GameManager.instance.BirdGetCoin(collision.gameObject);
        }
    }


    private void OnBecameInvisible()
    {
        if (!this.gameObject.scene.isLoaded) return;
        GameManager.instance.LoseGame();
    }
}
