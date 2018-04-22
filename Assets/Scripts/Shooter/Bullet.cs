using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float GravityForce = 2f;
    private const float MaxBulletLifetime = 5f;
    private float m_lifeTimer = MaxBulletLifetime;

    public event System.Action OnDestroyed;


	/// <summary>
    /// Start
    /// </summary>
	void Start ()
    {
	}
	
	/// <summary>
    /// Update
    /// </summary>
	void Update ()
    {
        m_lifeTimer -= Time.deltaTime;
        if (m_lifeTimer <= 0)
        {
            DestroyMe();
        }

        GetComponent<Rigidbody>().AddForce(-Vector3.up * GravityForce);
	}

    /// <summary>
    /// Set bullet colors.
    /// </summary>
    public void SetTeamColor(Material material)
    {
        GetComponentInChildren<Renderer>().material = material;
        GetComponentInChildren<TrailRenderer>().material.color = material.color;
    }

    /// <summary>
    /// When the bullet collides with anything, destroy it.
    /// </summary>
    /// <param name="col"></param>
    public void OnCollisionEnter(Collision col)
    {
        // Destroy the bullet
        DestroyMe();

        // If it hit a shooter, deal damage to them.
        BodyPart bodyPart = col.gameObject.GetComponentInParent<BodyPart>();
        if(bodyPart == null)
        {
            return;
        }

        bodyPart.OnHit(col.contacts[0]);
    }

    private void DestroyMe()
    {
        GameCamera camera = GetComponentInChildren<GameCamera>();
        if (camera != null)
        {
            camera.transform.parent = null;
        }
        GameObject.Destroy(gameObject);
    }

    /// <summary>
    /// When the object is destroyed, tell the game state.
    /// </summary>
    void OnDestroy()
    {
        if(OnDestroyed != null)
        {
            OnDestroyed();
        }
    }
}
