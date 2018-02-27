using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject screen;
    bool startFade;
    Color currentColor;
    float timeToLerp = 1;
    float timeStarted;

    private void Start()
    {
        Color tmp = new Color(0, 0, 0, 0);
        screen.GetComponent<Image>().color = tmp;
        screen.SetActive(false);
        startFade = false;
        currentColor = screen.GetComponent<Image>().color;
    }


    private void FixedUpdate()
    {
        if (startFade)
        {

            float timesincestarted = Time.time - timeStarted;
            float perc = timesincestarted / timeToLerp;
             screen.GetComponent<Image>().color = Color.Lerp(screen.GetComponent<Image>().color, new Color(0, 0, 0, 1), perc);
            if (perc >= 1)
                StartCoroutine(FadeOut());
        }
            
    }

    public void BeginFade()
    {
        timeStarted = Time.time;
        screen.SetActive(true);
        //StartCoroutine(FadeOut());
        startFade = true;
    }

    IEnumerator FadeOut()    {
        
             
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("dive");
    }
    
}
