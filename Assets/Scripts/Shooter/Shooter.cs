﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public ShooterData Data;
    public GameObject Visual;
    public Transform CameraSocket;
    public ShooterCanvas ShooterCanvas;
    public GameObject BulletPrefab;
    public Transform BulletSpawnPoint;

    public Vector2 TilePosition;

    public int TeamId { get; set; }

    private Material m_teamColor;

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
        m_teamColor = teamColor;
    }

    /// <summary>
    /// Fire a bullet.
    /// </summary>
    public void Fire(System.Action onDestroyed)
    {
        var bulletObject = GameObject.Instantiate(BulletPrefab, BulletSpawnPoint.transform.position, Quaternion.identity) as GameObject;
        bulletObject.GetComponent<Rigidbody>().AddForce(CameraSocket.forward * 1000f);
        bulletObject.GetComponent<Bullet>().SetTeamColor(m_teamColor);
        bulletObject.GetComponent<Bullet>().OnDestroyed += onDestroyed;
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
    public void TakeDamage(int damage)
    {
        m_currentHealth -= damage;
        if(m_currentHealth <= 0)
        {
            Die();
        }

        ShooterCanvas.SetHealth((float)((float)m_currentHealth / (float)Data.Health));
    }

    /// <summary>
    /// When this player dies, do some stuff.
    /// </summary>
    private void Die()
    {
        // Destroy the game object.
        //GameObject.Destroy(this.gameObject);

        // Remove the object from the map tile system.
    }
}
