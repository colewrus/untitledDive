using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {


    Animator ani;
    public bool inWater;
    Rigidbody2D rb;
    SpriteRenderer sr;
    public bool refreshAir;

    //Air Handlers
    int bubbleCount;
    public int bubbleCountMax;
    float bubbleHpMax;
    public float bubbleHpCurrent;
    public float airLossRate;
    bool drowning;

    public float speed;
    float startSpeed;
    public List<Transform> particleSpots = new List<Transform>();
    public float cameraRubberBandSpot;
    float horiz;
    float vert;
    //Health Vars
    int health;
    bool invulnerable;

    //Money handler
    public int coins;
        // 0 is default position, 1 is the diving position

	// Use this for initialization
	void Start () {
        ani = this.GetComponent<Animator>();
        inWater = false;
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        startSpeed = speed;
        refreshAir = false;
        bubbleHpMax = bubbleHpCurrent;

        bubbleCount = bubbleCountMax;
        health = Manager_UI.instance.PlayerHealth.Count;
        coins = 0;

        invulnerable = false;
	}
	
	// Update is called once per frame
	void Update () {
        //EnterWater();
        WaterCheck();
        Bubbles();
	}


    void Bubbles()
    {
        if (inWater && !refreshAir)
        {
            

            if(bubbleCount > 0)
            {
                if(bubbleHpCurrent > 0)
                {
                    bubbleHpCurrent -= airLossRate * Time.deltaTime;
                    Manager_UI.instance.BubbleSprites[bubbleCount-1].GetComponent<RectTransform>().localScale = new Vector3((bubbleHpCurrent / bubbleHpMax), (bubbleHpCurrent / bubbleHpMax), 1);
                    
                }
                else
                {
                    bubbleCount -= 1;                
                    bubbleHpCurrent = bubbleHpMax;
                    Debug.Log(bubbleCount);
                }
            }
            else
            {
                if (bubbleHpCurrent > 0)
                {
                    bubbleHpCurrent -= airLossRate * Time.deltaTime;
                }
                else
                {
                    Debug.Log("damage");//take some health damage
                    bubbleHpCurrent = bubbleHpMax;
                }
                
            }
        }
    }

    IEnumerator NooxyDamage(float t)
    {
        yield return new WaitForSeconds(t);
        Debug.Log("hurt");
    }


    void Swim()
    {
        horiz = Input.GetAxis("Horizontal");
        
        vert = Input.GetAxis("Vertical");

        if (horiz != 0 || vert != 0)
        {
            if (!refreshAir)
            {
                rb.AddForce(new Vector2(horiz, vert) * speed);
            }else
            {
                rb.AddForce(new Vector2(horiz, Mathf.Clamp(vert, -1, 0)) * speed);
            }
            
            ani.SetBool("swimIdle", false);    
        }


        if (horiz > 0 && vert <0.5f && vert > -0.5f)
        {
            ani.SetBool("swimHoriz", true);
            sr.flipX = false;            
            gameObject.transform.GetChild(1).transform.position = particleSpots[0].position;
        }
        else if( horiz < 0 && vert < 0.5f && vert > -0.5f)
        {
            ani.SetBool("swimHoriz", true);
            sr.flipX = true;            
            gameObject.transform.GetChild(1).transform.position = particleSpots[0].position;
        }
        else if(vert > 0.5f)
        {
            ani.SetBool("swimHoriz", false);
            ani.SetBool("swimVert", true);            
            gameObject.transform.GetChild(1).transform.position = particleSpots[0].position;
        }
        else if(vert < -0.5f)
        {
            ani.SetBool("swimVert", true);
            StartCoroutine(PlayerYdelay());
            gameObject.transform.GetChild(1).transform.position = particleSpots[1].position;
            //sr.flipY = true;
        }else if(vert == 0 && horiz == 0)//Idle
        {
            gameObject.transform.GetChild(1).transform.position = particleSpots[0].position;
            ani.SetBool("swimIdle", true);
            ani.SetBool("swimHoriz", false);
            ani.SetBool("swimVert", false);
            sr.flipX = false;
            sr.flipY = false;
        }

        if (vert < 0.5f && vert > -0.5f)
        {
            ani.SetBool("swimVert", false);
            sr.flipY = false;
        }
       
    }

    IEnumerator PlayerYdelay()
    {
        yield return new WaitForEndOfFrame();
        sr.flipY = true;
    }

    void WaterCheck()
    {
        //this is for that first jump in
        if (rb.gravityScale > 0 && transform.position.y < 0)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, 0);
            inWater = true;
            for (int i = 0; i < bubbleCount; i++)
            {
                Manager_UI.instance.BubbleSprites[i].SetActive(true);                
            }
        }


        if (inWater)
        { 
            if (!ani.GetBool("goJump"))//have you started?
            {
                
                
                if (!refreshAir) //have you surfaced to refill oxygen?
                {
                    Swim();                    
                }
                else
                {
                    Swim();
                    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                       
                        //add force, go down
                        rb.AddForce(Vector2.down, ForceMode2D.Impulse);
                        refreshAir = false;
                        Manager_UI.instance.resurfaceText.SetActive(false);
                    }
                }
                
            }
            
        }
    }


    IEnumerator InvulDelay(float t)
    {
        yield return new WaitForSeconds(t);
        invulnerable = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "enemy")
        {
            if (collision.gameObject.GetComponent<enemy>())
                collision.gameObject.GetComponent<enemy>().playerContact = true;
            if (!invulnerable)
            {
                Manager_UI.instance.PlayerHealth[health - 1].SetActive(false);
                health--;
                invulnerable = true;
                Debug.Log(invulnerable);
                StartCoroutine(InvulDelay(1.1f));
            }
         
        }
        if(collision.tag == "water")
        {
            if (!refreshAir)
                rb.velocity = new Vector2(0, 0);
            Debug.Log("water hit");
            refreshAir = true;
            Manager_UI.instance.resurfaceText.SetActive(true);
            bubbleCount = bubbleCountMax;
            bubbleHpCurrent = bubbleHpMax;
            //ani.SetBool("swimIdle", true);
           // horiz = 0;
            vert = 0;
            //resize the bubbles
            for (int i = 0; i < Manager_UI.instance.BubbleSprites.Count; i++)
            {
                Manager_UI.instance.BubbleSprites[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            
        }

        if(collision.tag == "slow")
        {
            speed = 0.5f;
        }

        if(collision.tag == "Coin")
        {
            
            coins++;
            collision.gameObject.SetActive(false);
            Manager_UI.instance.AddCoin();
        }

        if(collision.tag == "tank")
        {
            
            bubbleCountMax++;
            bubbleCount = bubbleCountMax;
            bubbleHpCurrent = bubbleHpMax;
            for (int i = 0; i < bubbleCount; i++)
            {
                Manager_UI.instance.BubbleSprites[i].SetActive(true);
                Manager_UI.instance.BubbleSprites[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "slow")
        {
            speed = startSpeed;
        }
    }


}
