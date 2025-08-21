using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamagePowerup : Powerup
{
    //public float damageBoost;
	//public float preBoostHp;

	public override bool Apply(PowerupManager target, bool direct)
    {
		if(direct)
		{
			if (target.pawn.shooter != null) 
			{
				target.pawn.shooter.BoostDamage(strength);
			}
			return true;
		} else 
		{
			// Apply Health changes
			Powerup stackingPowerup = null;
			foreach (Powerup powerup in target.powerups)
			{
				if(powerup as DamagePowerup != null)
				{
					stackingPowerup = powerup;
				}
			}
			if (target.pawn.shooter != null) 
			{
				//checks if the powerupmanager already has a powerup to stack with
				if(stackingPowerup != null)
				{
					if (stackingPowerup.Stack(target, this))
					{
						return true;
					}

				} else
				{
					target.pawn.shooter.BoostDamage(strength);
					return false;
				}
			}
			//the powerup shouldnt be added if the target is incapable of receiving the buff, so it should return true by default/
			return true;
		}
    }

    public override void Remove(PowerupManager target)
    {
        //Shooter targetShooter = target.GetComponent<Shooter>();
        if (target.pawn.shooter != null) 
        {
			target.pawn.shooter.BoostDamage(strength * -1);
		}
    }
}
