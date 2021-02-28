using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float speed;



    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Vector3 Movement = transform.up * -80000 * Time.deltaTime;
        rb2d.velocity = Movement;
        Destroy(gameObject, 3f);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "muro")
        {

            Destroy(gameObject);

        }
    }
}
