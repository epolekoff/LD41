using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour {

    public Shooter Shooter;
    public int DamagePerHit;
    public int DamagePerGrenade;

    /// <summary>
    /// When hit with a bullet, deal damage to the parent.
    /// </summary>
	public void OnHit(ContactPoint point)
    {
        Shooter.TakeDamage(DamagePerHit, point);
    }
    public void OnHitByGrenade(Vector3 direction)
    {
        Shooter.TakeDamage(DamagePerGrenade, direction);
    }
}
