using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persona : MonoBehaviour
{
    // Start is called before the first frame update
    private EnemiesGenerator enemies;

    void Start()
    {
        enemies = GameObject.FindObjectOfType<EnemiesGenerator>();

    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag.Equals("Fire"))
        {
            Destroy(this.gameObject);
            enemies.enemies.Remove(this.gameObject);
            enemies.mortaPersona = true;
        }
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
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, other.gameObject.transform.position.z + 5);
            }
            else
            {
                other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, this.gameObject.transform.position.z + 5);
            }
        }
        
    }

}
