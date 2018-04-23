using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject BulletHitEffect;
    public GameObject BulletMissEffect;

    public AudioClip HitShooter;
    public AudioClip HitGround;

    private const float GravityForce = 2f;
    private const float MaxBulletLifetime = 4f;
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
            DestroyMe(false);
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
        // If it hit a shooter, deal damage to them.
        BodyPart bodyPart = col.gameObject.GetComponentInParent<BodyPart>();
        if(bodyPart == null)
        {
            // Destroy the bullet
            DestroyMe(false);
            return;
        }
        bodyPart.OnHit(col.contacts[0]);

        // Destroy the bullet
        DestroyMe(true);
    }

    private void DestroyMe(bool hitPlayer)
    {
        GameCamera camera = GetComponentInChildren<GameCamera>();
        if (camera != null)
        {
            camera.transform.parent = null;
        }

        if(!hitPlayer)
        {
            GameObject.Instantiate(BulletMissEffect, transform.position, Quaternion.identity);
            PlaySound(HitGround);
        }
        else
        {
            GameObject.Instantiate(BulletHitEffect, transform.position, Quaternion.identity);
            PlaySound(HitShooter);
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

    private void PlaySound(AudioClip clip)
    {
        AudioManager.Instance.PlaySound(clip);
    }
}
