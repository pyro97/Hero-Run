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

}
