using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {


    Animator ani;
    public bool inWater;
    Rigidbody2D rb;
    SpriteRenderer sr;

    public List<Transform> particleSpots = new List<Transform>();
        // 0 is default position, 1 is the diving position

	// Use this for initialization
	void Start () {
        ani = this.GetComponent<Animator>();
        inWater = false;
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        EnterWater();
        WaterCheck();
	}


    void EnterWater()
    {
        if (!inWater)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("hit space " + ani.GetBool("goJump")); 
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
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            rb.AddForce(new Vector2(horiz, vert));
            ani.SetBool("swimIdle", false);    
        }
    
        if(horiz > 0)
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
            ani.SetBool("swimVert", true);
            ani.SetBool("swimHoriz", false);
            gameObject.transform.GetChild(1).transform.position = particleSpots[0].position;
        }
        else if(vert < -0.5f)
        {
            ani.SetBool("swimVert", true);
            StartCoroutine(PlayerYdelay());
            gameObject.transform.GetChild(1).transform.position = particleSpots[1].position;
            //sr.flipY = true;
        }else if(vert == 0 && horiz == 0)
        {
            gameObject.transform.GetChild(1).transform.position = particleSpots[0].position;
            ani.SetBool("swimIdle", true);
            ani.SetBool("swimHoriz", false);
            ani.SetBool("swimVert", false);
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
                Debug.Log(ani.GetBool("goJump"));
            }
            if (!ani.GetBool("goJump"))
            {
                Swim();
            }
            
        }
    }

    IEnumerator JumpDelay(float t)
    {
        yield return new WaitForSeconds(t);
        rb.AddForce(((Vector2.up * 2f) + Vector2.left*1.1f), ForceMode2D.Impulse);
        inWater = true;
        
    }
    void WaterIdle()
    {
        if (inWater)
        {

        }
    }



}
