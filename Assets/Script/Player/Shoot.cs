using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
  
    public Transform shotpos;
    public GameObject Bullet;
    private bool disparo = true; 
   
    
    //mecanica a desarrollar mas a adelante
    //public int cantidadBalas;

    void Start()
    {}

 
    void Update()
    {
        if (Input.GetKey("space") && disparo)
        {
            Instantiate(Bullet, shotpos.transform.position, shotpos.transform.rotation);
            StartCoroutine(Shooting(1.3f));
        }

    }

    //corrutina
    IEnumerator Shooting(float seconds)
    {
        disparo = false;
        yield return new WaitForSeconds(seconds);
        disparo = true;
    }
}
 
