using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
	public List<Powerup> powerups;
	private List<Powerup> removeQueue;
    // Start is called before the first frame update
    void Start()
    {
         powerups = new List<Powerup>();
		 removeQueue = new List<Powerup>();
    }

    // Update is called once per frame
    void Update()
    {
        DecrementPowerupTimers();
    }
	private void LateUpdate()
    {
        RunRemoveQueue();
    }

	public void GainBuff (Powerup toAdd)
	{
		toAdd.Apply(this);
		powerups.Add(toAdd);
	}

	public void LoseBuff (Powerup toRemove)
	{
		powerups.Remove(toRemove);
		toRemove.Remove(this);
	}

	public void DecrementPowerupTimers()
    {
        foreach (Powerup currPowerup in powerups) {
            currPowerup.duration -= Time.deltaTime;
            // If time is up, add to removal queue
            if (currPowerup.duration <= 0) 
			{
                removeQueue.Add(currPowerup);
            }
        }
    }
	public void RunRemoveQueue()
	{
		foreach(Powerup currPowerup in removeQueue)
		{
			LoseBuff(currPowerup);
			powerups.Remove(currPowerup);
		}
		removeQueue.Clear();
	}
}
