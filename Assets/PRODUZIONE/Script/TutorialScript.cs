using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        if (Application.systemLanguage != SystemLanguage.Italian)
        {
            SetEnglish();
        }

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
            if (playerPrefsHandler.isSamsung())
            {
                if (Application.systemLanguage == SystemLanguage.Italian)
                {
                    string italianGameMovements = "Movimento: Swipe a destra e a sinitra per cambiare corsia della strada.SPAZIOSparare: Swipe Up sullo schermo per attivare il lanciafiamme e sparare.";
                    panelTutorial.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = italianGameMovements.Replace("SPAZIO", "\n");
                }
                else
                {
                    panelTutorial.transform.GetChild(1).GetChild(2).GetComponent<Text>().text = Language.panelComandiSlide2TextSlide2Samsung.Replace("SPAZIO", "\n");
                }
                //TODO
            }
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
            if (Application.systemLanguage == SystemLanguage.Italian && Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string comandiBonusIos = "Cuore: ti permetterà di evitare la morte se toccherai un alieno o una persona.SPAZIODistintivo: ti permetterà di non essere ricercato se toccherai un poliziotto.";
                panelTutorial.transform.GetChild(5).GetChild(2).GetComponent<Text>().text = comandiBonusIos.Replace("SPAZIO", "\n");

            }
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

    void SetEnglish()
    {
        GameObject.Find("PanelTutorial").transform.GetChild(0).GetChild(1).GetComponent<Text>().text = Language.panelComandiSlideTextTitle;
        GameObject.Find("PanelTutorial").transform.GetChild(0).GetChild(2).GetComponent<Text>().text = Language.panelComandiSlide1TextSlide1;

        GameObject.Find("PanelTutorial").transform.GetChild(1).GetChild(1).GetComponent<Text>().text = Language.comandi;
        GameObject.Find("PanelTutorial").transform.GetChild(1).GetChild(2).GetComponent<Text>().text = Language.panelComandiSlide2TextSlide2.Replace("SPAZIO", "\n"); ;

        GameObject.Find("PanelTutorial").transform.GetChild(2).GetChild(1).GetComponent<Text>().text = Language.panelComandiSlide3TextTitle;
        GameObject.Find("PanelTutorial").transform.GetChild(2).GetChild(2).GetComponent<Text>().text = Language.panelComandiSlide3TextSlide;

        GameObject.Find("PanelTutorial").transform.GetChild(3).GetChild(1).GetComponent<Text>().text = Language.panelComandiSlide4TextTitle;
        GameObject.Find("PanelTutorial").transform.GetChild(3).GetChild(2).GetComponent<Text>().text = Language.panelComandiSlide4TextSlide;

        GameObject.Find("PanelTutorial").transform.GetChild(4).GetChild(1).GetComponent<Text>().text = Language.panelComandiSlide5TextTitle;
        GameObject.Find("PanelTutorial").transform.GetChild(4).GetChild(2).GetComponent<Text>().text = Language.panelComandiSlide5TextSlide;

        GameObject.Find("PanelTutorial").transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>().text = Language.saltaTutorial;
        GameObject.Find("PanelTutorial").transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<Text>().text = Language.saltaTutorial;
        GameObject.Find("PanelTutorial").transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<Text>().text = Language.saltaTutorial;
        GameObject.Find("PanelTutorial").transform.GetChild(3).GetChild(3).GetChild(0).GetComponent<Text>().text = Language.saltaTutorial;
        GameObject.Find("PanelTutorial").transform.GetChild(4).GetChild(3).GetChild(0).GetComponent<Text>().text = Language.saltaTutorial;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GameObject.Find("PanelTutorial").transform.GetChild(5).GetChild(2).GetComponent<Text>().text = Language.panelComandiSlide6TextSlideIos.Replace("SPAZIO", "\n");

        }
        else
        {
            GameObject.Find("PanelTutorial").transform.GetChild(5).GetChild(2).GetComponent<Text>().text = Language.panelComandiSlide6TextSlide.Replace("SPAZIO", "\n");
        }
    }
}
