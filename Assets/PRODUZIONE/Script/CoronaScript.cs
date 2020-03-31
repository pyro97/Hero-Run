using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoronaScript : MonoBehaviour
{
    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.AddForce(10 * new Vector3(0.0f, 0.0f, -30.0f));
    }
}
