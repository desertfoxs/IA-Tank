using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameControler : MonoBehaviour
{
    public static int Score;
    public TextMeshProUGUI TextScore;
    public static GameControler gameControler;
    public string nombreEscena;

    private bool pause = false;
    public GameObject panelPause;
    private Move playerScript;

    void Awake()
    {
        gameControler = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Move>();
        TextScore.text = Score.ToString();
    }


    void Update()
    {
        if (Input.GetKey("r"))
        {
            RestarGame();
        }

        if (pause)
        {
            Time.timeScale = 0;
            playerScript.enabled = false;          
            panelPause.SetActive(true);

        }
        else if (!pause)
        {
            Time.timeScale = 1;
            playerScript.enabled = true;
            panelPause.SetActive(false);

        }


    }

    public void SumarPuntos(int puntos)
    {
        Score = Score + puntos;
        TextScore.text = Score.ToString();

    }

    public void RestarGame()
    {
        SceneManager.LoadScene(nombreEscena);
        Score = 0;
    }

    public void Muerte()
    {        
        pause = true;
    }

}
