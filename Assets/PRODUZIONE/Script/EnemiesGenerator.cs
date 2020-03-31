using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{

    public GameObject theEnemy;
    public GameObject person;
    public List<GameObject> enemies;
    public GameObject player;
    float xPos;
    float xPosPerson;
    int zPos;
    float count;
    int zPosPerson;
    int enemyCount;
    float startWait;

    
    // Start is called before the first frame update
    void Start()
    {
        startWait = 3f;
        enemies = new List<GameObject>();
        StartCoroutine(EnemyDrop());

    }

    IEnumerator EnemyDrop()
    {

        while (enemyCount < 7)
        {
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            xPos = Random.Range(-4.1f, 5.1f);
            count = Random.Range(0.0f, 2.0f);
            if (xPos < -1)
            {
                xPos = -3;
                if (count < 1.01)
                    xPosPerson = 3;
                else
                    xPosPerson = 0;
            }
            else if (xPos > 1)
            {
                xPos = 3;
                if (count < 1.01)
                    xPosPerson = 0;
                else
                    xPosPerson = -3;
            }
            else
            {
                xPos = 0;
                if (count < 1.01)
                    xPosPerson = -3;
                else
                    xPosPerson = 3;
            }
            zPos = Random.Range(80, 100);
            zPosPerson = Random.Range(90, 110);
            GameObject go;
            go = Instantiate(theEnemy, new Vector3(xPos, 0.1f, zPos + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
            enemies.Add(go);
            go = Instantiate(person, new Vector3(xPosPerson, 0.1f, zPosPerson + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
            enemies.Add(go);
            yield return new WaitForSecondsRealtime(startWait);
            enemyCount += 2;
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
            if (enemyCount>0 && ((enemies[0].transform.position.z + 20) < player.transform.position.z) && ((enemies[1].transform.position.z + 20) < player.transform.position.z))
            {
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
        xPos = Random.Range(-4.1f, 5.1f);
        count = Random.Range(0.0f, 2.0f);
        if (xPos < -1)
        {
            xPos = -3;
            if (count < 1.01)
                xPosPerson = 3;
            else
                xPosPerson = 0;
        }
        else if (xPos > 1)
        {
            xPos = 3;
            if (count < 1.01)
                xPosPerson = 0;
            else
                xPosPerson = -3;
        }
        else
        {
            xPos = 0;
            if (count < 1.01)
                xPosPerson = -3;
            else
                xPosPerson = 3;
        }
        zPos = Random.Range(80, 100);
        zPosPerson = Random.Range(90, 110);
        GameObject go;
        go = Instantiate(theEnemy, new Vector3(xPos, 0.1f, zPos + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
        enemies.Add(go);
        go = Instantiate(person, new Vector3(xPosPerson, 0.1f, zPosPerson + player.transform.position.z), Quaternion.Euler(0, -180f, 0)) as GameObject;
        enemies.Add(go);
        enemyCount += 2;
    }
}
