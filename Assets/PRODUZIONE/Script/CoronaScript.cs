using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoronaScript : MonoBehaviour
{
    Rigidbody rigid;
    bool stoprun;
    // Start is called before the first frame update
    void Start()
    {
        stoprun = false;
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!stoprun && Score.buttonPause)
            this.rigid.AddForce(100 * new Vector3(0.0f, 0.0f, -30.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            if (other.tag == "Mask" || other.tag == "Paper")
            {
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.gameObject.transform.position.z + 5);
            }
            else if (this.transform.position.z > other.transform.position.z)
            {
                stoprun=true;
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.gameObject.transform.position.z + 10);
            }
            else
            {
                other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, this.gameObject.transform.position.z + 10);
            }
        }
    }

    
}
