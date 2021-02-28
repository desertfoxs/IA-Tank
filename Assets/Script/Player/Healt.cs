using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healt : MonoBehaviour
{

    public int vida;
    public GameObject explocionMuerte;

    public SpriteRenderer spriteRenderer;

    public GameControler gameControler;

    private bool muerto = false;
    
    //public int escudo

    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }
   
    void Update()
    {
        if (muerto)
        {
            gameControler.Muerte();
        }
    
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletsEnemy")
        {
            vida = vida - 1;
            if (vida == 0)
            {
                Instantiate(explocionMuerte, transform.position, Quaternion.identity);
                this.spriteRenderer.enabled = false; 
                //Destroy(gameObject);

                StartCoroutine(muerte(1f));
                
            }

            //Instantiate(SonidoEnemy[1], transform.position, Quaternion.identity);
        }

    }

    public IEnumerator muerte(float seconds)
    {
        //SonidoMuerte();
        yield return new WaitForSeconds(seconds);
        muerto = true;
    }
}
