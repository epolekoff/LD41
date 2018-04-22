using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>, IStateMachineEntity
{

    // Consts
    public const int TeamCount = 2;

    // Editor Data
    public List<TeamData> TeamData;
    public GameCamera GameCamera;
    public GameMap Map;
    public GameCanvas GameCanvas;
    public Transform PlayerStartingLookPoint;

    // Public Data
    public List<Shooter>[] Teams = new List<Shooter>[TeamCount];
    public List<Player> Players = new List<Player>();
    public bool IsGameActive { get; set; }
    public int TurnCount { get; set; }

    // Private data
    private Player m_currentPlayer;
    private FiniteStateMachine m_stateMachine;
    public FiniteStateMachine GetStateMachine(int number = 0) { return m_stateMachine; }

    // Use this for initialization
    void Start ()
    {

        // Create the players
        CreatePlayers();

        // Create all of the teams.
        CreateTeams();

        // Position the teams on the grid.
        PositionTeams();

        // State machine stuff.
        m_stateMachine = new FiniteStateMachine(new GameState(), this);
        IsGameActive = true;

        // Turn counter
        TurnCount = 0;

        // Show turn graphic for the player
        GameCanvas.ShowNextTurnGraphic(m_currentPlayer);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Update the current player each frame.
        m_currentPlayer.Update();
        m_stateMachine.Update();

        // Quit the game.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }

    /// <summary>
    /// Tell the next player that they are up.
    /// </summary>
    public void SetNextPlayerTurn()
    {
        if(!IsGameActive)
        {
            return;
        }

        int currentPlayerNumber = m_currentPlayer.Number;
        int nextPlayerNumber = (int)Mathf.Repeat(++currentPlayerNumber, 2);
        m_currentPlayer = Players[nextPlayerNumber];
        m_currentPlayer.SetMyTurn();

        // Show a graphic.
        GameCanvas.ShowNextTurnGraphic(m_currentPlayer);

        TurnCount++;
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
            Players[i].Team = Teams[i];
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
                Shooter shooter = team[i];
                Vector2 tilePosition = TeamData[t].StartingPositions[i];

                // Record the player on that tile.
                Map.MoveObjectToTile(shooter, (int)tilePosition.x, (int)tilePosition.y, firstTimeSetup: true);

                // Make the shooter look at the starting point.
                shooter.CameraSocket.LookAt(PlayerStartingLookPoint);
            }
        }
    }

    /// <summary>
    /// A player lost, the game is over.
    /// </summary>
    public void PlayerLost(int number)
    {
        m_stateMachine.ChangeState(new VictoryState());
        IsGameActive = false;
    }
}
