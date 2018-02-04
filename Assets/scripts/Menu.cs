using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public void StartGame()
    {
        //probably play sound, run coroutine that takes the clip's runtime as a time parameter + 0.5 or something
        SceneManager.LoadScene("dive");
    }
}
