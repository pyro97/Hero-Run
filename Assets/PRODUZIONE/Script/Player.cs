using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    Animator jump;
    private EnemiesGenerator enemies;
    bool salto,sparo;
    [Range(-3.1f, 3.1f)] float value;
    public float increment;
    bool paper;
    bool mask;
    Vector2 startTouch, endTouch;
    GameObject gun;
    float punteggio;
    bool end;


    // Start is called before the first frame update

    private void Awake()
    {
        enemies = GameObject.FindObjectOfType<EnemiesGenerator>();
    }
    void Start()
    {

        end = false;
        punteggio = 0;
        mask = false;
        paper = false;
        increment = 0f;
        salto = false;
        sparo = false;
        rigid=GetComponent<Rigidbody>();
        jump = gameObject.GetComponent<Animator>();
        jump.SetBool("Salto", salto);
        jump.SetBool("Shot", sparo);
        jump.SetBool("Morto", false);
        jump.SetBool("Cade", false);

        gun = GameObject.Find("GunMedico");
        gun.SetActive(false);
        Score.animazioneFine = false;



    }

    // Update is called once per frame
    void Update()
    {
        if (!end)
        {
            movePlayer();
            calcolaPunteggio();
            StartCoroutine(shotPlayer());
        }
        else
        {
            StartCoroutine(animationPerdente());
        }


       
        

        
    }

    public void movePlayer()
    {
        //muove avanti in automatico
        //rigid.AddForce(10 * new Vector3(0.0f, 0.0f, 25.0f));

        
        increment += 0.01f;
        //increment uguale a 50 potrebbe bastare perchè se no si andrebbe a distruggere la dinamica del gioco 
        rigid.AddForce((10 + increment) * new Vector3(0.0f, 0.0f, 25.0f));

        this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f,0.0f,0.0f);

        if (this.gameObject.transform.position.y < 0.1)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,0.1f, this.gameObject.transform.position.z);
        }

            transform.position = new Vector3(value, transform.position.y, transform.position.z);

            if(Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouch = Input.GetTouch(0).position;
            }

        if (Time.timeScale == 1)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouch = Input.GetTouch(0).position;

                if ((endTouch.x < startTouch.x) && transform.position.x > -3.1f)
                {
                    if (value == -3.1f)
                        return;
                    value -= 3.1f;

                }
                if ((endTouch.x > startTouch.x) && transform.position.x < 3.1f)
                {
                    if (value == 3.1f)
                        return;
                    value += 3.1f;
                }

            }
        }
          

    }

    IEnumerator shotPlayer()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouch = Input.GetTouch(0).position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouch = Input.GetTouch(0).position;
                if (startTouch == endTouch)
                {
                    sparo = true;
                    jump.SetBool("Shot", sparo);
                    yield return new WaitForSeconds(8 * Time.deltaTime);
                    gun.SetActive(true);
                    yield return new WaitForSeconds(6* Time.deltaTime);
                    sparo = false;
                    jump.SetBool("Shot", sparo);
                    yield return new WaitForSeconds(5 * Time.deltaTime);
                    gun.SetActive(false);
                }
            }
            }
    }

    public void calcolaPunteggio()
    {
        if (Time.timeScale == 1)
        {   
            punteggio = punteggio + 0.5f + increment;
            Score.punteggio = (int)punteggio;
            Text t = GameObject.Find("Punti").GetComponent<Text>();
            t.text = "" + Score.punteggio;
        }
    }
    
    IEnumerator animationPerdente()
    {
        GameObject cam = GameObject.Find("Main Camera");
        cam.transform.rotation = Quaternion.Euler(17.5f, 180, 0);
        cam.transform.position = new Vector3(this.transform.position.x, 2.5f, this.transform.position.z+5f);
        Score.animazioneFine = true;
        this.gameObject.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        rigid.AddForce(5*new Vector3(0.0f, 0.0f, 25.0f));
       jump.SetBool("Morto", true);
        yield return new WaitForSeconds(5 * Time.deltaTime);
        jump.SetBool("Cade", true);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        yield return new WaitForSeconds(50 * Time.deltaTime);
        Score.fine = true;


    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag.Equals("CoronaVirus"))
        {
            if (!mask)
            {
                //Score.fine = true;
                Destroy(collision.gameObject);
                enemies.enemies.Remove(collision.gameObject);
                end = true;

            }
            else
            {
                mask = false;
                Destroy(collision.gameObject);
                enemies.enemies.Remove(collision.gameObject);
            }
            

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Paper"))
        {
            paper = true;
        }
        if (other.gameObject.tag.Equals("Mask"))
        {
            mask = true;
        }
    }







}
