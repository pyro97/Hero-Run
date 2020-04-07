using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    Animator animator;
    private EnemiesGenerator enemies;
    private BonusGenerator bonusGenerator;
    [Range(-3.1f, 3.1f)] float value;
    public float increment;
    bool paper;
    bool mask;
    Vector2 startTouch, endTouch;
    GameObject gun,panelScore;
    float punteggio;
    bool end;
    GameObject countImage;
    public GameObject imageMask;
    public GameObject imagePaper;
    GameObject stelle;
    bool endPolice;
    bool endVirus;
    public Sprite sprite1;
    AudioSource musicaGioco, musicaFine, sourceSparo,sourceVirus,sourceTosse,sourceStarnuto,sourceSparoPolizia,source5Stelle;
    public AudioClip musicaSparo,musicaVirus, musicaTosse, musicaStarnuto, musicaSparoPolizia,musica5Stelle;
    bool endSwipeCentral;
    public int laneNum;
    public float horizVel;
    string contr;
    PlayerPrefsHandler playerPrefs;


    // Start is called before the first frame update

    private void Awake()
    {
        playerPrefs = new PlayerPrefsHandler();
        playerPrefs.SetNumPartiteTotali(playerPrefs.GetNumPartiteTotali() + 1);
        bonusGenerator = GameObject.FindObjectOfType<BonusGenerator>();
        enemies = GameObject.FindObjectOfType<EnemiesGenerator>();
        if (playerPrefs.GetIsMutedEffetti())
        {
            source5Stelle = AddAudio(musica5Stelle, false, false, 0f);
            sourceStarnuto = AddAudio(musicaStarnuto, false, false, 0f);
            sourceSparoPolizia = AddAudio(musicaSparoPolizia, false, false, 0f);
            sourceSparo = AddAudio(musicaSparo, false, false, 0f);
            sourceTosse = AddAudio(musicaTosse, false, false, 0f);
            sourceVirus = AddAudio(musicaVirus, false, false, 0f);

        }
        else
        {
            source5Stelle = AddAudio(musica5Stelle, false, false, 1f);
            sourceStarnuto = AddAudio(musicaStarnuto, false, false, 1f);
            sourceSparoPolizia = AddAudio(musicaSparoPolizia, false, false, 1f);
            sourceSparo = AddAudio(musicaSparo, false, false, 1f);
            sourceTosse = AddAudio(musicaTosse, false, false, 1f);
            sourceVirus = AddAudio(musicaVirus, false, false, 1f);

        }

        if (!playerPrefs.GetIsMutedMusica()) {
            musicaGioco = GameObject.Find("Music").GetComponent<AudioSource>();
            musicaGioco.enabled = true;
            musicaGioco.Play();
        }

        
    }
    void Start()
    {
        
        //GameObject.Find("ImageMask").SetActive(false);
        endPolice = false;
        endVirus = false;
        end = false;
        punteggio = 0;
        mask = false;
        paper = false;
        increment = 0f;
        rigid=GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("Shot", false);
        animator.SetBool("Morto", false);
        animator.SetBool("Tosse", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Countdown", false);
        animator.SetBool("Sparato", false);


        stelle = GameObject.Find("Stars");
        stelle.SetActive(false);
        gun = GameObject.Find("GunMedico");
        gun.SetActive(false);
        Score.animazioneFine = false;

        Score.countdown = true;
        Score.buttonPause = false;
        Score.pause = false;
        countImage = GameObject.Find("CountdownImage");
        countImage.SetActive(false);
        countImage.GetComponent<Animator>().SetBool("Count", false);
        
        
        
        laneNum = 2;
        horizVel = 0;
        contr = "n";

        panelScore = GameObject.Find("PanelScore");



    }

    // Update is called once per frame
    void Update()
    {
        if (!end && !Score.countdown)
        {
            if (Time.timeScale == 1)
            {
                if (!Score.pause)
                {
                    MovePlayer();
                    CalcolaPunteggio();
                    StartCoroutine(ShotPlayer());
                }
                
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouch = Input.GetTouch(0).position;
            }

        }
        else if(end && !Score.countdown)
        {
            StartCoroutine(AnimationPerdente());
        }

        else
        {
            StartCoroutine(CountdownAnimation());
        }

    }

    public void MovePlayer()
    {
        increment += 0.01f;

        if (!playerPrefs.GetIsMutedMusica() && musicaGioco.pitch < 2.0f)
        {
            if (increment % 10 == 0)
            {
                musicaGioco.pitch += 0.1f;
            }
        }

        if (!end)
            rigid.AddForce((30 + increment) * new Vector3(0.0f, 0.0f, 25.0f));
        this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f,0.0f,0.0f);
        if (this.gameObject.transform.position.y != 0.1)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0.1f, this.gameObject.transform.position.z);
        }

        rigid.AddForce((100) * new Vector3(horizVel, 0.0f, 0.0f));
        
        if (endSwipeCentral)
        {
            endSwipeCentral = false;
            this.gameObject.transform.position = new Vector3(0, 0.1f, this.gameObject.transform.position.z);
        }
        if (this.gameObject.transform.position.x < -3.1f)
        {
            this.gameObject.transform.position = new Vector3(-3.1f, 0.1f, this.gameObject.transform.position.z);
        }
        else if (this.gameObject.transform.position.x > 3.1f)
        {
            this.gameObject.transform.position = new Vector3(3.1f, 0.1f, this.gameObject.transform.position.z);
        }
        //transform.position = new Vector3(value, transform.position.y, transform.position.z);

        //transform.position = new Vector3(value, transform.position.y, transform.position.z);

        if (countImage.activeSelf == false)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouch = Input.GetTouch(0).position;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouch = Input.GetTouch(0).position;

                if ((endTouch.x < startTouch.x) && laneNum > 1 && contr=="n")
                {
                    SwipeMovementLeft();
                    /*
                    if (value == -3.1f)
                        return;
                    value -= 3.1f;
                    */

                }
                if ((endTouch.x > startTouch.x) && laneNum < 3 && contr=="n")
                {
                    SwipeMovementRight();
                    /*if (value == 3.1f)
                        return;
                    value += 3.1f;
                    */
                }

            }
            if (contr == "n")
            {
                ControlloPosizione();
            }
            //transform.position = new Vector3(value, transform.position.y, transform.position.z);
        }




    }

    void SwipeMovementLeft()
    {
        horizVel = -3.1f;
        if (this.gameObject.transform.position.x == 3.1f)
        {
            StartCoroutine(StopSlideF3T2());
        }
        else
        {
            StartCoroutine(StopSlideF2T1());
        }
        
        laneNum -= 1;
        contr = "y";
    }

    void SwipeMovementRight()
    {
        horizVel = 3.1f;
        if (this.gameObject.transform.position.x == -3.1f)
        {
            StartCoroutine(StopSlideF1T2());
        }
        else
        {
            StartCoroutine(StopSlideF2T3());
        }
        contr = "y";
        laneNum += 1;
    }

    IEnumerator StopSlideF2T1()
    {
        
        yield return new WaitForSeconds(.5f);
        contr = "n";
        horizVel = 0;
    }

    IEnumerator StopSlideF1T2()
    {
        
        yield return new WaitForSeconds(.5f);
        horizVel = 0;
        contr = "n";
        endSwipeCentral = true;
    }

    IEnumerator StopSlideF2T3()
    {
        yield return new WaitForSeconds(.5f);
        horizVel = 0;
        contr = "n";
    }

    IEnumerator StopSlideF3T2()
    {
        yield return new WaitForSeconds(.5f);
        horizVel = 0;
        endSwipeCentral = true;
        contr = "n";
    }

    IEnumerator ShotPlayer()
    {
        if (Input.touchCount > 0 && countImage.activeSelf == false)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began )
            {
                startTouch = Input.GetTouch(0).position;

            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouch = Input.GetTouch(0).position;
                if (startTouch == endTouch)
                {
                    contr = "y";
                    sourceSparo.enabled = true;
                    sourceSparo.Play();
                    ControlloPosizione();
                    animator.SetBool("Shot", true);
                    yield return new WaitForSeconds(0.3f);
                    ControlloPosizione();
                    gun.SetActive(true);
                    yield return new WaitForSeconds(0.3f);
                    animator.SetBool("Shot", false);
                    ControlloPosizione();
                    yield return new WaitForSeconds(0.1f);
                    gun.SetActive(false);
                    ControlloPosizione();
                    contr = "n";
                    sourceSparo.Stop();
                    sourceSparo.enabled = false;

                }
            }
        }
    }

    public void CalcolaPunteggio()
    {
         
            punteggio = punteggio + 0.5f ;
            Score.punteggio = (int)punteggio;
            Text t = GameObject.Find("Punti").GetComponent<Text>();
            t.text = "" + Score.punteggio;
        
    }
    
    IEnumerator AnimationPerdente()
    {
        Score.buttonPause = false;
        stelle.SetActive(false);
        panelScore.SetActive(false);
        imageMask.SetActive(false);
        imagePaper.SetActive(false);

        if (endVirus && !Score.animazioneFine)
        {

            sourceVirus.enabled = true;

            sourceVirus.Play();



            animator.SetBool("Shot", false);
            gun.SetActive(false);
            Score.buttonPause = false;
            animator.SetBool("Walk", true);

            yield return new WaitForSeconds(0.2f);
            GameObject cam = GameObject.Find("Main Camera");
            cam.transform.rotation = Quaternion.Euler(17.5f, 180, 0);
            cam.transform.position = new Vector3(this.transform.position.x, 4f, this.transform.position.z + 5f);
            Score.animazioneFine = true;
            this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            sourceVirus.Stop();
            sourceVirus.enabled = false;

            sourceTosse.enabled = true;
            sourceTosse.Play();

            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0.1f, this.gameObject.transform.position.z);
            animator.SetBool("Tosse", true);
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0.1f, this.gameObject.transform.position.z);


            yield return new WaitForSeconds(0.8f);

            animator.SetBool("Morto", true);


            sourceTosse.Stop();
            sourceTosse.enabled = false;


            yield return new WaitForSeconds(1.2f);
            Score.fine = true;


        }else if(endPolice && !Score.animazioneFine)
        {
            sourceVirus.enabled = true;


            animator.SetBool("Shot", false);
            gun.SetActive(false);
            Score.buttonPause = false;
            animator.SetBool("Sparato", true);

            yield return new WaitForSeconds(0.2f);
            GameObject cam = GameObject.Find("Main Camera");
            cam.transform.rotation = Quaternion.Euler(17.5f, 180, 0);
            cam.transform.position = new Vector3(this.transform.position.x, 4f, this.transform.position.z + 10f);
            Score.animazioneFine = true;
            this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            sourceSparoPolizia.enabled = true;
            sourceSparoPolizia.Play();

            yield return new WaitForSeconds(0.8f);

            sourceSparoPolizia.Stop();
            sourceSparoPolizia.enabled = false;


            yield return new WaitForSeconds(0.1f);
            Score.fine = true;
        }
  


    }

    IEnumerator CountdownAnimation()
    {

        countImage.GetComponent<Image>().sprite = sprite1;
        countImage.SetActive(true);
        countImage.GetComponent<Animator>().SetBool("Count", true);
        Score.countdown = false;

        yield return new WaitForSeconds(4f);

        countImage.SetActive(false);
        Score.buttonPause=true;
        Score.pause = false;
        countImage.GetComponent<Animator>().SetBool("Count", false);


    }



    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetType() == typeof(SphereCollider))
        {
            if (collision.gameObject.tag.Equals("CoronaVirus") || (collision.gameObject.tag.Equals("Person")))
            {
                if (!mask)
                {
                    AvviaMusicaFinale();
                    Destroy(collision.gameObject);
                    enemies.enemies.Remove(collision.gameObject);
                    end = true;
                    endVirus = true;

                }
                else
                {

                    mask = false;
                    imageMask.GetComponent<Image>().color = new Color32(255, 235, 235, 80);
                    Destroy(collision.gameObject);
                    enemies.enemies.Remove(collision.gameObject);

                }
            }

            if (collision.gameObject.tag.Equals("Police"))
            {
                if (!paper && stelle.activeSelf == true)
                {
                    AvviaMusicaFinale();
                    endPolice = true;
                    end = true;
                    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0.1f, this.gameObject.transform.position.z - 5);
                    this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

                }
                else if (!paper && stelle.activeSelf == false)
                {
                    source5Stelle.enabled = true;
                    source5Stelle.Play();
                    stelle.SetActive(true);
                    Destroy(collision.gameObject);
                    enemies.enemies.Remove(collision.gameObject);
                }
                else if (paper && stelle.activeSelf == false)
                {
                    paper = false;
                    imagePaper.GetComponent<Image>().color = new Color32(255, 235, 235, 80);
                    Destroy(collision.gameObject);
                    enemies.enemies.Remove(collision.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Paper"))
        {
            Destroy(other.gameObject);
            bonusGenerator.bonus.Remove(other.gameObject);
            bonusGenerator.paper = true;
            if (stelle.activeSelf == true)
            {
                stelle.SetActive(false);
            }
            else
            {
                paper = true;
                imagePaper.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
        if (other.gameObject.tag.Equals("Mask"))
        {
            Destroy(other.gameObject);
            bonusGenerator.bonus.Remove(other.gameObject);
            bonusGenerator.mask = true;
            mask = true;
            imageMask.GetComponent<Image>().color = new Color32(255, 235, 235, 255);
        }
    }

    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol){
      AudioSource newAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        newAudio.clip = clip; 
         newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.enabled = false;
         newAudio.volume = vol; 
        return newAudio; 
 }


    public void AvviaMusicaFinale()
    {
        if (!playerPrefs.GetIsMutedMusica())
        {
            GameObject.Find("Music").GetComponent<AudioSource>().Stop();
            GameObject.Find("Music").GetComponent<AudioSource>().enabled = false;

            GameObject.Find("FinalMusic").GetComponent<AudioSource>().enabled = true;
            GameObject.Find("FinalMusic").GetComponent<AudioSource>().Play();
        }
    
    }


    public void ControlloPosizione()
    {
        if (laneNum == 2)
        {           
            this.gameObject.transform.position = new Vector3(0, 0.1f, this.gameObject.transform.position.z);
        }
        if (laneNum == 1)
        {
            this.gameObject.transform.position = new Vector3(-3.1f, 0.1f, this.gameObject.transform.position.z);
        }
        if (laneNum == 3)
        {
            this.gameObject.transform.position = new Vector3(3.1f, 0.1f, this.gameObject.transform.position.z);
        }
    }

}
