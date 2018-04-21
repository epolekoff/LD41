using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraThirdPersonState : AbsState
{

    /// <summary>
    /// Update
    /// </summary>
    public override void Update(IStateMachineEntity entity)
    {
        // Handle Third Person input.
        HandleInput((GameCamera)entity);
    }

    /// <summary>
    /// Take player input and move the camera.
    /// </summary>
    private void HandleInput(GameCamera camera)
    {
        // No input while animating.
        if (camera.IsAnimating)
        {
            return;
        }

        // Pan the camera
        if (Input.GetKey(KeyCode.W))
        {
            camera.PanCamera(new Vector2(0, 1));
        }
        if (Input.GetKey(KeyCode.S))
        {
            camera.PanCamera(new Vector2(0, -1));
        }
        if (Input.GetKey(KeyCode.A))
        {
            camera.PanCamera(new Vector2(-1, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            camera.PanCamera(new Vector2(1, 0));
        }

        // Rotate the camera
        if (Input.GetKeyDown(KeyCode.E))
        {
            camera.RotateCamera(-1);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            camera.RotateCamera(1);
        }

        // Handle scrolling
        camera.ZoomCamera(-Input.mouseScrollDelta.y * GameCamera.CameraZoomSpeed);
    }

   
}
