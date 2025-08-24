using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeedPowerup : Powerup
{
	
	//public float spdBoost;
	//I think its dumb when health boosts remove the health they gave you when they run out, because then they didnt even do anything, 
	//so im making it so that the health is only removed if it wasnt used

	public override bool Apply(PowerupManager target, bool direct)
    {
		if(direct)
		{
			// Apply Health changes
			Pawn targetPawn = target.GetComponent<Pawn>();
			if (targetPawn != null) 
			{
				targetPawn.moveSpeed += strength;
				targetPawn.turnSpeed += (strength * 20);
				//I have to increase the bullet speed otherwise it is dumb.
				Shooter pawnShooter = targetPawn.GetComponent<Shooter>();
				pawnShooter.fireForce += strength;
			}
			return true;
		} else
		{
			Powerup stackingPowerup = null;
			foreach (Powerup powerup in target.powerups)
			{
				if(powerup as SpeedPowerup != null)
				{
					stackingPowerup = powerup;
				}
			}
			// Apply Health changes
			if(stackingPowerup != null)
				{
					if (stackingPowerup.Stack(target, this))
					{
						return true;
					}

				} else
				{
					// Apply Health changes
					Pawn targetPawn = target.GetComponent<Pawn>();
					if (targetPawn != null) 
					{
						targetPawn.moveSpeed += strength;
						targetPawn.turnSpeed += (strength * 20);
						//I have to increase the bullet speed otherwise it is dumb.
						Shooter pawnShooter = targetPawn.GetComponent<Shooter>();
						pawnShooter.fireForce += strength;
					}
					
				}
				return false;
		}
    }

    public override void Remove(PowerupManager target)
    {
        // TODO: Remove Health changes
		//pretty sure it will cause an error if I dont write this here even though it isnt neccessary
		Pawn targetPawn = target.GetComponent<Pawn>();
        if (targetPawn != null) 
        {
			targetPawn.moveSpeed -= strength;
			targetPawn.turnSpeed -= (strength * 20);
			//I have to increase the bullet speed otherwise it is dumb.
			Shooter pawnShooter = targetPawn.GetComponent<Shooter>();
			pawnShooter.fireForce -= strength;
        }
    }
	
	
}