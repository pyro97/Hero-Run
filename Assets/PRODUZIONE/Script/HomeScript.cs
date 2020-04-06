using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class HomeScript : MonoBehaviour
{
    AudioSource musicaMenu;
    GameObject playBtn, panelListaSettings, panelMenuSettings;
    PlayerPrefsHandler playerPrefsHandler;
    Toggle toggleMusica, toggleEffetti;
    public AudioClip clipClickButton;
    AudioSource sourceClick;

    private void Awake()
    {
        playerPrefsHandler = new PlayerPrefsHandler();
        playerPrefsHandler.CreateFirstTimePref();

        if (playerPrefsHandler.isFirstTime())
        {
            playerPrefsHandler.CreateFirstTimePref();
        }
        else
        {
            //setta tutte le preferenze dell'utente nel menu
        }

        if (!playerPrefsHandler.GetIsMutedMusica())
        {
            musicaMenu = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
            musicaMenu.enabled = true;
            musicaMenu.Play();
        }

        if (playerPrefsHandler.GetIsMutedEffetti())
        {
            sourceClick = AddAudio(clipClickButton, false, false,false, 0f);
           
        }
        else
        {
            sourceClick = AddAudio(clipClickButton, false, false,false, 1f);


        }



    }



    // Start is called before the first frame update
    void Start()
    {
        playBtn = GameObject.Find("PlayButton");
        panelListaSettings = GameObject.Find("PanelListaSettings");
        panelMenuSettings = GameObject.Find("PanelMenuSettings");


        ChiudiMenuSetting();
        if (!playerPrefsHandler.GetIsMutedEffetti())
        {
            sourceClick.enabled = true;

        }

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void ApriMenuSetting()
    {
        PulsantiHomeAttivi(false);
        AttivaMenuSetting(true);
    }

    public void ChiudiMenuSetting()
    {
        PulsantiHomeAttivi(true);
        AttivaMenuSetting(false);
    }

    public void PulsantiHomeAttivi(bool val)
    {
        if (!val)
        {
            playBtn.SetActive(false);
            panelListaSettings.SetActive(false);
        }
        else
        {
            playBtn.SetActive(true);
            panelListaSettings.SetActive(true);
        }
    }

    public void AttivaMenuSetting(bool val)
    {

        if (!val)
        {
            if (sourceClick.enabled)
            {
                sourceClick.Play();
            }
            panelMenuSettings.SetActive(false);

        }
        else
        {
            if (sourceClick.enabled)
            {
                sourceClick.Play();
            }
            panelMenuSettings.SetActive(true);
            OpenSubPanelMenu(0);


        }
    }

    //0-> stat, 1->setting, 2->info
    public void OpenSubPanelMenu(int indexSubPanel)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }

        if (indexSubPanel == 0)
        {
            panelMenuSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            panelMenuSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            GameObject.Find("ScoreVal").GetComponent<Text>().text = playerPrefsHandler.GetRecordPersonale().ToString();
            GameObject.Find("NPartiteVal").GetComponent<Text>().text = playerPrefsHandler.GetNumPartiteTotali().ToString();
            GameObject.Find("NCoinsVal").GetComponent<Text>().text = playerPrefsHandler.GetMoneteTotali().ToString();



        }
        else if (indexSubPanel == 1)
        {
            panelMenuSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            panelMenuSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);

            toggleMusica=GameObject.Find("ToggleMusica").GetComponent<Toggle>();
            if (playerPrefsHandler.GetIsMutedMusica())
            {
                toggleMusica.isOn = false;
                

            }
            else
            {
                toggleMusica.isOn = true;

            }


            toggleMusica.onValueChanged.AddListener(delegate {
                if (toggleMusica.isOn)
                {
                    playerPrefsHandler.SetMutedMusica(false);
                    musicaMenu = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
                    musicaMenu.enabled = true;
                    musicaMenu.Play();
                }
                else
                {
                    playerPrefsHandler.SetMutedMusica(true);
                    musicaMenu = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
                    musicaMenu.Pause();
                    musicaMenu.enabled = false;

                }
            });

             toggleEffetti = GameObject.Find("ToggleEffetti").GetComponent<Toggle>();
            if (playerPrefsHandler.GetIsMutedEffetti())
            {
                toggleEffetti.isOn = false;
            }
            else
            {
                toggleEffetti.isOn = true;
            }


            toggleEffetti.onValueChanged.AddListener(delegate {
                if (toggleEffetti.isOn)
                {
                    playerPrefsHandler.SetMutedEffetti(false);
                    sourceClick.enabled = true;

                }
                else
                {
                    playerPrefsHandler.SetMutedEffetti(true);
                    sourceClick.enabled = false;
                }
            });

        }

        else if (indexSubPanel == 2)
        {
            panelMenuSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        }
  
    }


    public void loadGame()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene("Game");

    }

    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake,bool enab,float vol)
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
