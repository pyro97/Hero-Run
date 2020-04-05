using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScript : MonoBehaviour
{
    AudioSource musicaMenu;
    GameObject playBtn, panelListaSettings, panelMenuSettings;
    PlayerPrefsHandler playerPrefsHandler;
    // Start is called before the first frame update
    void Start()
    {
        playBtn = GameObject.Find("PlayButton");
        panelListaSettings = GameObject.Find("PanelListaSettings");
        panelMenuSettings = GameObject.Find("PanelMenuSettings");

        playerPrefsHandler =new PlayerPrefsHandler();
        //playerPrefsHandler.CreateFirstTimePref();
        if (playerPrefsHandler.isFirstTime())
        {
            playerPrefsHandler.CreateFirstTimePref();
            playerPrefsHandler.InitializePreferences();
        }
        else
        {
            //setta tutte le preferenze dell'utente nel menu
        }

        ChiudiMenuSetting();

        if (!playerPrefsHandler.GetIsMutedMusica()) { 
            musicaMenu = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
            musicaMenu.enabled = true;
            musicaMenu.Play();
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
            panelMenuSettings.SetActive(false);

        }
        else
        {
            panelMenuSettings.SetActive(true);
            OpenSubPanelMenu(0);


        }
    }

    //0-> stat, 1->setting, 2->info
    public void OpenSubPanelMenu(int indexSubPanel)
    {
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
            Slider slider=GameObject.Find("Slider").GetComponent<Slider>();

            slider.value = playerPrefsHandler.GetVolume();

            slider.onValueChanged.AddListener(delegate {

                playerPrefsHandler.SetVolume(slider.value);
                AudioListener.volume = playerPrefsHandler.GetVolume();
            });

        }

        else if (indexSubPanel == 2)
        {
            panelMenuSettings.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            panelMenuSettings.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        }
  
    }

}
