using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_UI : MonoBehaviour {

    public static Manager_UI instance = null;
    public List<GameObject> BubbleSprites = new List<GameObject>();

    public GameObject player;
    public Vector3 offset;

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
        
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position + offset;

        if(player.transform.position.y <= -7)
        {
            if (offset.y < 1.2f)
                offset.y += 0.35f * Time.deltaTime;
        }
        if(player.transform.position.y > -7){
                if (offset.y > 0.2f) { 
                    offset.y -= 0.35f * Time.deltaTime;
                }   
        }
	}
}
