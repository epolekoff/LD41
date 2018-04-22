using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    FirstPerson,
    ThirdPerson
}

public class GameCamera : MonoBehaviour, IStateMachineEntity
{

    public static float CameraPanSpeed = 15f;
    public static float CameraRotateIncrement = 90f;
    public static float CameraLerpTime = 0.5f;
    public static float CameraZoomSpeed = 1f;

    private const float CameraSizeMin = 4;
    private const float CameraSizeMax = 24;

    private const float FOV = 60f;
    private const float zNear = 0.3f;
    private const float zFar = 1000f;

    private FiniteStateMachine m_stateMachine;
    private GameCameraFirstPersonState m_firstPersonState;
    private GameCameraThirdPersonState m_thirdPersonState;


    private CameraMode m_currentMode = CameraMode.ThirdPerson;
    private bool m_cameraAnimating;
    private Quaternion m_desiredRotation;
    private Vector3 m_desiredPosition;
    private Quaternion m_lastOrthoRotation;
    private Vector3 m_lastOrthoPosition;
    private Matrix4x4 m_startingOrthoProjectionMatrix;

    public FiniteStateMachine GetStateMachine(int number = 0){ return m_stateMachine; }

    public bool IsAnimating { get { return m_cameraAnimating; } }

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        // Ortho hack. Math is hard.
        m_startingOrthoProjectionMatrix = GetComponent<Camera>().projectionMatrix;

        // Make the states
        m_firstPersonState = new GameCameraFirstPersonState();
        m_thirdPersonState = new GameCameraThirdPersonState();

        m_stateMachine = new FiniteStateMachine(m_thirdPersonState, this);
    }

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        m_stateMachine.Update();
    }

    /// <summary>
    /// Move to first person mode.
    /// </summary>
    /// <param name="shooter"></param>
    public void TransitionToFirstPerson(Shooter shooter)
    {
        // Already in this state, return.
        if (m_currentMode == CameraMode.FirstPerson)
        {
            return;
        }

        // Get the destination
        m_lastOrthoPosition = transform.position;
        m_lastOrthoRotation = transform.rotation;
        m_desiredPosition = shooter.CameraSocket.position;
        m_desiredRotation = shooter.CameraSocket.rotation;

        // Toggle it to Perspective
        GetComponent<Camera>().projectionMatrix = Matrix4x4.Perspective(FOV, 16f/9f, zNear, zFar);

        // Hide the 3rd person canvas.
        GameManager.Instance.GameCanvas.ThirdPersonTutorial.gameObject.SetActive(false);

        // Change the state machine state
        m_currentMode = CameraMode.FirstPerson;
        m_firstPersonState = new GameCameraFirstPersonState();
        m_firstPersonState.SetShooter(shooter);
        m_stateMachine.ChangeState(m_firstPersonState);

        // Move the camera
        StartCoroutine(LerpCameraToRotation( () =>
        {
            // Show the First Person Canvas.
            GameManager.Instance.GameCanvas.FirstPersonCanvas.gameObject.SetActive(true);
        }));
    }

    /// <summary>
    /// Move to third person mode.
    /// </summary>
    public void TransitionToThirdPerson()
    {
        // Already in this state, return.
        if (m_currentMode == CameraMode.ThirdPerson)
        {
            return;
        }

        // Get the destination
        m_desiredPosition = m_lastOrthoPosition;
        m_desiredRotation = m_lastOrthoRotation;

        // Change the state machine state
        m_currentMode = CameraMode.ThirdPerson;
        m_stateMachine.ChangeState(new GameCameraThirdPersonState());

        // Hide the First Person Canvas.
        GameManager.Instance.GameCanvas.FirstPersonCanvas.gameObject.SetActive(false);

        // Move the camera
        StartCoroutine(LerpCameraToRotation(() =>
        {
            // Toggle it to Orthographic
            GetComponent<Camera>().projectionMatrix = m_startingOrthoProjectionMatrix;
            GetComponent<Camera>().ResetProjectionMatrix();

            // Show the 3rd person canvas.
            if(GameManager.Instance.TurnCount<2)
            {
                GameManager.Instance.GameCanvas.ThirdPersonTutorial.gameObject.SetActive(true);
            }
        }));
    }

    /// <summary>
    /// Keep the camera slightly behind the bullet as it travels.
    /// </summary>
    public void FollowBullet(Bullet bullet)
    {
        m_stateMachine.ChangeState(new GameCameraFollowBulletState(bullet));
    }

    /// <summary>
    /// Keep close behind the grenade.
    /// </summary>
    public void FollowGrenade(Grenade grenade)
    {
        m_stateMachine.ChangeState(new GameCameraFollowGrenadeState(grenade));
    }

    /// <summary>
    /// Move the camera across the screen based on movement speed.
    /// </summary>
    public void PanCamera(Vector2 amount)
    {
        transform.position += transform.up * amount.y * CameraPanSpeed * Time.deltaTime;
        transform.position += transform.right * amount.x * CameraPanSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Rotate
    /// </summary>
    /// <param name="direction"></param>
    public void RotateCamera(int direction)
    {
        if (m_cameraAnimating)
        {
            return;
        }

        float angle = CameraRotateIncrement * Mathf.Sign(direction);

        // Raycast to hit a tile.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10000f, LayerMask.GetMask("CameraRotationPlane")))
        {
            // Rotate around the hit point.
            Quaternion oldRotation = transform.rotation;
            Vector3 oldPosition = transform.position;
            transform.RotateAround(hit.point, Vector3.up, angle);

            // Keep the camera looking at the point
            transform.LookAt(hit.point);

            // If the camera is too close to the point, move it back so it doesn't clip into the level
            if (Vector3.Distance(transform.position, hit.point) < 10)
            {
                transform.position += transform.forward * -100;
            }

            // Start a coroutine to rotate the camera instead of snapping it.
            m_desiredRotation = transform.rotation;
            m_desiredPosition = transform.position;
            transform.rotation = oldRotation;
            transform.position = oldPosition;
            StartCoroutine(LerpCameraToRotation());
        }
        else
        {
            // Rotate the camera manually by euler angles and keep it isometric.
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y + CameraRotateIncrement * Mathf.Sign(direction),
                transform.rotation.eulerAngles.z);
        }
    }

    /// <summary>
    /// Scroll to zoom
    /// </summary>
    public void ZoomCamera(float zoomAmount)
    {
        float currentSize = GetComponent<Camera>().orthographicSize;
        currentSize += zoomAmount;

        GetComponent<Camera>().orthographicSize = Mathf.Clamp(currentSize, CameraSizeMin, CameraSizeMax);
    }

    /// <summary>
    /// Coroutine to move the camera into place over time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LerpCameraToRotation(System.Action callback = null)
    {
        m_cameraAnimating = true;

        float timer = 0;

        Quaternion startRotation = transform.rotation;
        Vector3 startPosition = transform.position;

        while (timer < CameraLerpTime)
        {
            timer += Time.deltaTime;
            float ratio = timer / CameraLerpTime;

            transform.rotation = Quaternion.Lerp(startRotation, m_desiredRotation, ratio);
            transform.position = Vector3.Lerp(startPosition, m_desiredPosition, ratio);

            yield return new WaitForEndOfFrame();
        }

        m_cameraAnimating = false;

        if(callback != null)
        {
            callback();
        }
    }
}
