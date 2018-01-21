using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {


    Animator ani;
    public bool inWater;

	// Use this for initialization
	void Start () {
        ani = this.GetComponent<Animator>();
        inWater = false;
	}
	
	// Update is called once per frame
	void Update () {
        EnterWater();
		
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
            }
        }
    }



}
