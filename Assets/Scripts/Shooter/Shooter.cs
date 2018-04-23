using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public ShooterData Data;
    public GameObject Visual;
    public Transform CameraSocket;
    public ShooterCanvas ShooterCanvas;
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;
    public GameObject GrenadePrefab;

    public AudioClip ShootSound;
    public AudioClip DeadSound;

    public Vector2 TilePosition;

    public int TeamId { get; set; }

    public Material TeamColor { get; set; }
    public bool IsDead { get; set; }

    private const float BulletHitForce = 200f;
    private const float BulletFireForce = 1000f;
    private const float GrenadeThrowForce = 800f;

    private int m_currentHealth;

	// Use this for initialization
	void Start ()
    {
        // Set starting values.
        m_currentHealth = Data.Health;
        ShooterCanvas.SetHealth(1);

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// Set this guy's team color.
    /// </summary>
    public void SetTeamColor(Material teamColor)
    {
        foreach(var renderer in Visual.GetComponentsInChildren<Renderer>())
        {
            renderer.material = teamColor;
        }
        TeamColor = teamColor;
    }

    /// <summary>
    /// Fire a bullet.
    /// </summary>
    public void Fire(System.Action onDestroyed)
    {
        var bulletObject = GameObject.Instantiate(
            BulletPrefab, 
            BulletSpawnPoint.transform.position, 
            Quaternion.LookRotation(-CameraSocket.right, Vector3.up)) as GameObject;
        bulletObject.GetComponent<Rigidbody>().AddForce(CameraSocket.forward * BulletFireForce);
        bulletObject.GetComponent<Bullet>().SetTeamColor(TeamColor);
        bulletObject.GetComponent<Bullet>().OnDestroyed += onDestroyed;

        // Make the camera follow the bullet.
        GameManager.Instance.GameCamera.FollowBullet(bulletObject.GetComponent<Bullet>());

        // Play sound
        AudioManager.Instance.PlaySound(ShootSound);
    }

    /// <summary>
    /// Throw a grenade.
    /// </summary>
    public void ThrowGrenade(System.Action onDestroyed)
    {
        var grenadeObject = GameObject.Instantiate(GrenadePrefab, BulletSpawnPoint.transform.position, Quaternion.identity) as GameObject;
        grenadeObject.GetComponent<Rigidbody>().AddForce(CameraSocket.forward * GrenadeThrowForce);
        grenadeObject.GetComponent<Grenade>().SetTeamColor(TeamColor);
        grenadeObject.GetComponent<Grenade>().OnDestroyed += onDestroyed;

        // Make the camera follow the grenade.
        GameManager.Instance.GameCamera.FollowGrenade(grenadeObject.GetComponent<Grenade>());
    }

    /// <summary>
    /// Hide the visual so we don't see it in first person.
    /// </summary>
    public void HideVisual()
    {
        Visual.SetActive(false);
    }

    public void ShowVisual()
    {
        Visual.SetActive(true);
    }

    /// <summary>
    /// Take some amount of damage.
    /// </summary>
    public void TakeDamage(int damage, Vector3 point, Vector3 direction)
    {
        m_currentHealth -= damage;
        if(m_currentHealth <= 0)
        {
            Die(transform.position, direction);
        }

        ShooterCanvas.SetHealth((float)((float)m_currentHealth / (float)Data.Health));
    }
    public void TakeDamage(int damage, Vector3 direction)
    {
        TakeDamage(damage, transform.position, direction);
    }
    public void TakeDamage(int damage, ContactPoint hitPoint)
    {
        TakeDamage(damage, hitPoint.point, -hitPoint.normal * BulletHitForce);
    }

    /// <summary>
    /// When this player dies, do some stuff.
    /// </summary>
    private void Die(Vector3 point, Vector3 direction)
    {
        if(IsDead)
        {
            return;
        }
        IsDead = true;

        AudioManager.Instance.PlaySound(DeadSound);

        // Knock the person over.
        Rigidbody body = gameObject.AddComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.None;
        body.AddForceAtPosition(direction, point);

        // Remove the object from the map tile system.
        GameManager.Instance.Map.RemoveObjectFromTiles(this);

        // Tell my player to check if I lost
        GameManager.Instance.Players[TeamId].CheckIsLoser();
    }
}
