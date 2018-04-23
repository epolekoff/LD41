using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public void GoToGame()
    {
        StartCoroutine(PlaySoundAndGoToMenu());
    }

    private IEnumerator PlaySoundAndGoToMenu()
    {
        GetComponent<AudioSource>().Play();
        while (GetComponent<AudioSource>().isPlaying)
        {
            yield return null;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
