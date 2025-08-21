using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Powerup
{
	public float duration;
	//
	public float displayMaxDuration;
	//powerups should have values in their apply and remove functions that are multiplied by strength
	public float strength;
	//handles what happens when you get multiple of the same powerup. 
	public enum StackTypes{Duration, Strength, NewInstance, CantStack};
	public StackTypes stackType;
	public bool isPermanent;
	//returns true if it was consumed as a stack, false if it needs to be added to the list. direct skips all the logic for checking for stacks, and doesnt add it to the buffs list, simply applying the buff.
    public virtual bool Apply(PowerupManager target, bool direct)
	{	
		return true;
	}
	public virtual void Remove(PowerupManager target)
	{

	}
	public virtual bool Stack(PowerupManager target, Powerup newPowerup)
	{
		switch(stackType)
		{
			//Duration stacking takes the higher strength value and adds the new duration
			case StackTypes.Duration:
				AdjustBuff(target, duration + newPowerup.duration, Mathf.Max(strength, newPowerup.strength));
				//add the strength thing
				return true;
				break;
			//Strength stacking takes the higher duration value and adds the new strength
			case StackTypes.Strength:
				AdjustBuff(target, Mathf.Max(duration, newPowerup.duration), strength + newPowerup.strength);
				return true;
				break;
			//returns false, which should tell the powerup to create a new instance of the powerup
			case StackTypes.NewInstance:
				return false;
				break;
			case StackTypes.CantStack:
				return true;
				break;
		}
		return false;
	}

	public virtual void AdjustBuff(PowerupManager target, float newDuration, float newStrength)
	{
		//removes the old instance of the buff
		Remove(target);

		//Adjusts the internal values of the buff
		//Debug.Log(newDuration);
		duration = newDuration;
		//since the bar represents the fraction of remaining time left, if the total amount of time left increases above the previous maximum, the scaling factor of the bar should be adjusted to compensate for that
		if(newDuration > displayMaxDuration)
		{
			displayMaxDuration = newDuration;
		}
		//Debug.Log(newStrength);
		strength = newStrength;


		//Adds the adjusted buff!
		Apply(target, true);

		//bullets already in the air may or may not be affected
		//thats fine
	}


}
