using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeedPowerup : Powerup
{
	
	public float spdBoost;
	//I think its dumb when health boosts remove the health they gave you when they run out, because then they didnt even do anything, 
	//so im making it so that the health is only removed if it wasnt used

	public override void Apply(PowerupManager target)
    {
		// Apply Health changes
        Pawn targetPawn = target.GetComponent<Pawn>();
        if (targetPawn != null) 
        {
			targetPawn.moveSpeed += spdBoost;
			//I have to increase the bullet speed otherwise it is dumb.
			Shooter pawnShooter = targetPawn.GetComponent<Shooter>();
			pawnShooter.fireForce += spdBoost;
        }
    }

    public override void Remove(PowerupManager target)
    {
        // TODO: Remove Health changes
		//pretty sure it will cause an error if I dont write this here even though it isnt neccessary
		Pawn targetPawn = target.GetComponent<Pawn>();
        if (targetPawn != null) 
        {
			targetPawn.moveSpeed -= spdBoost;
			//I have to increase the bullet speed otherwise it is dumb.
			Shooter pawnShooter = targetPawn.GetComponent<Shooter>();
			pawnShooter.fireForce -= spdBoost;
        }
    }
	
	
}