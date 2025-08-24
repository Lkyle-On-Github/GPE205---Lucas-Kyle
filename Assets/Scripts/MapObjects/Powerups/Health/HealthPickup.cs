using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
	public HealthPowerup powerup;	//
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
        // If the other object has a PowerupController
        if (powerupManager != null) {
			bool playerInRoom = false;
			foreach(PlayerController player in GameManager.inst.listPlayers)
			{
				if(powerupManager.pawn.roomLocation == player.pawn.roomLocation)
				{
					playerInRoom = true;
				}
			}
			if(playerInRoom)
			{
				GameManager.inst.SpawnSoundEffect(audioClip, transform.position);
			}
            // Add the powerup
            powerupManager.GainBuff(powerup);

            // Destroy this pickup
            Destroy(gameObject);
        }
    }
}
