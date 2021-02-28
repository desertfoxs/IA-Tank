using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float speed = 100f;
    public float rotation = 1f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }

    //Vector3 Movement;

    void Update()
    {
        //movimiento de adelante y atras
        if (Input.GetKey("w"))
        {
            Vector3 Movement = transform.up * -speed * Time.deltaTime; 
            rb2d.AddForce(Movement);
            
        }

        if (Input.GetKey("s"))
        {
            Vector3 Movement = transform.up * speed * Time.deltaTime;
            rb2d.AddForce(Movement);

        }


        //movimiento de rotacion
        if (Input.GetKey("a"))
        {         
            transform.Rotate(rotation * Vector3.forward, Space.World);
        }

        if (Input.GetKey("d"))
        {
           
            transform.Rotate(-rotation * Vector3.forward, Space.World);
        }
    }
}