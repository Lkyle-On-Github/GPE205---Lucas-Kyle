using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnpoint : Spawnpoint
{
    // Start is called before the first frame update
    void Start()
    {
        team = 0;
		pawn = GameManager.inst.preTankPawn;

		base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
