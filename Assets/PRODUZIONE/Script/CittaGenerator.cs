using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CittaGenerator : MonoBehaviour
{
    private float spawnz = -6f;
    public GameObject[] prefabs;
    private float RoadLenght = 96f;
    private int amountOfRoads = 3;
    private List<GameObject> roadList;
    private Transform playertransform;
    // Start is called before the first frame update
    void Start()
    {
        roadList = new List<GameObject>();
        playertransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(playertransform.position.z > (spawnz - amountOfRoads * RoadLenght))
        {
            SpawnRoad(0);
            DeleteRoad();

        }
    }

    void SpawnRoad(int prefabIndex)
    {
        GameObject go;
        go = Instantiate(prefabs[prefabIndex]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnz;
        spawnz += RoadLenght;

        roadList.Add(go);
    }

    void DeleteRoad()
    {
        if(roadList.Count > 5)
        {
            Destroy(roadList[0]);
            roadList.RemoveAt(0);
        }
        
    }
}
