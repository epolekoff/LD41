using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraFirstPersonState : AbsState
{
    public float sensitivityX = 3F;
    public float sensitivityY = 3F;

    public float webGL_sensitivityX = 1f;
    public float webGL_sensitivityY = 1f;

    public float GyroRotationSpeedX = 2f;
    public float GyroRotationSpeedY = 2f;

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
#if UNITY_EDITOR
        RotateCamera(camera);
#elif UNITY_ANDROID || UNITY_IOS
        Input.gyro.enabled = true;
        RotateCamera_Mobile(camera);
#else
        RotateCamera(camera);
#endif
    }

    public override void Enter(IStateMachineEntity entity)
    {
        rotationY = 0;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

#if UNITY_WEBGL
        sensitivityX = webGL_sensitivityX;
        sensitivityY = webGL_sensitivityY;
#endif
    }

    /// <summary>
    /// When exiting first person mode.
    /// </summary>
    /// <param name="entity"></param>
    public override void Exit(IStateMachineEntity entity)
    {
        m_shooter = null;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Hide the First Person Canvas.
        GameManager.Instance.GameCanvas.FirstPersonCanvas.gameObject.SetActive(false);
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

    /// <summary>
    /// Rotate the camera with gyro.
    /// </summary>
    /// <param name="camera"></param>
    private void RotateCamera_Mobile(GameCamera camera)
    {
        camera.transform.Rotate(
            -Input.gyro.rotationRateUnbiased.x * GyroRotationSpeedX,
            -Input.gyro.rotationRateUnbiased.y * GyroRotationSpeedY,
            0);// -Input.gyro.rotationRateUnbiased.z * GyroRotationSpeedY);
    }
}
