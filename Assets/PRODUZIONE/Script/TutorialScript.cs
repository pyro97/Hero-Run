using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{

    GameObject panelTutorial;
    AudioSource musicaMenu;
    // Start is called before the first frame update
    PlayerPrefsHandler playerPrefsHandler;
    AudioSource sourceClick;
    public AudioClip clipClickButton;


    void Awake()
    {
        playerPrefsHandler = new PlayerPrefsHandler();
        if (!playerPrefsHandler.GetIsMutedMusica())
        {
            musicaMenu = GameObject.Find("Music").GetComponent<AudioSource>();
            musicaMenu.enabled = true;
            musicaMenu.Play();
        }

        if (playerPrefsHandler.GetIsMutedEffetti())
        {
            sourceClick = AddAudio(clipClickButton, false, false, false, 0f);
        }
        else
        {
            sourceClick = AddAudio(clipClickButton, false, false, false, 0.25f);

        }
    }
    void Start()
    {
        panelTutorial = GameObject.Find("PanelTutorial");
        if (!playerPrefsHandler.GetIsMutedEffetti())
        {
            sourceClick.enabled = true;

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void attivaFinestra(int index)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }

        if (index == 0)
        {
            panelTutorial.transform.GetChild(0).gameObject.SetActive(true);
            panelTutorial.transform.GetChild(1).gameObject.SetActive(false);

        }
        else if (index == 1)
        {
            panelTutorial.transform.GetChild(0).gameObject.SetActive(false);
            panelTutorial.transform.GetChild(1).gameObject.SetActive(true);
            panelTutorial.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (index == 2)
        {
            panelTutorial.transform.GetChild(1).gameObject.SetActive(false);
            panelTutorial.transform.GetChild(2).gameObject.SetActive(true);
            panelTutorial.transform.GetChild(3).gameObject.SetActive(false);
        }
        else if (index == 3)
        { 
            panelTutorial.transform.GetChild(2).gameObject.SetActive(false);
            panelTutorial.transform.GetChild(3).gameObject.SetActive(true);
            panelTutorial.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (index == 4)
        {
            panelTutorial.transform.GetChild(3).gameObject.SetActive(false);
            panelTutorial.transform.GetChild(4).gameObject.SetActive(true);
            panelTutorial.transform.GetChild(5).gameObject.SetActive(false);
        }
        else if (index == 5)
        {
            panelTutorial.transform.GetChild(4).gameObject.SetActive(false);
            panelTutorial.transform.GetChild(5).gameObject.SetActive(true);
        }
    }

    public void gioca()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        SceneManager.LoadScene("Game");
    }


    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, bool enab, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.enabled = enab;
        newAudio.volume = vol;
        return newAudio;
    }
}
