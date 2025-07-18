using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankSpawnpoint : Spawnpoint
{
    // Start is called before the first frame update
    void Start()
    {
        team = 1;
		controller = GameManager.inst.preAIControllerTank;
		pawn = GameManager.inst.preTankPawn;

		base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
