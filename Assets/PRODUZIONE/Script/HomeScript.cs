﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Proyecto26;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class HomeScript : MonoBehaviour
{
    AudioSource musicaMenu;
    GameObject playBtn, panelListaSettings, panelMenuSettings,
        panelGameSettings, panelShopSettings, panelClassificaSettings,
        panelAlertClassifica, panelAlertPersonaggio,panelInputNomeUtente,panelAlertInputNomeUtente,panelAlertNomePresente, panelAlertNoConnInsNome,panelAlertUpdate;
    PlayerPrefsHandler playerPrefsHandler;
    Button buttonMusica, buttonEffetti;
    GameObject handlerMusica, handlerEffetti;
    public AudioClip clipClickButton, clipSblocco;
    AudioSource sourceClick, sourceSblocco;
    public GameObject[] players;
    List<User> listaOrdinata = new List<User>();
    bool listaPiena;
    GameObject buyButton,acquistatoButton,restoreButton;
    string versionName = "1.2.1";



    private void Awake()
    {
        if (Application.systemLanguage != SystemLanguage.Italian)
        {
            SetEnglish();
        }
        playerPrefsHandler = new PlayerPrefsHandler();
        buyButton = GameObject.Find("ButtonRemoveAds");
        acquistatoButton = GameObject.Find("ButtonAcquistato");
        restoreButton = GameObject.Find("ButtonRestore");

        panelAlertInputNomeUtente = GameObject.Find("PanelAlertInputNomeUtente");
        panelAlertInputNomeUtente.SetActive(false);
        panelInputNomeUtente = GameObject.Find("PanelInputNomeUtente");
        panelInputNomeUtente.SetActive(false);
        panelAlertNomePresente = GameObject.Find("PanelAlertNomePresente");
        panelAlertNomePresente.SetActive(false);
        panelAlertNoConnInsNome = GameObject.Find("PanelAlertNoConnInsNome");
        panelAlertNoConnInsNome.SetActive(false);
        panelAlertUpdate = GameObject.Find("PanelAlertUpdate");
        panelAlertUpdate.SetActive(false);


        if (AudioListener.pause)
        {
            AudioListener.pause = false;
            AudioListener.volume = 1f;
        }

        if (playerPrefsHandler.isFirstTime())
        {
            panelInputNomeUtente.SetActive(true);
        }
        else
        {
            if (playerPrefsHandler.GetPersonaggioAttuale().Equals("Medico"))
            {
                playerPrefsHandler.SetPersonaggioAttuale("The Loocka");
            }

            if (!playerPrefsHandler.GetVersionName().Equals(versionName))
            {
                panelAlertUpdate.SetActive(true);
                if (Application.systemLanguage == SystemLanguage.Italian)
                {
                    panelAlertUpdate.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text= "Versione " + versionName;
                }
                else
                {
                    panelAlertUpdate.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "Version " + versionName;
                }
                
                if (playerPrefsHandler.isSamsung())
                {
                    if (Application.systemLanguage == SystemLanguage.Italian)
                    {
                        panelAlertUpdate.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text += "-Usa Swipe Up per sparare invece del tocco sullo schermo.";
                    }
                    else
                    {
                        panelAlertUpdate.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text += "-Use Swipe Up to shoot instead of the touch on the screen.";
                    }

                }
                else
                {
                    if (Application.systemLanguage == SystemLanguage.Italian)
                    {
                        panelAlertUpdate.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text += "-Tocca lo schermo per sparare invece di usare Swipe Up.";
                    }
                    else
                    {
                        panelAlertUpdate.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text += "-Tap the screen to shoot instead of using Swipe Up.";
                    }
                    

                }
            }
            //playerPrefsHandler.SetMonete(6000);

            //TEST TEST TEST TEST

            /*
             *if (playerPrefsHandler.GetMonete() < 1000)
                 {
                     playerPrefsHandler.CreateFirstTimePref();
                     playerPrefsHandler.SetMonete(6000);
                 }
             *
             */

            //playerPrefsHandler.CreateFirstTimePref();

        }

        if (playerPrefsHandler.GetDeviceName().Length == 0)
        {
            playerPrefsHandler.SetDeviceName(SystemInfo.deviceName);
        }

        if (!playerPrefsHandler.GetIsMutedMusica())
        {
            musicaMenu = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
            musicaMenu.enabled = true;
            musicaMenu.Play();
        }

        if (playerPrefsHandler.GetIsMutedEffetti())
        {
            sourceClick = AddAudio(clipClickButton, false, false, false, 0f);
            sourceSblocco = AddAudio(clipSblocco, false, false, false, 0f);


        }
        else
        {
            sourceClick = AddAudio(clipClickButton, false, false, false, 0.25f);
            sourceSblocco = AddAudio(clipSblocco, false, false, false, 25f);



        }



    }



    // Start is called before the first frame update
    void Start()
    {
        listaPiena = false;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Score.connessione = false;
        }
        else
        {
            Score.connessione = true;
        }

        playBtn = GameObject.Find("PlayButton");
        panelListaSettings = GameObject.Find("PanelListaSettings");
        panelMenuSettings = GameObject.Find("PanelMenuSettings");
        panelGameSettings = GameObject.Find("PanelGameSettings");
        panelShopSettings = GameObject.Find("PanelShopSettings");
        panelGameSettings = GameObject.Find("PanelGameSettings");
        panelClassificaSettings = GameObject.Find("PanelClassificaSettings");
        panelAlertClassifica = GameObject.Find("PanelAlertClassifica");
        panelAlertPersonaggio = GameObject.Find("PanelAlertPersonaggio");
        /*
        for (int i = 1; i < 11; i++)
        {
            User user = new User("Giuseppe"+i, i + 10);
            RestClient.Post("https://corun-b2a77.firebaseio.com/utenti"+".json", user);
        }
        */

        SetResponsive();
        ApriMenuSetting(false);
        ApriGameSetting(false);
        ApriShopSetting(false);
        ApriClassificaSetting(false);
        panelAlertClassifica.gameObject.SetActive(false);
        panelAlertPersonaggio.gameObject.SetActive(false);

        if (!playerPrefsHandler.GetIsMutedEffetti())
        {
            sourceClick.enabled = true;


        }

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            getListaClassifica();
            Score.connessione = true;
        }




    }

    // Update is called once per frame
    void Update()
    {
        if (listaOrdinata.Count > 0)
        {
            listaPiena = true;
        }
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


    public void ControllaInputNome()
    {
        InputField inputField = panelInputNomeUtente.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<InputField>();

        if (Application.internetReachability != NetworkReachability.NotReachable || !listaPiena)
        {


            if (inputField.text.ToString().Length > 0 && inputField.text.ToString().Length <= 10)
            {

                bool nomePresente = false;
                foreach (User user in listaOrdinata)
                {
                    if (user.nome == inputField.text.ToString()) nomePresente = true;
                }

                if (!nomePresente)
                {
                    playerPrefsHandler.SetPlayerKey(inputField.text.ToString());
                    playerPrefsHandler.SetVersionName(versionName);
                    playerPrefsHandler.CreateFirstTimePref();
                    panelInputNomeUtente.SetActive(false);
                    panelAlertInputNomeUtente.SetActive(false);
                }
                else
                {
                    panelInputNomeUtente.SetActive(false);
                    panelAlertNomePresente.SetActive(true);

                }


            }
            else
            {
                panelInputNomeUtente.SetActive(false);
                panelAlertInputNomeUtente.SetActive(true);
            }
        }
        else
        {
            panelInputNomeUtente.SetActive(false);
            panelAlertNoConnInsNome.SetActive(true);
        }

    }

    public void ChiudiAlertInputField()
    {
        panelAlertInputNomeUtente.SetActive(false);
        panelInputNomeUtente.SetActive(true);

    }

    public void ChiudiAlertNomeGiaPresente()
    {
        panelAlertNomePresente.SetActive(false);
        panelInputNomeUtente.SetActive(true);

    }

    public void ChiudiAlertNoConnInpNome()
    {
        panelAlertNoConnInsNome.SetActive(false);
        panelInputNomeUtente.SetActive(true);

    }

    public void ChiudiAlertUpdate()
    {
        panelAlertUpdate.SetActive(false);
        playerPrefsHandler.SetVersionName(versionName);
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

    public void ApriClassificaSetting(bool val)
    {
        if (val)
        {
            PulsantiHomeAttivi(false);
            if (!Score.connessione)
            {
                panelAlertClassifica.gameObject.SetActive(true);
            }
            else if (!listaPiena)
            {
                panelAlertClassifica.gameObject.SetActive(true);
                if (Application.systemLanguage == SystemLanguage.Italian)
                {
                    panelAlertClassifica.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "La classifica è ancora in caricamento. Per favore riprova.";
                }
                else
                {
                    panelAlertClassifica.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = "The ranking is still loading. Please try again.";
                }
                
            }
            else
            {
                AttivaClassificaSetting(true);

            }
        }
        else
        {
            PulsantiHomeAttivi(true);
            AttivaClassificaSetting(false);
        }

    }

    public void ChiudiPanelAlertClassifica()
    {
        panelAlertClassifica.gameObject.SetActive(false);
        PulsantiHomeAttivi(true);
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
            if (!playerPrefsHandler.GetRemoveAds())
            {
                buyButton.SetActive(true);
                acquistatoButton.SetActive(false);
                if (Application.platform == RuntimePlatform.Android)
                {
                    restoreButton.SetActive(false);

                }
                else
                {
                    restoreButton.GetComponent<Button>().interactable = true;
                }

            }
            else
            {
                buyButton.SetActive(false);
                acquistatoButton.SetActive(true);

                if (Application.platform == RuntimePlatform.Android)
                {
                    restoreButton.SetActive(false);

                }
                else
                {
                    restoreButton.GetComponent<Button>().interactable = false;
                }


            }
            //OpenSubPanelGame(0);


        }
    }


    public void AttivaClassificaSetting(bool val)
    {
        if (!val)
        {
            if (sourceClick.enabled)
            {
                sourceClick.Play();
            }
            panelClassificaSettings.SetActive(false);

        }
        else
        {
            if (sourceClick.enabled)
            {
                sourceClick.Play();
            }
            panelClassificaSettings.SetActive(true);
            OpenSubPanelClassifica(0);


        }
    }

    public void ChangeEffetti()
    {
        handlerEffetti = GameObject.Find("ButtonEffetti");
        buttonEffetti = handlerEffetti.GetComponent<Button>();
        if (playerPrefsHandler.GetIsMutedEffetti())
        {
            playerPrefsHandler.SetMutedEffetti(false);
            sourceClick.enabled = true;
            sourceClick.volume = 0.25f;
            handlerEffetti.transform.GetChild(0).gameObject.SetActive(true);
            handlerEffetti.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            playerPrefsHandler.SetMutedEffetti(true);
            sourceClick.enabled = false;
            sourceClick.volume = 0f;
            handlerEffetti.transform.GetChild(0).gameObject.SetActive(false);
            handlerEffetti.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void ChangeMusica()
    {
        handlerMusica = GameObject.Find("ButtonMusica");
        buttonMusica = handlerMusica.GetComponent<Button>();

        if (playerPrefsHandler.GetIsMutedMusica())
        {
            playerPrefsHandler.SetMutedMusica(false);
            musicaMenu = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
            musicaMenu.enabled = true;
            musicaMenu.Play();
            handlerMusica.transform.GetChild(0).gameObject.SetActive(true);
            handlerMusica.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            playerPrefsHandler.SetMutedMusica(true);
            musicaMenu = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
            musicaMenu.Pause();
            musicaMenu.enabled = false;
            handlerMusica.transform.GetChild(0).gameObject.SetActive(false);
            handlerMusica.transform.GetChild(1).gameObject.SetActive(true);
        }

    }

    public void AvantiIndietroComandi(int index)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        if (index == 0)
        {
            panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);

        }
        else if (index == 1)
        {
            panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
            if (playerPrefsHandler.isSamsung())
            {
                if (Application.systemLanguage == SystemLanguage.Italian)
                {
                    panelGameSettings.transform.GetChild(1).GetChild(3).GetChild(2).GetComponent<Text>().text = "Movimento: Swipe a destra e a sinitra per cambiare corsia della strada. Sparare: Swipe Up sullo schermo per attivare il lanciafiamme e sparare.";
                }
                else
                {
                    panelGameSettings.transform.GetChild(1).GetChild(3).GetChild(2).GetComponent<Text>().text = "Movement: Swipe right and left to change lane of the road. Shoot: Swipe Up on the screen to activate the flamethrower and shoot.";
                }
            }
              


        }
        else if(index == 2)
        {
            panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);

        }
        else if (index == 3)
        {
            panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);

        }
        else if (index == 4)
        {
            panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(7).gameObject.SetActive(false);


        }
        else if (index == 5)
        {
            panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(7).gameObject.SetActive(true);

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

            panelMenuSettings.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = new Color32(164, 164, 164, 255);
            panelMenuSettings.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            panelMenuSettings.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);


            panelMenuSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            panelMenuSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
            
            GameObject.Find("ScoreVal").GetComponent<Text>().text = playerPrefsHandler.GetRecordPersonale().ToString();
            GameObject.Find("NPartiteVal").GetComponent<Text>().text = playerPrefsHandler.GetNumPartiteTotali().ToString();
            GameObject.Find("NCoinsVal").GetComponent<Text>().text = playerPrefsHandler.GetMonete().ToString(); 


        }
        else if (indexSubPanel == 1)
        {
            panelMenuSettings.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            panelMenuSettings.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().color = new Color32(164, 164, 164, 255);
            panelMenuSettings.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);


            panelMenuSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            panelMenuSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);

            panelMenuSettings.transform.GetChild(1).GetChild(1).GetChild(6).GetComponent<Text>().text = playerPrefsHandler.GetPlayerKey();


            handlerMusica = GameObject.Find("ButtonMusica");
            buttonMusica = handlerMusica.GetComponent<Button>();

            if (playerPrefsHandler.GetIsMutedMusica())
            {
                handlerMusica.transform.GetChild(0).gameObject.SetActive(false);
                handlerMusica.transform.GetChild(1).gameObject.SetActive(true);


                //set immagine musica offf

            }
            else
            {
                handlerMusica.transform.GetChild(0).gameObject.SetActive(true);
                handlerMusica.transform.GetChild(1).gameObject.SetActive(false);
                //set immagine musica on

            }




            handlerEffetti = GameObject.Find("ButtonEffetti");
            buttonEffetti = handlerEffetti.GetComponent<Button>();

            if (playerPrefsHandler.GetIsMutedEffetti())
            {
                handlerEffetti.transform.GetChild(0).gameObject.SetActive(false);
                handlerEffetti.transform.GetChild(1).gameObject.SetActive(true);
                //set effetto off
            }
            else
            {
                handlerEffetti.transform.GetChild(0).gameObject.SetActive(true);
                handlerEffetti.transform.GetChild(1).gameObject.SetActive(false);
                //set effetto on

            }

        }

        else if (indexSubPanel == 2)
        {

            panelMenuSettings.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            panelMenuSettings.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            panelMenuSettings.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>().color = new Color32(164, 164, 164, 255);

            panelMenuSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);

            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(1).gameObject.SetActive(true);
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(2).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(3).gameObject.SetActive(true);
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(4).gameObject.SetActive(false);



        }

    }

    public void CambiaPageCredits(int i)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        if (i == 0)
        {
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(1).gameObject.SetActive(true);
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(2).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(3).gameObject.SetActive(true);
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(4).gameObject.SetActive(false);


        }
        else if (i == 1)
        {
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(1).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(2).gameObject.SetActive(true);
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(3).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(2).GetChild(4).gameObject.SetActive(true);
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
            panelGameSettings.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = new Color32(164, 164, 164, 255);
            panelGameSettings.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            //panelGameSettings.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            panelGameSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(7).gameObject.SetActive(false);
            //panelGameSettings.transform.GetChild(1).GetChild(8).gameObject.SetActive(false);
            attivaCatalogo();
            panelGameSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            GameObject moneteCatalogo = panelGameSettings.transform.GetChild(1).GetChild(2).gameObject;
            moneteCatalogo.transform.GetChild(0).GetComponent<Text>().text = playerPrefsHandler.GetMonete().ToString();

            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);



        }
        else if (indexSubPanel == 1)
        {
            panelGameSettings.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            panelGameSettings.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().color = new Color32(164, 164, 164, 255);
            //panelGameSettings.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);


            panelGameSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(7).gameObject.SetActive(false);
            //panelGameSettings.transform.GetChild(1).GetChild(8).gameObject.SetActive(false);

            panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);

            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        /*
        else if (indexSubPanel == 2)
        {
            panelGameSettings.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            panelGameSettings.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            panelGameSettings.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>().color = new Color32(164, 164, 164, 255);

            panelGameSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(7).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(8).gameObject.SetActive(true);

            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);

        }*/

    }

    public void SbloccaGiocatore(string s)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        GameObject daSbloccare = null;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name.Equals(s)) daSbloccare = players[i];
        }
        Text t = daSbloccare.transform.GetChild(3).GetComponent<Text>();
        int num = int.Parse(t.text.ToString());
        if (playerPrefsHandler.GetMonete() >= num)
        {
            sourceSblocco.enabled = true;
            sourceSblocco.Play();
            playerPrefsHandler.SetMonete(playerPrefsHandler.GetMonete() - num);
            playerPrefsHandler.SetGiocatoreByNome(s);
            attivaCatalogo();
            GameObject moneteCatalogo = panelGameSettings.transform.GetChild(1).GetChild(2).gameObject;
            moneteCatalogo.transform.GetChild(0).GetComponent<Text>().text = playerPrefsHandler.GetMonete().ToString();

        }
        else
        {
            panelAlertPersonaggio.gameObject.SetActive(true);
        }

    }

    public void ChiudiAlertPersonaggio()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        panelAlertPersonaggio.gameObject.SetActive(false);
    }

    public void SelectGiocatore(string s)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
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

    public void AvantiIndietroCatalogo(int index)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        if (index == 0)
        {
            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else if (index == 1)
        {
            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(true);

        }
    }

    public void OpenSubPanelClassifica(int indexSubPanel)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }

        if (indexSubPanel == 0)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                //modificare quando ci saranno piu pannelli
                getListaClassifica();

                GameObject.Find("PanelClassifica").transform.GetChild(0).gameObject.SetActive(true);
                GameObject.Find("PanelClassifica").transform.GetChild(1).gameObject.SetActive(false);
                GameObject.Find("PanelClassifica").transform.GetChild(2).gameObject.SetActive(false);


                GameObject righe = GameObject.Find("ListaUtentiClassifica");

                

                for (int i = 0; i < 10; i++)
                {
                    GameObject row = righe.transform.GetChild(i).gameObject;
                    row.transform.GetChild(0).gameObject.GetComponent<Text>().text = listaOrdinata[i].punti.ToString();
                    row.transform.GetChild(1).gameObject.GetComponent<Text>().text = listaOrdinata[i].nome.ToString();
                    if (listaOrdinata[i].nome.ToString().Equals(playerPrefsHandler.GetPlayerKey()))
                    {
                        row.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.red;
                        row.transform.GetChild(1).gameObject.GetComponent<Text>().color = Color.red;
                        row.transform.GetChild(2).gameObject.GetComponent<Text>().color = Color.red;
                    }
                }

            }
            else
            {
                panelAlertClassifica.SetActive(true);
                ApriClassificaSetting(false);
            }




        }
        if (indexSubPanel == 1)
        {
            GameObject.Find("PanelClassifica").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("PanelClassifica").transform.GetChild(1).gameObject.SetActive(true);
            GameObject.Find("PanelClassifica").transform.GetChild(2).gameObject.SetActive(false);

            GameObject righe2 = GameObject.Find("ListaUtentiClassifica2");

            for (int i = 10; i < 20; i++)
            {
                GameObject row = righe2.transform.GetChild(i-10).gameObject;
                row.transform.GetChild(0).gameObject.GetComponent<Text>().text = listaOrdinata[i].punti.ToString();
                row.transform.GetChild(1).gameObject.GetComponent<Text>().text = listaOrdinata[i].nome.ToString();
                if (listaOrdinata[i].nome.ToString().Equals(playerPrefsHandler.GetPlayerKey()))
                {
                    row.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.red;
                    row.transform.GetChild(1).gameObject.GetComponent<Text>().color = Color.red;
                    row.transform.GetChild(2).gameObject.GetComponent<Text>().color = Color.red;
                }
            }

        }
        if (indexSubPanel == 2)
        {
            GameObject.Find("PanelClassifica").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("PanelClassifica").transform.GetChild(1).gameObject.SetActive(false);
            GameObject.Find("PanelClassifica").transform.GetChild(2).gameObject.SetActive(true);

            GameObject righe3 = GameObject.Find("ListaUtentiClassifica3");

            for (int i = 20; i < 30; i++)
            {
                GameObject row = righe3.transform.GetChild(i-20).gameObject;
                row.transform.GetChild(0).gameObject.GetComponent<Text>().text = listaOrdinata[i].punti.ToString();
                row.transform.GetChild(1).gameObject.GetComponent<Text>().text = listaOrdinata[i].nome.ToString();
                if (listaOrdinata[i].nome.ToString().Equals(playerPrefsHandler.GetPlayerKey()))
                {
                    row.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.red;
                    row.transform.GetChild(1).gameObject.GetComponent<Text>().color = Color.red;
                    row.transform.GetChild(2).gameObject.GetComponent<Text>().color = Color.red;
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
        if (playerPrefsHandler.GetNumPartiteTotali() > 0)
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            SceneManager.LoadScene("Tutorial");

        }

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



    public void getListaClassifica()
    {

        RestClient.Get("https://corun-b2a77.firebaseio.com/" + ".json").Then(response =>
        {

            JObject stringa = JObject.Parse(response.Text);
            // get JSON result objects into a list
            IList<JToken> results = stringa["utenti"].Children().Children().ToList();
            // serialize JSON results into .NET objects
            IList<User> userResults = new List<User>();
            foreach (JToken result in results)
            {
                User u = result.ToObject<User>();

                userResults.Add(u);
            }

            listaOrdinata = userResults.ToList().OrderByDescending(x => x.punti).ToList();




            for (int i = 0; i < listaOrdinata.Count; i++)
            {
                if (i == 29)
                {
                    Score.ultimoPunteggioClassifica = listaOrdinata[i].punti;
                }
            }

        });


    }


    public void SetResponsive()
    {
        print(Screen.height + " " + Screen.width);
        if ((Screen.height > 2600 && Screen.height < 3000) && (Screen.width > 1100 && Screen.width < 1500))
        {

            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);

            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);

            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);

            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);

        }
        else if ((Screen.height > 2500 && Screen.height < 2900) && (Screen.width > 1100 && Screen.width < 1500))
        {
            panelListaSettings.GetComponent<RectTransform>().localPosition = new Vector3(305.5f, -480f, -115f);
            panelListaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelListaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelListaSettings.transform.GetChild(2).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelListaSettings.transform.GetChild(3).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 120f, 0f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 120f, 0f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 120f, 0f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 120f, 0f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

        }
        else if ((Screen.height > 2100 && Screen.height < 2500) && (Screen.width > 1000 && Screen.width < 1500))
        {
            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);

            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);

            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);

            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);

        }
        else if ((Screen.height > 1900 && Screen.height < 2100) && (Screen.width > 1000 && Screen.width < 1500))
        {
            panelListaSettings.GetComponent<RectTransform>().localPosition = new Vector3(305.5f, -480f, -115f);
            panelListaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelListaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelListaSettings.transform.GetChild(2).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelListaSettings.transform.GetChild(3).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 180f, 0f);
            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 180f, 0f);
            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 180f, 0f);
            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 180f, 0f);
            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);



        }

        else if ((Screen.height > 1200 && Screen.height < 1500) && (Screen.width > 700 && Screen.width < 1000))
        {
            panelListaSettings.GetComponent<RectTransform>().localPosition = new Vector3(305.5f, -480f, -115f);
            panelListaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelListaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelListaSettings.transform.GetChild(2).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelListaSettings.transform.GetChild(3).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);

        }

        else if ((Screen.height > 780 && Screen.height < 1200) && (Screen.width > 400 && Screen.width < 700))
        {
            panelListaSettings.GetComponent<RectTransform>().localPosition = new Vector3(305.5f, -450f, -115f);
            panelListaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
            panelListaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
            panelListaSettings.transform.GetChild(2).GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
            panelListaSettings.transform.GetChild(3).GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);

            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 0.85f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 0.85f);

            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 0.85f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 0.85f);

            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 0.85f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 0.85f);

            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 0.85f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 130f, 0f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.85f, 0.85f, 0.85f);

        }
        else if ((Screen.height > 550 && Screen.height < 800) && (Screen.width > 250 && Screen.width < 350))
        {
            panelListaSettings.GetComponent<RectTransform>().localPosition = new Vector3(305.5f, -500f, -115f);
            panelListaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelListaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelListaSettings.transform.GetChild(2).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelListaSettings.transform.GetChild(3).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);

            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelMenuSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelMenuSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);

            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelGameSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelGameSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);

            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelShopSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelShopSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);

            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(0f, 140f, 0f);
            panelClassificaSettings.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(0f, 150f, 0f);
            panelClassificaSettings.transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0.95f);

        }


    }


    void SetEnglish()
    {
        GameObject.Find("NPartiteTitle").GetComponent<Text>().text = Language.partiteTotali;
        GameObject.Find("ScoreTitle").GetComponent<Text>().text = Language.scoreTitle;
        GameObject.Find("TitlePanelStats").GetComponent<Text>().text = Language.titlePanelStats;
        GameObject.Find("NCoinsTitle").GetComponent<Text>().text = Language.nCoinsTitle;
        GameObject.Find("NPartiteTitle").GetComponent<Text>().text = Language.partiteTotali;
        GameObject.Find("TitleMusic").GetComponent<Text>().text = Language.titleMusic;
        GameObject.Find("TitleEffetti").GetComponent<Text>().text = Language.titleEffetti;
        GameObject.Find("ComandiButton").transform.GetChild(0).GetComponent<Text>().text = Language.comandi;
        GameObject.Find("TitlePanelPlayer").GetComponent<Text>().text = Language.titlePanelPlayer;
        GameObject.Find("NPartiteTitle").GetComponent<Text>().text = Language.partiteTotali;
        GameObject.Find("Select").transform.GetChild(0).GetComponent<Text>().text = Language.seleziona;
        GameObject.Find("The Loocka").transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = Language.sblocca;
        GameObject.Find("Peppe").transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = Language.sbloccaPeppe;
        GameObject.Find("PanelComandiSlide1").transform.GetChild(0).GetComponent<Text>().text = Language.panelComandiSlideTextTitle;
        GameObject.Find("TextSlide1").GetComponent<Text>().text = Language.panelComandiSlide1TextSlide1;
        GameObject.Find("PanelComandiSlide2").transform.GetChild(0).GetComponent<Text>().text = Language.comandi;
        GameObject.Find("TextSlide2").GetComponent<Text>().text = Language.panelComandiSlide2TextSlide2;
        GameObject.Find("PanelComandiSlide3").transform.GetChild(0).GetComponent<Text>().text = Language.panelComandiSlide3TextTitle;
        GameObject.Find("TextSlide3").GetComponent<Text>().text = Language.panelComandiSlide3TextSlide;
        GameObject.Find("PanelComandiSlide4").transform.GetChild(0).GetComponent<Text>().text = Language.panelComandiSlide4TextTitle;
        GameObject.Find("TextSlide4").GetComponent<Text>().text = Language.panelComandiSlide4TextSlide;
        GameObject.Find("PanelComandiSlide5").transform.GetChild(0).GetComponent<Text>().text = Language.panelComandiSlide5TextTitle;
        GameObject.Find("TextSlide5").GetComponent<Text>().text = Language.panelComandiSlide5TextSlide;
        GameObject.Find("TextSlide6").GetComponent<Text>().text = Language.panelComandiSlide6TextSlide;
        GameObject.Find("PanelShop1").transform.GetChild(3).GetComponent<Text>().text = Language.rimuoviPubblicità;
        GameObject.Find("ButtonAcquistato").transform.GetChild(0).GetComponent<Text>().text = Language.buttonAcquistato;
        GameObject.Find("ButtonRestore").transform.GetChild(0).GetComponent<Text>().text = Language.ripristinaAcquisti;
        GameObject.Find("ButtonRemoveAds").transform.GetChild(0).GetComponent<Text>().text = Language.buttonRemoveAds;
        GameObject.Find("TitlePanelClassifica").GetComponent<Text>().text = Language.titleClassifica;
        GameObject.Find("PanelAlertClassifica").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Language.alertClassifica;
        GameObject.Find("PanelAlertPersonaggio").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Language.alertPersonaggio;
        GameObject.Find("PanelAlertInputNomeUtente").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Language.alertNick;
        GameObject.Find("PanelInputNomeUtente").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Language.panelInput;
        GameObject.Find("PanelAlertNomePresente").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Language.alertNomeInserito;
        GameObject.Find("PanelAlertNoConnInsNome").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Language.alertConnesione;
        GameObject.Find("PanelAlertPersonaggio").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Language.alertPersonaggio;
        GameObject.Find("PanelAlertUpdate").transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Language.alertUpdate;
    }


}
