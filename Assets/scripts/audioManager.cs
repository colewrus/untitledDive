using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour {

    public static audioManager Instance;
    public enum stage { menu, dive };

    public stage myStage;

    public List<AudioClip> clips = new List<AudioClip>();

    public AudioSource self;
    public float timer;
    public float tick;
    float delay0;

    bool switch0;  //dive intro
    bool switch1;  //cave transition
    bool switch2;  //standard loop
    bool switch3;

    public bool caveSwitch;
    GameObject player;

	//AudioSource newAS;
    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {
        self = this.GetComponent<AudioSource>();
        if (myStage == stage.menu) {
            tick = 0;
            self.clip = clips[0];
            timer = clips[0].length;
            self.Play();
        }

        if(myStage == stage.dive)
        {
            self.clip = clips[0];
            timer = clips[0].length;
            tick = 0;
            self.Play();
            caveSwitch = false;
            player = GameObject.FindGameObjectWithTag("Player");
            switch0 = false;
            switch1 = false;
        }
        
	}

    // Update is called once per frame
    void Update()
    {
        SaveSelf();
        if(myStage == stage.menu)
            Menu();
        if(myStage == stage.dive)
            Dive();
    }

    void Menu()
    {
        if (!self.isPlaying)
        {
            if (tick < timer)
            {
                tick += 1.1f * Time.deltaTime;
            }
            else
            {
                tick = 0;
                self.Play();
            }
        }
    }

    void Dive()
    {

        //this is for the start
        if (!switch0)
        {
            if (tick < timer- 0.1f)
            {
                tick += 1 * Time.deltaTime;
            }
            else
            {
                tick = 0;
                Crossfade(clips[1], 0.2f);
                switch0 = true;
                switch2 = true;
            }
        }
        /*
        //after intro basic loop
        if(switch0 && !caveSwitch && !switch2)
        {
            
            Crossfade(clips[6], 0.3f, true);
            Debug.Log(self.clip.name);
            self.loop = true;
            switch2 = true;
        }
        */

	

		//Enter the Cave
        if (player.GetComponent<player>().inCave && !caveSwitch) {
            switch2 = false;
            switch1 = false;
            Debug.Log("cave muzak");
            timer = clips[3].length;
			Crossfade(clips[3], 1f, true);
			self.loop = true;
            caveSwitch = true;
        }

		//Exit the cave
        if (!player.GetComponent<player>().inCave && caveSwitch && switch0) {
            timer = clips[1].length;
            //self.clip = clips[1];
            Crossfade(clips[1], 0.8f);
			Debug.Log (self.clip.name);
			self.loop = true;
            caveSwitch = false;
        }
        
        //cave loop
        /*
        if (caveSwitch && !switch1)
        {
            if(tick < timer-0.15f)
            {
                tick += Time.deltaTime;
            }else
            {
                self.loop = true;
				Crossfade(clips[3], 0.1f, true);
                switch1 = true;
            }            
        }
        */
		if (switch2)
			self.loop = true;
        if (caveSwitch)
            self.loop = true;      

    }

    void SaveSelf()
    {
        if(self == null)
        {
            self = this.GetComponent<AudioSource>();
		}
    }

	public void Crossfade(AudioClip newTrack, float fadeTime = 1.0f, bool loopy = false)
    {
        AudioSource newAS = gameObject.AddComponent<AudioSource>();
        newAS.volume = 0.0f;

        newAS.clip = newTrack;
        newAS.Play();
        StartCoroutine(_Crossfade(newAS, fadeTime));
        
		self.loop = loopy;
    }

    IEnumerator _Crossfade(AudioSource newsource, float fadeTime)
    {
        float t = 0.0f;

        while(t < fadeTime)
        {
            newsource.volume = Mathf.Lerp(0.0f, 1.0f, t / fadeTime);
            GetComponent<AudioSource>().volume = 1.0f - newsource.volume;      

            t += Time.deltaTime;
            yield return null;
        }
        newsource.volume = 1.0f;
		Destroy(this.GetComponent<AudioSource>());
        SaveSelf();
    }

}
