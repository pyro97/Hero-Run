using System.Collections;
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
    GameObject playBtn, panelListaSettings, panelMenuSettings, panelGameSettings, panelShopSettings, panelClassificaSettings;
    PlayerPrefsHandler playerPrefsHandler;
    Button buttonMusica, buttonEffetti;
    GameObject handlerMusica, handlerEffetti;
    public AudioClip clipClickButton, clipSblocco;
    AudioSource sourceClick, sourceSblocco;
    public GameObject[] players;
    List<User> listaOrdinata = new List<User>();


    private void Awake()
    {
        playerPrefsHandler = new PlayerPrefsHandler();

        if (AudioListener.pause)
        {
            AudioListener.pause = false;
            AudioListener.volume = 1f;
        }


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

        /*
        for (int i = 0; i < 10; i++)
        {
            User user = new User("fff"+i, i + 1);
            RestClient.Post("https://corun-b2a77.firebaseio.com/utenti/" +".json", user);
        }
        */
        ApriMenuSetting(false);
        ApriGameSetting(false);
        ApriShopSetting(false);
        ApriClassificaSetting(false);

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

    public void ApriClassificaSetting(bool val)
    {
        if (val)
        {
            PulsantiHomeAttivi(false);
            AttivaClassificaSetting(true);
        }
        else
        {
            PulsantiHomeAttivi(true);
            AttivaClassificaSetting(false);
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

    public void Right1T2()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
    }

    public void Left2T1()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
    }

    public void Right2T3()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(true);
    }

    public void Left3T2()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
        panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
    }

    public void Right3T4()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
        panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(true);
    }

    public void Left4T3()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);
        panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(true);
    }

    public void Right4T5()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);
        panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(true);
    }

    public void Left5T4()
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }
        panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);
        panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(true);
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
            panelGameSettings.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            panelGameSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(7).gameObject.SetActive(false);
            attivaCatalogo();
            panelGameSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
            GameObject moneteCatalogo = panelGameSettings.transform.GetChild(1).GetChild(2).gameObject;
            moneteCatalogo.transform.GetChild(0).GetComponent<Text>().text = playerPrefsHandler.GetMonete().ToString();




        }
        else if (indexSubPanel == 1)
        {
            panelGameSettings.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            panelGameSettings.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Image>().color = new Color32(164, 164, 164, 255);
            panelGameSettings.transform.GetChild(0).GetChild(3).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);


            panelGameSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(4).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(5).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(6).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(7).gameObject.SetActive(false);
            panelGameSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        }
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
            panelGameSettings.transform.GetChild(1).GetChild(7).gameObject.SetActive(true);
        }

    }

    public void SbloccaGiocatore(string s)
    {
        GameObject daSbloccare = null;
        print(players[0]);
        print(players[1]);
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name.Equals(s)) daSbloccare = players[i];
        }
        print(daSbloccare);
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

    public void OpenSubPanelClassifica(int indexSubPanel)
    {
        if (sourceClick.enabled)
        {
            sourceClick.Play();
        }

        if (indexSubPanel == 0)
        {
            if (Score.connessione)
            {
                //modificare quando ci saranno piu pannelli
                getListaClassifica();



                GameObject righe = GameObject.Find("ListaUtentiClassifica");

                for (int i = 0; i < 10; i++)
                {

                    GameObject row = righe.transform.GetChild(i).gameObject;
                    row.transform.GetChild(0).gameObject.GetComponent<Text>().text = listaOrdinata[i].punti.ToString();
                    row.transform.GetChild(1).gameObject.GetComponent<Text>().text = listaOrdinata[i].nome.ToString();
                }

            }
            else
            {
                //popup
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
            //print(response.Text);

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
                if (i == 9)
                {
                    Score.ultimoPunteggioClassifica = listaOrdinata[i].punti;
                }
            }

        });


    }


}
