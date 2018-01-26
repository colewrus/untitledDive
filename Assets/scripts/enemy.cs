using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

    public enum EnemyType { puffer, barracuda, pirate };

    public EnemyType myEnemy;

    Rigidbody2D rb;
    Animator ani;

    //puffer vars
    Vector3 startPos;
    public Vector3 destPos;
    public bool pMove;
    public float timer;
    public float currentTime;
    public float pufferSpeed;
    public bool puffUp;
    SpriteRenderer p_sr;
    public float puffMaxChance;
    float initMax;
    public bool playerContact;

     // Use this for initialization
	void Start () {
        startPos = this.transform.position;
        pMove = true;
        rb = this.GetComponent<Rigidbody2D>();
        ani = this.GetComponent<Animator>();
        currentTime = 0;

        puffUp = false;
        p_sr = this.GetComponent<SpriteRenderer>();
        initMax = puffMaxChance;
        playerContact = false;

	}
	
	// Update is called once per frame
	void Update () {
        Puffer();

        if (Input.GetKeyDown(KeyCode.P))
        {
            puffUp = true;
        }
	}



    void Puffer()
    {

        if (!puffUp)
        {

            if (playerContact)
            {
                Debug.Log("Contact: " + playerContact);
                puffUp = true;
                playerContact = false;
            }

            if (currentTime < timer)
            {
                currentTime += 1 * Time.deltaTime;
            }
            else
            {
                pMove = true;
                currentTime = 0;
            }
            //pick a point to the left or right x unitys apart
            if (pMove)
            {
                float m;
                GameObject p = GameObject.Find("Player");
                if (this.transform.position.x < p.transform.position.x)
                {
                    m = 3;
                    p_sr.flipX = true;
                }
                else
                {
                    m = -3;
                    p_sr.flipX = false;
                }
                //will need a way to make sure y point doesn't send the fish above water, math clamp
                destPos = new Vector3(this.transform.position.x + m, Mathf.Clamp(this.transform.position.y + Random.Range(-1, 1),-6.5f,-1), startPos.z);
                if (destPos.x <= -9) {
                    destPos.x += 3;
                }
                float puffRoll = Random.Range(0, 100);
               
                if(puffRoll >= puffMaxChance)
                {
                    puffUp = true;
                    puffMaxChance += 10;
                }else
                {
                    puffMaxChance = initMax;
                }                 
                             
                pMove = false;
            }


                
            transform.position = Vector3.MoveTowards(transform.position, destPos, pufferSpeed * Time.deltaTime);
        }
        else
        {

            ani.SetBool("puff", true);
            StartCoroutine(PuffDelay(2.5f));
            destPos = transform.position;
            this.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "slow")
        {
            pMove = false;
            pMove = true;
        }
    }

    IEnumerator PuffDelay(float t)
    {
        yield return new WaitForSeconds(t);
        currentTime = 6;
        ani.SetBool("puff", false);
        puffUp = false;
        this.GetComponent<CircleCollider2D>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = true;
    }
}
