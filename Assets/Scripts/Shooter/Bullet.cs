using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float GravityForce = 2f;
    private const float MaxBulletLifetime = 5f;

    public event System.Action OnDestroyed;


	/// <summary>
    /// Start
    /// </summary>
	void Start ()
    {
        GameObject.Destroy(gameObject, MaxBulletLifetime);
	}
	
	/// <summary>
    /// Update
    /// </summary>
	void Update ()
    {
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
        Destroy(gameObject);

        // If it hit a shooter, deal damage to them.
        BodyPart bodyPart = col.gameObject.GetComponentInParent<BodyPart>();
        if(bodyPart == null)
        {
            return;
        }

        bodyPart.OnHit(col.contacts[0]);
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
