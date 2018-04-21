using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    // Consts
    public const int TeamCount = 2;

    // Editor Data
    public List<TeamData> TeamData;
    public GameCamera GameCamera;
    public GameMap Map;

    // Public Data
    public List<Shooter>[] Teams = new List<Shooter>[TeamCount];
    public List<Player> Players = new List<Player>();

    // Private data
    private Player m_currentPlayer;

	// Use this for initialization
	void Start () {

        // Create the players
        CreatePlayers();

        // Create all of the teams.
        CreateTeams();

        // Position the teams on the grid.
        PositionTeams();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Update the current player each frame.
        m_currentPlayer.Update();
	}

    /// <summary>
    /// Create some players.
    /// </summary>
    private void CreatePlayers()
    {
        for(int i = 0; i < TeamCount; i++)
        {
            var player = new Player(i);
            Players.Add(player);
        }

        // Set the starting player.
        m_currentPlayer = Players[0];
    }

    /// <summary>
    /// Create the teams and all shooters on the teams.
    /// </summary>
    private void CreateTeams()
    {
        for (int i = 0; i < TeamCount; i++)
        {
            Teams[i] = ShooterFactory.CreateShooterTeam(i, TeamData[i]);
        }
    }

    /// <summary>
    /// After the teams are created, position them on the tiles.
    /// </summary>
    private void PositionTeams()
    {
        // Set up each team.
        for(int t = 0; t < Teams.Length; t++)
        {
            for(int i = 0; i < Teams[t].Count; i++)
            {
                var team = Teams[t];
                Vector2 tilePosition = TeamData[t].StartingPositions[i];

                // Record the player on that tile.
                Map.MoveObjectToTile(team[i], (int)tilePosition.x, (int)tilePosition.y, firstTimeSetup: true);
            }
        }
    }

    
}
