using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    public bool mortaPersona;
    public GameObject theEnemy;
    public GameObject personCheBalla;
    //public GameObject uomo;
    public List<GameObject> enemies;
    GameObject player;
    public GameObject police;
    float xPosAlien;
    float xPosPerson;
    float xPosPoliceMan;
    int zPosAlien;
    float count;
    int zPosPerson;
    int zPosPolice;
    int enemyCount;
    float startWait;
    PlayerPrefsHandler playerPrefs;


    void Awake()
    {
        playerPrefs = new PlayerPrefsHandler();
        player = GameObject.Find(playerPrefs.GetPersonaggioAttuale());
    }

    // Start is called before the first frame update
    void Start()
    {
        startWait = 4f;
        mortaPersona = false;
        enemies = new List<GameObject>();
        StartCoroutine(EnemyDrop());

    }

    IEnumerator EnemyDrop()
    {

        while (enemyCount < 7 )
        {
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            xPosAlien = Random.Range(-4.1f, 5.1f);
            count = Random.Range(0.0f, 2.0f);
            if (xPosAlien < -1)
            {
                xPosAlien = -3;
                if (count < 1.01)
                {
                    xPosPerson = 3;
                    xPosPoliceMan = 0;
                }
                else
                {
                    xPosPerson = 0;
                    xPosPoliceMan = 3;
                }
            }
            else if (xPosAlien > 1)
            {
                xPosAlien = 3;
                if (count < 1.01)
                {
                    xPosPerson = 0;
                    xPosPoliceMan = -3;
                }
                else
                {
                    xPosPerson = -3;
                    xPosPoliceMan = 0;
                }
            }
            else
            {
                xPosAlien = -3;
                xPosPerson = 0;
                xPosPoliceMan = 3;
            }
            zPosAlien = Random.Range(80, 90);
            zPosPerson = Random.Range(90, 100);
            zPosPolice = Random.Range(110, 120);
            GameObject go;
            go = Instantiate(theEnemy, new Vector3(xPosAlien, 0.1f, zPosAlien + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
            enemies.Add(go);
            /*UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            int i= Random.Range(1, 3);
            if (i == 1)
            {
                go = Instantiate(uomo, new Vector3(xPosPerson, 0.1f, zPosPerson + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;

            }
            else if(i == 2)
            {*/
                go = Instantiate(personCheBalla, new Vector3(xPosPerson, 0.1f, zPosPerson + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
            //}
           
            //go = Instantiate(personCheBalla, new Vector3(xPosPerson, 0.1f, zPosPerson + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
            enemies.Add(go);
            go = Instantiate(police, new Vector3(xPosPoliceMan, 0.1f, zPosPolice + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
            enemies.Add(go);
            yield return new WaitForSecondsRealtime(startWait);
            enemyCount += 3;
        }

    }

    // Update is called once per frame
    void Update()
    {
        DestroyEnemies();
    }

    void DestroyEnemies()
    {
        bool flag = true;
        while (flag)
        {
            /*if (mortaPersona && ((enemies[0].transform.position.z + 20) < player.transform.position.z) && ((enemies[1].transform.position.z + 20) < player.transform.position.z))
            {
                Destroy(enemies[0]);
                enemies.RemoveAt(0);
                Destroy(enemies[0]);
                enemies.RemoveAt(0);
                CreateEnemies();
                mortaPersona = false;
            }*/
            if (enemyCount>0 && ((enemies[0].transform.position.z + 20) < player.transform.position.z) && ((enemies[1].transform.position.z + 20) < player.transform.position.z)
                &&  ((enemies[2].transform.position.z + 20) < player.transform.position.z))
            {
                Destroy(enemies[0]);
                enemies.RemoveAt(0);
                Destroy(enemies[0]);
                enemies.RemoveAt(0);
                Destroy(enemies[0]);
                enemies.RemoveAt(0);
                CreateEnemies();

            }
            else
                flag = false;
        }

    }

    void CreateEnemies()
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        xPosAlien = Random.Range(-4.1f, 5.1f);
        count = Random.Range(0.0f, 2.0f);
        if (xPosAlien < -1)
        {
            xPosAlien = -3;
            if (count < 1.01)
            {
                xPosPerson = 3;
                xPosPoliceMan = 0;
            }
            else
            {
                xPosPerson = 0;
                xPosPoliceMan = 3;
            }
        }
        else if (xPosAlien > 1)
        {
            xPosAlien = 3;
            if (count < 1.01)
            {
                xPosPerson = 0;
                xPosPoliceMan = -3;
            }
            else
            {
                xPosPerson = -3;
                xPosPoliceMan = 0;
            }
        }
        else
        {
            xPosAlien = 0;
            if (count < 1.01)
            {
                xPosPerson = -3;
                xPosPoliceMan = 3;
            }
            else
            {
                xPosPerson = 3;
                xPosPoliceMan = -3;
            }
        }
        zPosAlien = Random.Range(80, 90);
        zPosPerson = Random.Range(90, 100);
        zPosPolice = Random.Range(100, 110);
        GameObject go;
        go = Instantiate(theEnemy, new Vector3(xPosAlien, 0.1f, zPosAlien + player.transform.position.z), Quaternion.Euler(0, 0, 0)) as GameObject;
        enemies.Add(go);
        /*UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        int i = Random.Range(1, 3);
        if (i == 1)
        {
            go = Instantiate(uomo, new Vector3(xPosPerson, 0.1f, zPosPerson + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;

        }
        else if (i == 2)
        {*/
            go = Instantiate(personCheBalla, new Vector3(xPosPerson, 0.1f, zPosPerson + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
        //}
        
        //go = Instantiate(personCheBalla, new Vector3(xPosPerson, 0.1f, zPosPerson + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
        enemies.Add(go);
        go = Instantiate(police, new Vector3(xPosPoliceMan, 0.1f, zPosPolice + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
        enemies.Add(go);
        enemyCount += 3;
    }

}
