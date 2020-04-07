using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelezionaGiocatore : MonoBehaviour
{
    public GameObject[] players;
    PlayerPrefsHandler playerPrefs;
    string scelto;

    void Awake()
    {
        playerPrefs = new PlayerPrefsHandler();
        scelto = playerPrefs.GetPersonaggioAttuale();
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i].name.Equals(scelto))
            {
                players[i].SetActive(true);
            }
            else
            {
                players[i].SetActive(false);

            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
