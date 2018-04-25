using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonCanvas : MonoBehaviour {

    public Image Left;
    public Image Right;
    public Text Timer;

    public GameObject PhoneCanvas;
    public GameObject PCCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

#if UNITY_ANDROID || UNITY_IOS
        PhoneCanvas.SetActive(true);
        PCCanvas.SetActive(false);
#else
        PhoneCanvas.SetActive(false);
        PCCanvas.SetActive(true);
#endif

    }

    /// <summary>
    /// Timer
    /// </summary>
    public void SetTimer(float timer, float maxTimer, Material teamColor)
    {
        Timer.text = Mathf.CeilToInt(timer).ToString();
        float ratio = 1 - (timer / maxTimer);
        Left.fillAmount = ratio;
        Right.fillAmount = ratio;
        Left.color = teamColor.color;
        Right.color = teamColor.color;
    }
}
