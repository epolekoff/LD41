using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public const int TeamSize = 4;
    public const int TeamCount = 2;
    public List<Shooter>[] Teams = new List<Shooter>[TeamCount];

	// Use this for initialization
	void Start () {

        // Create all of the teams.
        CreateTeams();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Create the teams and all shooters on the teams.
    /// </summary>
    private void CreateTeams()
    {
        for (int i = 0; i < TeamCount; i++)
        {
            Teams[i] = ShooterFactory.CreateShooterTeam(i, TeamSize);
        }
    }
}
