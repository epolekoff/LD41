using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Obstacle ShatteredVersion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Explode from a grenade.
    /// </summary>
    public void Explode(Vector3 force)
    {
        // Spawn the Shattered Version if there is one, and add some force
        if(ShatteredVersion != null)
        {
            // Spawn a shattered version at my position.
            var shatteredVersion = GameObject.Instantiate(ShatteredVersion, transform.position, transform.rotation);

            // Add some force to it.
            foreach(Transform child in shatteredVersion.transform)
            {
                child.gameObject.GetComponent<Rigidbody>().AddForce(force);
                child.transform.parent = null;
            }

            AudioManager.Instance.PlaySound(AudioManager.Instance.ShatterSound);

            // Destroy me
            GameObject.Destroy(gameObject);
        }
    }
}
