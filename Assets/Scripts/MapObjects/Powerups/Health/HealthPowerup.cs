using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthPowerup : Powerup
{
	
	public float hpBoost;
	//I think its dumb when health boosts remove the health they gave you when they run out, because then they didnt even do anything, 
	//so im making it so that the health is only removed if it wasnt used
	public float preBoostHp;

	public override void Apply(PowerupManager target)
    {
		// Apply Health changes
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null) 
        {
			preBoostHp = targetHealth.hp;
            targetHealth.TakeHealing(hpBoost); 

        }
    }

    public override void Remove(PowerupManager target)
    {
        // TODO: Remove Health changes
		//pretty sure it will cause an error if I dont write this here even though it isnt neccessary
		Health targetHealth = target.GetComponent<Health>();
		//if some of the health from the boost wasn't used
		if(targetHealth.hp > preBoostHp)
		{
			//subtract the amount of the health boost
			targetHealth.hp -= hpBoost;
			//correct health value if it accidentally subtracted too much
			targetHealth.hp = Mathf.Clamp(targetHealth.hp, preBoostHp, targetHealth.maxHp);
			//even though this shouldnt be able to kill them like who knows idk
			if(targetHealth.hp <= 0) 
			{
				targetHealth.Die();
			}
		}
    }
	
	
}
