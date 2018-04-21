using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HighlightState
{
    None,
    Friendly,
    Enemy
}

public class MapTile : MonoBehaviour
{
    // All tiles are squares, and the same size.
    public static float Width = 2f;

    // Tiles have modifiable height.
    // They default to 1, but you can change it.
    public float Height = 1;

    /// <summary>
    /// The highlight object.
    /// </summary>
    public HighlightState HighlightState { get; private set; }
    public GameObject HighlightFriendly;
    public GameObject HighlightEnemy;

    /// <summary>
    /// The position of this tile in the grid.
    /// </summary>
    public Vector2 Position;

    /// <summary>
    /// If an obstacle is attached to this tile, it here.
    /// </summary>
    public Obstacle Obstacle;

	// Use this for initialization
	void Start () {
        RegisterMapTile(GameManager.Instance.Map, Position, this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// When creating the map tiles, hook them up to be reverse engineered.
    /// </summary>
    private void RegisterMapTile(GameMap gameMap, Vector2 position, MapTile tile)
    {
        // If the tile was already registered, remove it.
        if (gameMap.MapTiles.ContainsKey(position))
        {
            GameObject.Destroy(gameMap.MapTiles[position]);
            gameMap.MapTiles.Remove(position);
        }

        // Register the new tile.
        gameMap.MapTiles.Add(position, tile);
    }

    /// <summary>
    /// Show the highlight, or hide it.
    /// </summary>
    public void ShowHighlight(HighlightState state)
    {
        HighlightState = state;
        switch (state)
        {
            case HighlightState.None:
                HighlightFriendly.SetActive(false);
                HighlightEnemy.SetActive(false);
                break;
            case HighlightState.Friendly:
                HighlightFriendly.SetActive(true);
                HighlightEnemy.SetActive(false);
                break;
            case HighlightState.Enemy:
                HighlightFriendly.SetActive(false);
                HighlightEnemy.SetActive(true);
                break;
        }
    }
}
