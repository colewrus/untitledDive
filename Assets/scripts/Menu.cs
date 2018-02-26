using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject screen;
    bool startFade;
    Color currentColor;

    private void Start()
    {
        Color tmp = new Color(0, 0, 0, 0);
        screen.GetComponent<Image>().color = tmp;
        screen.SetActive(false);
        startFade = false;
        currentColor = screen.GetComponent<Image>().color;
    }


    private void Update()
    {
        if (startFade)
        {
            Debug.Log(currentColor);
             screen.GetComponent<Image>().color = Color.Lerp(screen.GetComponent<Image>().color, new Color(0, 0, 0, 1), 1*Time.deltaTime);
        }
            
    }

    public void BeginFade()
    {
        screen.SetActive(true);
        StartCoroutine(FadeOut());
        startFade = true;
    }

    IEnumerator FadeOut()    {
        
             
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("dive");
    }
    
}
