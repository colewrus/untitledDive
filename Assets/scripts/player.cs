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
    int bubbleCountMax;
    float bubbleHpMax;
    public float bubbleHpCurrent;
    public float airLossRate;
    bool drowning;

    public float speed;
    float startSpeed;
    public List<Transform> particleSpots = new List<Transform>();
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
        bubbleCountMax = 1;
        bubbleCount = 1;
        drowning = false;
	}
	
	// Update is called once per frame
	void Update () {
        EnterWater();
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

    void EnterWater()
    {
        if (!inWater)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //play jump animation
                ani.SetBool("goJump", true);                
                StartCoroutine(JumpDelay(1.2f));
            
            }
        }
    }


    void Swim()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        if (horiz != 0 || vert != 0)
        {
            rb.AddForce(new Vector2(horiz, vert)*speed);
            ani.SetBool("swimIdle", false);    
        }

        if (transform.position.y >= 0.5f) //for when you surface to refill air
        {            
            refreshAir = true;
            bubbleCount = bubbleCountMax;
            bubbleHpCurrent = bubbleHpMax;
            ani.SetBool("swimIdle", true);
            //resize the bubbles
            for(int i = 0; i < Manager_UI.instance.BubbleSprites.Count; i++)
            {
                Manager_UI.instance.BubbleSprites[i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            rb.velocity = new Vector2(0, 0);
        }

        if (horiz > 0)
        {
            ani.SetBool("swimHoriz", true);
            sr.flipX = false;
            sr.flipY = false;
            gameObject.transform.GetChild(1).transform.position = particleSpots[0].position;
        }
        else if( horiz < 0)
        {
            ani.SetBool("swimHoriz", true);
            sr.flipX = true;
            sr.flipY = false;
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
        if (inWater)
        {


            //this is for that first jump in
            if(rb.gravityScale > 0 && transform.position.y < 0 && ani.GetBool("goJump"))
            {
                
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, 0);
                //using the animation start boolean to gate this first jump in
                ani.SetBool("goJump", false);
                
            }
            if (!ani.GetBool("goJump"))//have you started?
            {
                if (!refreshAir) //have you surfaced to refill oxygen?
                {
                    Swim();
                    
                }
                else
                {
                    if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        //add force, go down
                        rb.AddForce(Vector2.down, ForceMode2D.Impulse);
                        refreshAir = false;
                    }
                }
                
            }
            
        }
    }

    IEnumerator JumpDelay(float t)
    {
        yield return new WaitForSeconds(t);
        rb.AddForce(((Vector2.up * 2f) + Vector2.left*1.1f), ForceMode2D.Impulse);
        
        inWater = true;
        for (int i = 0; i < bubbleCount; i++)
        {
            Manager_UI.instance.BubbleSprites[i].SetActive(true);
            Debug.Log("bubb" + bubbleCount);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "enemy")
        {
            collision.gameObject.GetComponent<enemy>().playerContact = true;
            Debug.Log(collision.gameObject.GetComponent<enemy>().myEnemy);
        }

        if(collision.tag == "slow")
        {
            speed = 0.3f;
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
