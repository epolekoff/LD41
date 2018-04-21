using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    /// <summary>
    /// My player number
    /// </summary>
    public int Number { get; set; }

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
	public void Update () {
        HandleInput();
	}

    private void HandleInput()
    {
        // Click to select units/move
        if(Input.GetMouseButtonDown(0))
        {

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
}
