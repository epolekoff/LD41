using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    SelectingUnit,
    ViewingEnemy,
    MovingUnit,
    Shooting
}

public class Player
{
    /// <summary>
    /// My player number
    /// </summary>
    public int Number { get; set; }

    private PlayerState m_currentState = PlayerState.SelectingUnit;
    private Shooter m_selectedShooter;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="number"></param>
    public Player(int number)
    {
        Number = number;
    }

	/// <summary>
    /// Update this player's actions when it's their turn.
    /// </summary>
	public void Update ()
    {
        HandleInput();
	}

    /// <summary>
    /// Handle clicking on tiles.
    /// </summary>
    private void HandleInput()
    {
        // Click to select units/move
        if(Input.GetMouseButtonDown(0))
        {
            // Click on a tile.
            RaycastHit hit;
            Ray ray = GameManager.Instance.GameCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(
                ray, 
                out hit, 
                10000f, 
                LayerMask.GetMask("MapTile")))
            {
                // Handle clicking on the tile.
                var mapTile = hit.transform.parent.GetComponent<MapTile>();
                ClickOnTile(mapTile);
            }
        }

        // Space to test camera transitions.
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // For testing, focust on the first player on the team.
            GameManager.Instance.GameCamera.TransitionToFirstPerson(GameManager.Instance.Teams[Number][0]);
        }

        // ESC to test camera transitions.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // For testing, focust on the first player on the team.
            GameManager.Instance.GameCamera.TransitionToThirdPerson();
        }
    }

    /// <summary>
    /// When a player clicks on a tile, what happens.
    /// </summary>
    private void ClickOnTile(MapTile tile)
    {
        switch (m_currentState)
        {
            case PlayerState.SelectingUnit:
                ClickOnShooter(tile);
                return;

            case PlayerState.MovingUnit:
                ClickOnMoveRange(tile);
                return;

            case PlayerState.ViewingEnemy:
                ClickOnMoveRange(tile);
                return;

            case PlayerState.Shooting:
                return;
        }
    }

    /// <summary>
    /// Handle when the player clicks off of a valid tile. It should reset state.
    /// </summary>
    private void ClickOffTile()
    {
        // Clear out the selected shooter.
        m_selectedShooter = null;

        // Clear all highlights
        GameManager.Instance.Map.ClearHighlightedTiles();
    }

    /// <summary>
    /// Handle clicking on a shooter, friend or foe.
    /// </summary>
    private void ClickOnShooter(MapTile tile)
    {
        GameMap map = GameManager.Instance.Map;
        Shooter shooter = map.GetShooterOnTile(tile.Position);
        if (shooter != null)
        {
            m_selectedShooter = shooter;
            map.HighlightMovementRange(shooter, this);

            // Was the player a teammate or an enemy.
            if (shooter.TeamId == Number)
            {
                m_currentState = PlayerState.MovingUnit;
            }
            else
            {
                m_currentState = PlayerState.ViewingEnemy;
            }
        }
        else
        {
            ClickOffTile();
        }
    }

    /// <summary>
    /// Move a shooter by clicking on a tile.
    /// </summary>
    private void ClickOnMoveRange(MapTile tile)
    {
        GameMap map = GameManager.Instance.Map;

        // If clicking a blue highlight, move there.
        if (tile.HighlightState == HighlightState.Friendly && m_selectedShooter != null)
        {
            map.MoveObjectToTile(m_selectedShooter, (int)tile.Position.x, (int)tile.Position.y);
            m_currentState = PlayerState.SelectingUnit; // Shooting
            ClickOffTile();
        }
        else
        {
            ClickOffTile();
            m_currentState = PlayerState.SelectingUnit;
        }
    }
}
