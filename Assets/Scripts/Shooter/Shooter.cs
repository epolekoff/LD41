using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public ShooterData Data;
    public GameObject Visual;
    public Transform CameraSocket;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Set this guy's team color.
    /// </summary>
    public void SetTeamColor(Material teamColor)
    {
        foreach(var renderer in Visual.GetComponentsInChildren<Renderer>())
        {
            renderer.material = teamColor;
        }
    }
}
