using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using inst;

public class NoiseMaker : MonoBehaviour
{
	public float volumeDistance;

	//if we are making it a rule that the AI can only have the information a human in its shoes would have, then it wouldnt know that the player is the one who made a noise
	//however, it would know the type of noise made, or what the noise "sounds like"
	//noises enum is in the GameManager
	//explosion will go unused but it would be for bullets hitting walls or tanks dying
	public GameManager.Noises noise;
	public bool active;

	//the number of seconds the noise lasts
	public float maxNoiseTime;
	//the number of seconds left
	public float noiseTime;
	//the location the noise plays
	public Vector3 noiseLocation;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(active == true)
		{
			if (noiseTime > 0)
			{
				noiseTime -= Time.deltaTime;
				noiseTime = Mathf.Clamp(noiseTime, 0, maxNoiseTime);
			} 
			else
			{
				//Debug.Log("Noise ending!");
				active = false;
				if (GameManager.inst.activeNoises != null) 
				{
					GameManager.inst.activeNoises.Remove(this);
				}
			}
		}
    }

	public virtual Vector3 GetNoisePos()
	{
		if(noiseLocation == null)
		{
			return transform.position;
		}
		return noiseLocation;
	}
	public virtual void StartNoise(Vector3 location)
	{
		//Debug.Log(noise);
		noiseLocation = location; 
		noiseTime = maxNoiseTime;
		if (GameManager.inst.activeNoises != null && active == false) 
		{
				GameManager.inst.activeNoises.Add(this);
		}
		active = true;
		//Debug.Log(active);
		
	}
}
