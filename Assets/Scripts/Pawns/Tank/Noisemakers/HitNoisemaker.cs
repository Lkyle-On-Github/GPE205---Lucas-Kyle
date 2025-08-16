using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNoisemaker : NoiseMaker
{
    // Start is called before the first frame update
    public override void Start()
    {
		base.Start();
        noise = GameManager.Noises.Hit;
    }
	public override void StartNoise(Vector3 location)
	{
		base.StartNoise(location);
		//prevents noise playing at the same time as death noise, although i dont think that would even happen
		if (GetComponent<Health>().hp > 0)
		{
			audioSource.Play();
		}
	}
}
