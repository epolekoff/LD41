using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour {

    public FirstPersonCanvas FirstPersonCanvas;
    public GameObject VictoryCanvas;
    public GameObject NextTurnGraphic;
    public GameObject FirstPersonTutorial;
    public GameObject ThirdPersonTutorial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Turn graphic.
    /// </summary>
    /// <param name="player"></param>
    public void ShowNextTurnGraphic(Player player)
    {
        string teamColorName = player.TeamColorName;
        TMPro.TextMeshProUGUI text = NextTurnGraphic.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Image glow = NextTurnGraphic.GetComponentInChildren<Image>();

        // Text and color
        glow.color = player.Team[0].TeamColor.color;
        text.text = teamColorName + "'s Turn";

        // Animation
        NextTurnGraphic.GetComponent<Animator>().SetTrigger("show");
    }
}
