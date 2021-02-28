using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector3 targetPos;

    // Variables para gestionar el radio de visión, el de ataque y la velocidad
    public float visionRadius;
    public float attackRadius;
    public float speed;

    // Variables relacionadas con el ataque
    [Tooltip("Velocidad de ataque (segundos entre ataques)")]
    public float attackSpeed = 2f;
    bool attacking;
    public Transform shotpos;
    public GameObject BulletEnemy;

    ///--- Variables relacionadas con la vida
    [Tooltip("Puntos de vida")]
    public int maxHp = 1;
    [Tooltip("Vida actual")]
    private double hp;

    // Variable para guardar al jugador
    GameObject player;
 

    // Variable para guardar la posición inicial
    Vector3 initialPosition, target;

    // Animador y cuerpo cinemático con la rotación en Z congelada
    Animator anim;
    Rigidbody2D rb2d;

    //movimiento aleatorio
    private bool movAleato = true;

    //variable para los puntos
    public int puntos = 20;
    public GameControler gameControler;

    //para acceder al collider;
    private BoxCollider2D m_ObjectCollider;

    //variables para la muerte
    private bool muerto = false;
    public GameObject explocionMuerte;
    private bool variable = true;

    //sonido
    //public GameObject[] SonidoEnemy;
    //private bool variable = true;




    void Start()
    {

        // Recuperamos al jugador gracias al Tag
        player = GameObject.FindGameObjectWithTag("Player");

        m_ObjectCollider = GetComponent<BoxCollider2D>();

        //anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        ///--- Iniciamos la vida
        hp = maxHp;
    }

    void Update()
    {

        if (!muerto)
        {

            // Guardamos nuestra posición inicial
            initialPosition = transform.position;

            // Por defecto nuestro target siempre será nuestra posición inicial
            target = initialPosition;

            // Comprobamos un Raycast del enemigo hasta el jugador
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                player.transform.position - transform.position,
                visionRadius,
                1 << LayerMask.NameToLayer("Default")
            // Poner el propio Enemy en una layer distinta a Default para evitar el raycast
            // También poner al objeto Attack y al Prefab Slash una Layer Attack 
            // Sino los detectará como entorno y se mueve atrás al hacer ataques
            );

            // Aquí podemos debugear el Raycast
            Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
            Debug.DrawRay(transform.position, forward, Color.red);

            // Si el Raycast encuentra al jugador lo ponemos de target
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    target = player.transform.position;
                }
               
            }

            // Calculamos la distancia y dirección actual hasta el target
            float distance = Vector3.Distance(target, transform.position);
            Vector3 dir = (target - transform.position).normalized;

            // Si es el enemigo y está en rango de ataque nos paramos y le atacamos
            if (target != initialPosition && distance < attackRadius)
            {
                //poner la animacion de mover aca
                //gameObject.GetComponent<Animator>().SetBool("move", false);

                //rotacion del enemy;
                Vector3 dir2 = (player.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;
                angle = angle + 90f;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                ///-- Empezamos a atacar (importante una Layer en ataque para evitar Raycast)
                if (!attacking) StartCoroutine(Attack(attackSpeed));
            }
            // En caso contrario nos movemos hacia él
            else
            {
                if (hit.collider.tag == "Player") {
                    //rotacion del enemy;
                    
                    Vector3 dir2 = (player.transform.position - transform.position).normalized;

                    //targetPos = new Vector3(player.transform.rotation.x, player.transform.rotation.y, transform.rotation.z);
                    //Quaternion dir2 = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetPos), 10 * Time.deltaTime);

                    float angle = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;

                    //angle = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetPos), 10 * Time.deltaTime);

                    angle = angle + 90f;

                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    
                }

                rb2d.MovePosition(transform.position + dir * speed * Time.deltaTime);

                //poner la animacion de mover
                //gameObject.GetComponent<Animator>().SetBool("move", true);
            }

            // Una última comprobación para evitar bugs forzando la posición inicial
            if (target == initialPosition && distance < 0.05f)
            {
                // Y cambiamos la animación de nuevo a Idle
                //gameObject.GetComponent<Animator>().SetBool("move", false);

                if (movAleato)
                {
                    float movimientoX = Random.Range(-1f, 1f);
                    float movimientoY = Random.Range(-1f, 1f);
                    float time = Random.Range(7f, 15f);

                    Vector2 offset = new Vector2(movimientoX, movimientoY);
                    StartCoroutine(MovimientoAleatorio(time, offset));
                }
            }
        }



    }

    //la funcion para que caminen cuando no estan atacando
    public IEnumerator MovimientoAleatorio(float seconds, Vector2 offset)
    {

        movAleato = false;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        rb2d.velocity = offset * 700f * Time.fixedDeltaTime;

        //gameObject.GetComponent<Animator>().SetBool("move", true);

        yield return new WaitForSeconds(3f);

        rb2d.velocity = Vector2.zero;
        //gameObject.GetComponent<Animator>().SetBool("move", false);

        yield return new WaitForSeconds(seconds);

        movAleato = true;

    }


    IEnumerator Attack(float seconds)
    {
        attacking = true;
      
        Instantiate(BulletEnemy, shotpos.transform.position, shotpos.transform.rotation);
    
        yield return new WaitForSeconds(seconds);
        attacking = false;
    }


    ///--- Gestión del daño de las armas y la vida
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullets" && !muerto)
        {
            hp = hp - 1;
            if (hp == 0)
            {
                muerto = true;                               
                Instantiate(explocionMuerte, transform.position, Quaternion.identity);
                Destroy(gameObject);

                StartCoroutine(muerte(2.25f));               
                gameControler.SumarPuntos(puntos);
            }

            //Instantiate(SonidoEnemy[1], transform.position, Quaternion.identity);
        }

    }

    public IEnumerator muerte(float seconds)
    {
        SonidoMuerte();
        yield return new WaitForSeconds(seconds);

    }

    // el sonido de la muerte
    private void SonidoMuerte()
    {

        if (variable)
        {
            //Instantiate(SonidoEnemy[0], transform.position, Quaternion.identity);
            variable = false;
        }
    }
}