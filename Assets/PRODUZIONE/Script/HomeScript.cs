using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class HomeScript : MonoBehaviour
{
    AudioSource musicaMenu;
    GameObject playBtn, panelListaSettings, panelMenuSettings, panelGameSettings, panelShopSettings;
    PlayerPrefsHandler playerPrefsHandler;
    Toggle toggleMusica, toggleEffetti;
    public AudioClip clipClickButton;
    AudioSource sourceClick;
    public GameObject[] players;

    private void Awake()
    {
        playerPrefsHandler = new PlayerPrefsHandler();

       

        if (playerPrefsHandler.isFirstTime())
        {
            playerPrefsHandler.CreateFirstTimePref();
        }
        else
        {
            //TEST TEST TEST TEST

            //playerPrefsHandler.CreateFirstTimePref();
            // PlayerPrefs.DeleteKey("Medico");
            /*
             *if (playerPrefsHandler.GetMonete() < 1000)
                 {
                     playerPrefsHandler.CreateFirstTimePref();
                     playerPrefsHandler.SetMonete(6000);
                 }
             *
             */

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
            sourceClick = AddAudio(clipClickButton, false, false,false, 0.25f);


        }



    }



    // Start is called before the first frame update
    void Start()
    {
        playBtn = GameObject.Find("PlayButton");
        panelListaSettings = GameObject.Find("PanelListaSettings");
        panelMenuSettings = GameObject.Find("PanelMenuSettings");
        panelGameSettings = GameObject.Find("PanelGameSettings");
        panelShopSettings = GameObject.Find("PanelShopSettings");



        ApriMenuSetting(false);
        ApriGameSetting(false);
        ApriShopSetting(false);
        if (!playerPrefsHandler.GetIsMutedEffetti())
        {
            sourceClick.enabled = true;

        }

    }

    // Update is called once per frame
    void Update()
    {
      
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


    public void ApriMenuSetting(bool val)
    {
        if (val)
        {
            PulsantiHomeAttivi(false);
            AttivaMenuSetting(true);
        }
        else
        {
            PulsantiHomeAttivi(true);
            AttivaMenuSetting(false);
        }
        
    }

    public void ApriGameSetting(bool val)
    {
        if (val)
        {
            PulsantiHomeAttivi(false);
            AttivaGameSetting(true);
        }
        else
        {
            PulsantiHomeAttivi(true);
            AttivaGameSetting(false);
        }

    }

    public void ApriShopSetting(bool val)
    {
        if (val)
        {
            PulsantiHomeAttivi(false);
            AttivaShopSetting(true);
        }
        else
        {
            PulsantiHomeAttivi(true);
            AttivaShopSetting(false);
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

    public void AttivaGameSetting(bool val)
    {
        if (!val)
        {
            if (sourceClick.enabled)
            {
                sourceClick.Play();
            }
            panelGameSettings.SetActive(false);

        }
        else
        {
            if (sourceClick.enabled)
            {
                sourceClick.Play();
            }
            panelGameSettings.SetActive(true);
            OpenSubPanelGame(0);


        }
    }

    public void AttivaShopSetting(bool val)
    {
        if (!val)
        {
            if (sourceClick.enabled)
            {
                sourceClick.Play();
            }
            panelShopSettings.SetActive(false);

        }
        else
        {
            if (sourceClick.enabled)
            {
                sourceClick.Play();
            }
            panelShopSettings.SetActive(true);
            //OpenSubPanelGame(0);


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
            GameObject.Find("NCoinsVal").GetComponent<Text>().text = playerPrefsHandler.GetMonete().ToString();



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
                    sourceClick.volume = 0.25f;

                }
                else
                {
                    playerPrefsHandler.SetMutedEffetti(true);
                    sourceClick.enabled = false;
                    sourceClick.volume = 0f;

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


    public void OpenSubPanelGame(int indexSubPanel)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }

        if (indexSubPanel == 0)
        {
            panelGameSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            attivaCatalogo();
            panelGameSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            GameObject moneteCatalogo = panelGameSettings.transform.GetChild(1).GetChild(2).gameObject;
            moneteCatalogo.transform.GetChild(0).GetComponent<Text>().text = playerPrefsHandler.GetMonete().ToString();




        }
        else if (indexSubPanel == 1)
        {
            panelGameSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
         
           

        }

    }

    public void SbloccaGiocatore(string s)
    {
        GameObject daSbloccare=null;
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i].name.Equals(s)) daSbloccare = players[i];
        }

        Text t=daSbloccare.transform.GetChild(3).GetComponent<Text>();
        int num = int.Parse(t.text.ToString());
        if (playerPrefsHandler.GetMonete() >= num)
        {
            playerPrefsHandler.SetMonete(playerPrefsHandler.GetMonete() - num);
            playerPrefsHandler.SetGiocatoreByNome(s);
            attivaCatalogo();
            GameObject moneteCatalogo = panelGameSettings.transform.GetChild(1).GetChild(2).gameObject;
            moneteCatalogo.transform.GetChild(0).GetComponent<Text>().text = playerPrefsHandler.GetMonete().ToString();

        }

    }

    public void SelectGiocatore(string s)
    {
        GameObject daSelezionare = null;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name.Equals(s)) daSelezionare = players[i];
        }

        playerPrefsHandler.SetPersonaggioAttuale(daSelezionare.name);
        attivaCatalogo();
        

    }

    public void attivaCatalogo()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].transform.GetChild(2).gameObject.SetActive(true);
            players[i].transform.GetChild(1).gameObject.SetActive(true);

        }
        for (int i = 0; i < players.Length; i++)
        {
            if (playerPrefsHandler.GetGiocatoreByNome(players[i].name))
            {
                players[i].transform.GetChild(2).gameObject.SetActive(false);//pulsante sblocca

                if (playerPrefsHandler.GetPersonaggioAttuale().Equals(players[i].name))
                {
                    players[i].transform.GetChild(1).gameObject.SetActive(false);//pulsante seleziona
                }
            }

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
