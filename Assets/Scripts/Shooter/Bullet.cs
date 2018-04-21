using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float GravityForce = 4f;

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
        GetComponent<Rigidbody>().AddForce(-Vector3.up * GravityForce);
	}
}
