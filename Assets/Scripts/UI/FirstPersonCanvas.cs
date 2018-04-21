using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonCanvas : MonoBehaviour {

    public Image Left;
    public Image Right;
    public Text Timer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
