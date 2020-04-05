using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persona : MonoBehaviour
{
    // Start is called before the first frame update
    private EnemiesGenerator enemies;
    AudioSource sourceRogo;
    public AudioClip musicaRogo;


    private void Awake()
    {
        sourceRogo = AddAudio(musicaRogo, false, false, 1f);
    }
    void Start()
    {   
        sourceRogo = AddAudio(musicaRogo, false, false, 1f);
        enemies = GameObject.FindObjectOfType<EnemiesGenerator>();
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag.Equals("Fire"))
        {
            StartCoroutine(EffettoRogo());
            Destroy(this.gameObject);
            enemies.enemies.Remove(this.gameObject);
            enemies.mortaPersona = true;
        }
    }

    IEnumerator EffettoRogo()
    {
        sourceRogo.enabled = true;
        sourceRogo.Play();
        yield return new WaitForSeconds(3.1f);
        sourceRogo.Stop();
        sourceRogo.enabled = false;
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

    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.enabled = false;
        newAudio.volume = vol;
        return newAudio;
    }

}
