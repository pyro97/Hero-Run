using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds;
using System;
using Proyecto26;
using Newtonsoft.Json.Linq;
using System.Linq;

public class MenuScript : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    Text punti, puntiMenu, puntiFinali, testoFinale;
    GameObject panel, panelFine, panelScore, panelAlert, panelAlertRicomincia;
    GameObject settaggi;
    PlayerPrefsHandler playerPrefs;
    AudioSource sourceClick;
    public AudioClip musicaClick;
    Button buttonVideo;
    string appUnitId;
    string idRewardedAd, idInterstitialAd;
    List<User> listaOrdinata = new List<User>();
    bool punteggioInserito = false;

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
        if (Application.platform == RuntimePlatform.Android)
        {
            appUnitId = "ca-app-pub-3940256099942544~3347511713";
            idRewardedAd = "ca-app-pub-3940256099942544/5224354917";
            idInterstitialAd = "ca-app-pub-3940256099942544/1033173712";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            appUnitId = "ca-app-pub-3940256099942544~1458002511";
            idRewardedAd = "ca-app-pub-3940256099942544/1712485313";
            idInterstitialAd = "ca-app-pub-3940256099942544/4411468910";
        }

        MobileAds.Initialize(appUnitId);


        rewardedAd = new RewardedAd(idRewardedAd);
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;


        interstitialAd = new InterstitialAd(idInterstitialAd);
        // Called when an ad request has successfully loaded.
        this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;


        RequestRewardVideo();
        RequestInterstitial();

        Time.timeScale = 1;
        panel = GameObject.Find("Panel");
        panelScore = GameObject.Find("PanelScore");
        panelFine = GameObject.Find("PanelFine");
        settaggi = GameObject.Find("Setting");
        panelAlert = GameObject.Find("PanelAlert");
        panelAlertRicomincia = GameObject.Find("PanelAlertRicomincia");
        puntiMenu = GameObject.Find("PuntiMenu").GetComponent<Text>();
        punti = GameObject.Find("Punti").GetComponent<Text>();
        puntiFinali = GameObject.Find("PuntiFinali").GetComponent<Text>();
        testoFinale = GameObject.Find("TitleFinale").GetComponent<Text>();
        buttonVideo = GameObject.Find("Continua").GetComponent<Button>();
        panel.gameObject.SetActive(false);
        panelFine.gameObject.SetActive(false);
        panelAlert.gameObject.SetActive(false);
        panelScore.gameObject.SetActive(true);
        panelAlertRicomincia.gameObject.SetActive(false);
        if (!Score.continua)
        {
            Score.punteggio = 0;

        }
        Score.fine = false;
        if (!playerPrefs.GetIsMutedEffetti())
        {
            sourceClick.enabled = true;

        }
    }





    // Update is called once per frame
    void Update()
    {


        if (Score.fine)
        {
            apriMenuFine();
        }

        if (punteggioInserito)
        {
            playerPrefs.SetMonete(playerPrefs.GetMonete() + Score.monete);
            panelScore.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
            Score.buttonPause = false;
            SceneManager.LoadScene("Home");
            Score.punteggio = 0;
            Score.fine = false;
            Time.timeScale = 0;
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

    public void apriMenu()
    {
        //StartCoroutine(waitForClickSound());

        AvviaMusicaPausa();
        Score.pause = false;
        panelScore.gameObject.SetActive(false);
        panelAlert.gameObject.SetActive(false);
        panelAlertRicomincia.gameObject.SetActive(false);
        panel.gameObject.SetActive(true);
        Score.buttonPause = false;
        punti.gameObject.SetActive(false);
        puntiMenu.text = "" + Score.punteggio;
        Time.timeScale = 0;
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


    public void AvviaMusicaGiocoDaFineGioco()
    {
        if (!playerPrefs.GetIsMutedMusica())
        {
            GameObject.Find("FinalMusic").GetComponent<AudioSource>().Stop();
            GameObject.Find("FinalMusic").GetComponent<AudioSource>().enabled = false;

            GameObject.Find("Music").GetComponent<AudioSource>().enabled = true;
            GameObject.Find("Music").GetComponent<AudioSource>().Play();
        }

    }
    public void apriMenuFine()
    {

        int bestRecord = playerPrefs.GetRecordPersonale();
        //panelScore.gameObject.SetActive(false);
        if (Score.punteggio > bestRecord)
        {
            playerPrefs.SetRecordPersonale(Score.punteggio);
            testoFinale.text = "Nuovo Record";
        }
        panelFine.gameObject.SetActive(true);
        panel.gameObject.SetActive(false);
        //Score.buttonPause = false;
        punti.gameObject.SetActive(false);
        puntiFinali.gameObject.SetActive(true);
        puntiFinali.text = "" + Score.punteggio;

        if (Score.adWatched || Application.internetReachability == NetworkReachability.NotReachable)
        {
            buttonVideo.interactable = false;
        }

        Time.timeScale = 0;
        Score.fine = false;


    }

    public void chiudiMenu()
    {
        //StartCoroutine(waitForClickSound());
        AvviaMusicaGiocoDaPausa();
        panelScore.gameObject.SetActive(true);
        panel.gameObject.SetActive(false);
        Score.buttonPause = false;
        punti.gameObject.SetActive(true);
        Time.timeScale = 1;
        Score.pause = true;
        Score.countdown = true;


    }

    public void esci()
    {

        if (Score.connessione && Application.internetReachability != NetworkReachability.NotReachable &&
            Score.punteggio > Score.ultimoPunteggioClassifica)
        {
            getListaClassifica();
            if (Score.punteggio > Score.ultimoPunteggioClassifica)
            {
                User user = new User(playerPrefs.GetPlayerKey(), Score.punteggio);
                //RestClient.Delete("https://corun-b2a77.firebaseio.com/utenti.json?orderBy="+"idUtente"+"&equalTo=" + Score.ultimoIdClassifica);
                RestClient.Post("https://corun-b2a77.firebaseio.com/utenti" + ".json", user).Then(response => {

                    punteggioInserito = true;
                });
            }


        }
        else
        {
            if (Score.CountInterstitial != 3)
            {
                Score.CountInterstitial += 1;
                playerPrefs.SetMonete(playerPrefs.GetMonete() + Score.monete);
                //StartCoroutine(waitForClickSound());
                panelScore.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                Score.buttonPause = false;
                SceneManager.LoadScene("Home");
                Score.punteggio = 0;
                Score.fine = false;
                Time.timeScale = 0;
            }
            else
            {

                Score.CountInterstitial = 0;

                ShowInterstitialAd();
            }
        }





    }

    public void esciPausa()
    {
        OpenAlert();

    }


    public void HomeDaPausa()
    {
        if (Score.CountInterstitial != 3)
        {
            Score.CountInterstitial += 1;
            panelScore.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
            panelAlert.gameObject.SetActive(false);
            Score.buttonPause = false;
            SceneManager.LoadScene("Home");
            Score.punteggio = 0;
            Score.fine = false;
            Time.timeScale = 0;
        }
        else
        {

            Score.CountInterstitial = 0;

            ShowInterstitialAd();
        }
    }
    void OpenAlert()
    {
        panelAlert.gameObject.SetActive(true);
    }

    public void ricomincia()
    {

        //StartCoroutine(waitForClickSound());
        OpenAlertRicomincia();



    }

    void OpenAlertRicomincia()
    {
        panelAlertRicomincia.gameObject.SetActive(true);
    }

    public void RestartAlert()
    {
        panelScore.gameObject.SetActive(false);
        panelAlertRicomincia.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        Score.buttonPause = false;
        SceneManager.LoadScene("Game");
        Score.punteggio = 0;
        Score.fine = false;

        Time.timeScale = 0;
    }


    public void ContinuaPartita()
    {
        Score.Premiato = false;
        //waitForClickSound();
        ShowRewardedVideo();
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







    private void RequestRewardVideo()
    {
        rewardedAd.LoadAd(CreateNewRequest());
    }

    private void ShowRewardedVideo()
    {
        if (rewardedAd.IsLoaded())
        {
            AudioListener.pause = true;
            AudioListener.volume = 0;
            rewardedAd.Show();
        }
    }


    private void RequestInterstitial()
    {
        interstitialAd.LoadAd(CreateNewRequest());
    }


    private void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            AudioListener.pause = true;
            AudioListener.volume = 0;
            interstitialAd.Show();
        }
    }

    private AdRequest CreateNewRequest()
    {
        return new AdRequest.Builder().Build();
    }


    //REWARDED VIDEO
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        RequestRewardVideo();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        AudioListener.pause = true;
        AudioListener.volume = 0f;
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        RequestRewardVideo();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardVideo();

        if (Score.Premiato)
        {
            panelScore.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
            Score.buttonPause = false;
            SceneManager.LoadScene("Game");
            Score.fine = false;
            Time.timeScale = 0;
            Score.continua = true;
        }
        else
        {
            AudioListener.pause = false;
            AudioListener.volume = 1f;
            buttonVideo.interactable = false;


        }


    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Score.Premiato = true;

    }



    //INTERSTITIAL
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        playerPrefs.SetMonete(playerPrefs.GetMonete() + Score.monete);
        //StartCoroutine(waitForClickSound());
        panelScore.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        Score.buttonPause = false;
        SceneManager.LoadScene("Home");
        Score.punteggio = 0;
        Score.fine = false;
        Time.timeScale = 0;
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        AudioListener.pause = true;
        AudioListener.volume = 0f;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        interstitialAd.Destroy();
        playerPrefs.SetMonete(playerPrefs.GetMonete() + Score.monete);
        //StartCoroutine(waitForClickSound());
        panelScore.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        Score.buttonPause = false;
        SceneManager.LoadScene("Home");
        Score.punteggio = 0;
        Score.fine = false;
        Time.timeScale = 0;

    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {

    }




    /*
    public void OnDestroy()
    {
        rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
        rewardedAd.OnAdOpening -= HandleRewardedAdOpening;

        rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;



        // Called when an ad request has successfully loaded.
        this.interstitialAd.OnAdLoaded -= HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAd.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAd.OnAdOpening -= HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed -= HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitialAd.OnAdLeavingApplication -= HandleOnAdLeavingApplication;

    }*/






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
                if (i == 9)
                {
                    Score.ultimoPunteggioClassifica = listaOrdinata[i].punti;
                }
            }
        });


    }



}
