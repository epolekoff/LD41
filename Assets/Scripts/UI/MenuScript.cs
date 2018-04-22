using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public void GoToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
