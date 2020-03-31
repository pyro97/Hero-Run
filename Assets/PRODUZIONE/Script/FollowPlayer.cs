using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    Vector3 distanza;
    // Start is called before the first frame update
    void Start()
    {
        distanza = player.transform.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        follow();
    }


    public void follow()
    {
        this.transform.position = player.transform.position - distanza;
    }
}
