using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_UI : MonoBehaviour {

    public static Manager_UI instance = null;
    public List<GameObject> BubbleSprites = new List<GameObject>();
    public List<GameObject> PlayerHealth = new List<GameObject>();

    public Vector3 dampVel;

    public GameObject resurfaceText;
    public GameObject player;
    public Vector3 offset;
    public float speed;

    float startOffsetY;
    public Text coinAmount;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    // Use this for initialization
    void Start () {
		for(int i=0; i < BubbleSprites.Count; i++)
        {
            BubbleSprites[i].SetActive(false);
        }
        startOffsetY = offset.y;
        /*
        for(int i=0; i<PlayerHealth.Count; i++)
        {
            PlayerHealth[i].SetActive(true);
        }
        */

        coinAmount.text = "" + player.GetComponent<player>().coins;
        resurfaceText.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 tempPos = player.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, tempPos, ref dampVel, speed);
             
        
        
        transform.position = player.transform.position + offset;

        /*
        if(player.transform.position.y <= player.GetComponent<player>().cameraRubberBandSpot)
        {
            if (offset.y < 1.2f)
                offset.y += 0.35f * Time.deltaTime;
        }
        if(player.transform.position.y > player.GetComponent<player>().cameraRubberBandSpot)
        {
                if (offset.y > startOffsetY) { 
                    offset.y -= 0.35f * Time.deltaTime;
                }   
        }
        */
	}

    public void AddCoin()
    {
        coinAmount.text = "x" + player.GetComponent<player>().coins;
    }
}
