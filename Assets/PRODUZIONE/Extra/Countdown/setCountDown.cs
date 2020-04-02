using UnityEngine;
using System.Collections;

public class setCountDown : MonoBehaviour {

    private GameManagerScript gms;

    public void setCountDow()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        gms.counterDownDone = true;
    }

}
