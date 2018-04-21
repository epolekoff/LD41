using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraFirstPersonState : AbsState
{
    public float sensitivityX = 8F;
    public float sensitivityY = 8F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    private Shooter m_shooter;

    /// <summary>
    /// Constructor
    /// </summary>
    public void SetShooter(Shooter shooter)
    {
        m_shooter = shooter;
    }

    /// <summary>
    /// Update
    /// </summary>
    public override void Update(IStateMachineEntity entity)
    {
        HandleInput((GameCamera)entity);

        // Keep the camera socket on the shooter synced with the camera, 
        // so this shooter's last view position is saved between modes.
        m_shooter.CameraSocket.transform.rotation = ((GameCamera)entity).transform.rotation;
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

        // Rotate the camera.
        RotateCamera(camera);

        // Lean the camera.
    }

    /// <summary>
    /// When exiting first person mode.
    /// </summary>
    /// <param name="entity"></param>
    public override void Exit(IStateMachineEntity entity)
    {
        m_shooter = null;
    }

    /// <summary>
    /// Rotate the camera using the mouse.
    /// </summary>
    private void RotateCamera(GameCamera camera)
    {
        float rotationX = camera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        camera.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
}
