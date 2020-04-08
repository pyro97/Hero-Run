using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator : MonoBehaviour
{

    public GameObject bonusMask;
    public GameObject bonusPaper;
    public List<GameObject> bonus;
    GameObject player;
    public bool paper;
    public bool mask;

    PlayerPrefsHandler playerPrefs;


    void Awake()
    {
        playerPrefs = new PlayerPrefsHandler();
        player = GameObject.Find(playerPrefs.GetPersonaggioAttuale());
    }


    // Start is called before the first frame update
    void Start()
    {
        paper = false;
        mask = false;
        bonus = new List<GameObject>();
        BonusCreate();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyBonus();
    }

    void BonusCreate()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        float xPos = Random.Range(-4.1f, 5.1f);
        if (xPos < -1)
        {
            xPos = -3;
        }
        else if (xPos > 1)
        {
            xPos = 3;
        }
        else
        {
            xPos = 0;
        }
        float zPos = Random.Range(500f, 700f);
        GameObject go;
        go = Instantiate(bonusPaper, new Vector3(xPos, 1.5f, zPos + player.transform.position.z), Quaternion.Euler(0, 0, 0)) as GameObject;
        bonus.Add(go);
        zPos = Random.Range(700f, 1000f);
        go = Instantiate(bonusMask, new Vector3(xPos, 1.5f, zPos + player.transform.position.z), Quaternion.Euler(0, 0, 0)) as GameObject;
        bonus.Add(go);
    }
    void DestroyBonus()
    {
        if(paper && mask)
        {
            paper = false;
            mask = false;
            BonusCreate();
        }else if((paper || mask) && ((bonus[0].transform.position.z + 20) < player.transform.position.z))
        {
            paper = false;
            mask = false;
            Destroy(bonus[0]);
            bonus.RemoveAt(0);
            BonusCreate();
        }
        else if(((bonus[0].transform.position.z + 20) < player.transform.position.z) && ((bonus[1].transform.position.z + 20) < player.transform.position.z))
        {
            paper = false;
            mask = false;
            Destroy(bonus[0]);
            bonus.RemoveAt(0);
            Destroy(bonus[0]);
            bonus.RemoveAt(0);
            BonusCreate();
        }
    }
}
