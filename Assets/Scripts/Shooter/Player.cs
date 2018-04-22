using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    OtherPlayerTurn,
    SelectingUnit,
    ViewingEnemy,
    MovingUnit,
    Shooting,
    WatchingBullet
}

public class Player
{
    /// <summary>
    /// My player number
    /// </summary>
    public int Number { get; set; }

    public List<Shooter> Team { get; set; }

    private PlayerState m_currentState = PlayerState.SelectingUnit;
    private Shooter m_selectedShooter;

    public string TeamColorName { get { return Number == 0 ? "Red" : "Blue"; } }

    private float m_turnTimer = 0f;
    private const float MaxTurnTimer = 5f;

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
        HandleTurnTimer();
    }

    /// <summary>
    /// Tell me it's my turn now.
    /// </summary>
    public void SetMyTurn()
    {
        m_currentState = PlayerState.SelectingUnit;
    }

    /// <summary>
    /// Turn counts down, you fire at the end.
    /// </summary>
    private void HandleTurnTimer()
    {
        if (m_turnTimer > 0)
        {
            m_turnTimer -= Time.deltaTime;
        }

        // Update the UI.
        if(m_selectedShooter != null)
        {
            GameManager.Instance.GameCanvas.FirstPersonCanvas.SetTimer(m_turnTimer, MaxTurnTimer, m_selectedShooter.TeamColor);
        }
        
        if(m_turnTimer <= 0)
        {
            FireBullet();
        }
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

            // Handle clicking to shoot
            FireBullet();
        }

        if(Input.GetMouseButtonDown(1))
        {
            ThrowGrenade();
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

            default:
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
            Shooter selectedShooter = m_selectedShooter;
            map.MoveObjectToTile(m_selectedShooter, (int)tile.Position.x, (int)tile.Position.y, false, () => 
            {
                GameManager.Instance.GameCamera.TransitionToFirstPerson(selectedShooter);
                m_selectedShooter.HideVisual();
                m_currentState = PlayerState.Shooting;
                m_turnTimer = MaxTurnTimer;
            });

            // Clear all highlights
            GameManager.Instance.Map.ClearHighlightedTiles();
        }
        else
        {
            ClickOffTile();
            m_currentState = PlayerState.SelectingUnit;
        }
    }

    /// <summary>
    /// Fire a bullet, then return to
    /// </summary>
    private void FireBullet()
    {
        // Can only fire in the shooting state.
        if(m_currentState != PlayerState.Shooting)
        {
            return;
        }

        // Fire 1 bullet
        m_selectedShooter.Fire(OnBulletDestroyed);

        // Set the state to watching the bullet
        m_currentState = PlayerState.WatchingBullet;
    }

    /// <summary>
    /// When the bullet has been destroyed, go to the next player's turn and return to 3rd person.
    /// </summary>
    private void OnBulletDestroyed()
    {
        // Transition back to 3rd person (should be after bullet is destroyed
        GameManager.Instance.GameCamera.TransitionToThirdPerson();

        // Show the shooter again.
        m_selectedShooter.ShowVisual();

        // Go to the other player's turn
        m_currentState = PlayerState.OtherPlayerTurn;
        GameManager.Instance.SetNextPlayerTurn();
    }

    private void ThrowGrenade()
    {
        // Can only fire in the shooting state.
        if (m_currentState != PlayerState.Shooting)
        {
            return;
        }

        // Allow a player to blow himself up.
        m_selectedShooter.ShowVisual();

        // Throw 1 grenade
        m_selectedShooter.ThrowGrenade(OnBulletDestroyed);

        // Set the state to watching the bullet
        m_currentState = PlayerState.WatchingBullet;
    }

    /// <summary>
    /// Check if I lost.
    /// </summary>
    public void CheckIsLoser()
    {
        foreach(Shooter shooter in Team)
        {
            if(!shooter.IsDead)
            {
                return;
            }
        }

        GameManager.Instance.PlayerLost(Number);
    }
}
