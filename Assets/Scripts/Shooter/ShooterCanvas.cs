using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterCanvas : MonoBehaviour
{

    public Image HealthbarFill;
    public Image HealthbarBackground;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Show or hide the health bars.
    /// </summary>
    /// <param name="show"></param>
    public void ShowHealthBar(bool show)
    {
        HealthbarBackground.gameObject.SetActive(show);
        HealthbarFill.gameObject.SetActive(show);
    }

    /// <summary>
    /// Set the health of this shooter.
    /// </summary>
    public void SetHealth(float ratio)
    {
        // If Health is at full, hide the health bar.
        if(ratio == 1)
        {
            ShowHealthBar(false);
        }

        HealthbarFill.fillAmount = ratio;
    }
}
