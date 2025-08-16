using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
	/*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	*/
	public override bool FireTankBullet(int team)
	{
		if(base.FireTankBullet(team))
		{
			pawn.MakeNoise(GameManager.Noises.Shot);
			return true;
		}
		return false;
	}
}
