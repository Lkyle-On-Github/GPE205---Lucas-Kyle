using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamagePowerup : Powerup
{
    public float damageBoost;
	//public float preBoostHp;

	public override void Apply(PowerupManager target)
    {
		// Apply Health changes
        Shooter targetShooter = target.GetComponent<Shooter>();
        if (targetShooter != null) 
        {
			targetShooter.BoostDamage(damageBoost);
        }
    }

    public override void Remove(PowerupManager target)
    {
        Shooter targetShooter = target.GetComponent<Shooter>();
        if (targetShooter != null) 
        {
			targetShooter.BoostDamage(damageBoost * -1);
		}
    }
}
