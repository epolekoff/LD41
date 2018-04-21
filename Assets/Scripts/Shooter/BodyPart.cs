using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour {

    public Shooter Shooter;
    public int DamagePerHit;

    /// <summary>
    /// When hit with a bullet, deal damage to the parent.
    /// </summary>
	public void OnHit()
    {
        Shooter.TakeDamage(DamagePerHit);
    }
}
