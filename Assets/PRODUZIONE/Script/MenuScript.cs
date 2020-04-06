using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour
{

     Text punti,puntiMenu,puntiFinali; 
     GameObject panel,panelFine,panelScore;
     GameObject settaggi;
     PlayerPrefsHandler playerPrefs;
    AudioSource sourceClick;
    public AudioClip musicaClick;


    void Awake()
    {
        playerPrefs = new PlayerPrefsHandler();

        if (playerPrefs.GetIsMutedEffetti())
        {
            sourceClick = AddAudio(musicaClick, false, false, 0f);

        }
        else
        {
            sourceClick = AddAudio(musicaClick, false, false, 1f);

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        panel = GameObject.Find("Panel");
        panelScore = GameObject.Find("PanelScore");
        panelFine = GameObject.Find("PanelFine");
        settaggi = GameObject.Find("Setting");
        puntiMenu = GameObject.Find("PuntiMenu").GetComponent<Text>();
        punti = GameObject.Find("Punti").GetComponent<Text>();
        puntiFinali = GameObject.Find("PuntiFinali").GetComponent<Text>();
        panel.gameObject.SetActive(false);
        panelFine.gameObject.SetActive(false);
        panelScore.gameObject.SetActive(true);
        Score.punteggio=0;
        Score.fine=false;
        if (!playerPrefs.GetIsMutedEffetti())
        {
            sourceClick.enabled = true;

        }
    }

    // Update is called once per frame
    void Update()
    {


       if(Score.fine){
            apriMenuFine();                              
         }

        if (Score.buttonPause)
        {
            settaggi.SetActive(true);
        }
        else
        {
            settaggi.SetActive(false);

        }

    }

    public void apriMenu(){
        StartCoroutine(waitForClickSound());

        AvviaMusicaPausa();
        Score.pause = false;
        panelScore.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        Score.buttonPause = false;
        AvviaMusicaPausa();
        punti.gameObject.SetActive(false);
        puntiMenu.text = "" + Score.punteggio;
        Time.timeScale=0;
    }

    public void AvviaMusicaPausa()
    {
        if (!playerPrefs.GetIsMutedMusica())
        {
            GameObject.Find("Music").GetComponent<AudioSource>().Stop();
            GameObject.Find("Music").GetComponent<AudioSource>().enabled = false;

            GameObject.Find("PauseMusic").GetComponent<AudioSource>().enabled = true;
            GameObject.Find("PauseMusic").GetComponent<AudioSource>().Play();
        }
    }

    public void AvviaMusicaGiocoDaPausa()
    {
        if (!playerPrefs.GetIsMutedMusica())
        {
            GameObject.Find("PauseMusic").GetComponent<AudioSource>().Stop();
            GameObject.Find("PauseMusic").GetComponent<AudioSource>().enabled = false;

            GameObject.Find("Music").GetComponent<AudioSource>().enabled = true;
            GameObject.Find("Music").GetComponent<AudioSource>().Play();
        }
        
    }

    public void apriMenuFine()
    {
       

            panelScore.gameObject.SetActive(false);
            panelFine.gameObject.SetActive(true);
            panel.gameObject.SetActive(false);
            Score.buttonPause = false;
            punti.gameObject.SetActive(false);
            puntiFinali.gameObject.SetActive(true);
            puntiFinali.text = "" + Score.punteggio;
             int bestRecord = playerPrefs.GetRecordPersonale();
            if(Score.punteggio > bestRecord){
                playerPrefs.SetRecordPersonale(Score.punteggio);
            }
             Time.timeScale = 0;
            Score.fine = false;
        
   
    }

    public void chiudiMenu(){
        StartCoroutine(waitForClickSound());



        AvviaMusicaGiocoDaPausa();
        panelScore.gameObject.SetActive(true);
        panel.gameObject.SetActive(false);
        Score.buttonPause = false;
        AvviaMusicaGiocoDaPausa();
        punti.gameObject.SetActive(true);
        Time.timeScale=1;
        Score.pause = true;
        Score.countdown = true;


    }

    public void esci(){
        StartCoroutine(waitForClickSound());



        panelScore.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        Score.buttonPause = false;
        SceneManager.LoadScene("Home");
       Score.punteggio=0;    
       Score.fine=false;
        Time.timeScale = 0;


    }

    public void ricomincia(){
        StartCoroutine(waitForClickSound());

        panelScore.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        Score.buttonPause = false;
        SceneManager.LoadScene("Game");
        Score.punteggio=0;
        Score.fine = false;
        Time.timeScale = 0;


    }


    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.enabled = false;
        newAudio.volume = vol;
        return newAudio;
    }

    IEnumerator waitForClickSound()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        yield return new WaitForSeconds(0.5f);
       
    }



}
