using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
     GameObject player;
    Vector3 distanza;

    PlayerPrefsHandler playerPrefs;


    void Awake()
    {
        playerPrefs = new PlayerPrefsHandler();
        player = GameObject.Find(playerPrefs.GetPersonaggioAttuale());
    }

    // Start is called before the first frame update
    void Start()
    {
        distanza = player.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Score.animazioneFine)
        {
            follow();

        }
 
    }


    public void follow()
    {
        this.transform.position = player.transform.position - distanza;
    }

}
