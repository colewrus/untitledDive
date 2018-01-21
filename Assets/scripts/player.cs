using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {


    Animator ani;
    public bool inWater;
    Rigidbody2D rb;


	// Use this for initialization
	void Start () {
        ani = this.GetComponent<Animator>();
        inWater = false;
        rb = this.GetComponent<Rigidbody2D>();
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


    void WaterCheck()
    {
        if (inWater)
        {
            if(rb.gravityScale > 0 && transform.position.y < 0)
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, 0);
            }
        }
    }

    IEnumerator JumpDelay(float t)
    {
        yield return new WaitForSeconds(t);
        rb.AddForce(((Vector2.up * 1.5f) + Vector2.left), ForceMode2D.Impulse);
        inWater = true;
        
    }
    void WaterIdle()
    {
        if (inWater)
        {

        }
    }



}
