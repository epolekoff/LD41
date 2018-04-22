using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private const float GravityForce = 10f;
    private const float MaxGrenadeLifetime = 4f;
    private const int GrenadeDamage = 1;

    private const float ExplodeForce = 300f;
    private const float ExplosionRadiusValue = 2f;

    private float m_lifeTimer = MaxGrenadeLifetime;

    public event System.Action OnDestroyed;


    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(-Vector3.up * GravityForce);

        m_lifeTimer -= Time.deltaTime;
        if(m_lifeTimer <= 0)
        {
            Explode();
        }
    }

    /// <summary>
    /// Explode and hit things.
    /// </summary>
    private void Explode()
    {
        // Destroy.
        Destroy(gameObject);

        // Hit things.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadiusValue);
        int i = 0;
        while (i < hitColliders.Length)
        {
            var explodeVector = ExplodeForce * (hitColliders[i].transform.position - transform.position).normalized;

            // Check if the explosion hit a body part.
            BodyPart bodyPart = hitColliders[i].gameObject.GetComponent<BodyPart>();
            if (bodyPart != null)
            {
                bodyPart.OnHitByGrenade(explodeVector);
            }

            // Check if the explosion hit an obstacle.
            Obstacle obstacle = hitColliders[i].gameObject.GetComponentInParent<Obstacle>();
            if(obstacle != null)
            {
                obstacle.Explode(explodeVector);
            }

            // If this is a piece of debris, add some force.
            Rigidbody rigidbody = hitColliders[i].gameObject.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.AddForce(explodeVector);
            }

            i++;
        }
    }

    /// <summary>
    /// Set bullet colors.
    /// </summary>
    public void SetTeamColor(Material material)
    {
        GetComponent<Renderer>().material = material;
    }

    /// <summary>
    /// When the object is destroyed, tell the game state.
    /// </summary>
    void OnDestroy()
    {
        if (OnDestroyed != null)
        {
            OnDestroyed();
        }
    }
}
