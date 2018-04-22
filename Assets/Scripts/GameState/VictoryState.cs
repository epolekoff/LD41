using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : AbsState
{

    public override void Enter(IStateMachineEntity entity)
    {
        GameManager.Instance.GameCanvas.VictoryCanvas.SetActive(true);
    }

    public override void Update(IStateMachineEntity entity)
    {
        // Click to replay the game.
        if(Input.GetMouseButtonDown(0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }
}
