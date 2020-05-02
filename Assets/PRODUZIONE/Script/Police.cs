using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            if (other.tag == "Mask" || other.tag == "Paper")
            {
                this.gameObject.SetActive(false);
                //this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.gameObject.transform.position.z - 150);
            }
            else if (this.transform.position.z > other.transform.position.z)
            {
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.gameObject.transform.position.z + 12);
            }
            else
            {
               // other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, this.gameObject.transform.position.z + 12);
            }
        }

    }
}
