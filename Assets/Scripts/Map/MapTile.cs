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
    public static float Width = 1f;

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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
