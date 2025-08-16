using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePickup : Pickup
{
	public DamagePowerup powerup;	//
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void OnTriggerEnter(Collider other)
    {
        // variable to store other object's PowerupController - if it has one
        PowerupManager powerupManager = other.GetComponent<PowerupManager>();
		GameManager.inst.SpawnSoundEffect(audioClip, transform.position);
        // If the other object has a PowerupController
        if (powerupManager != null) {
            // Add the powerup
            powerupManager.GainBuff(powerup);

            // Destroy this pickup
            Destroy(gameObject);
        }
    }
}

