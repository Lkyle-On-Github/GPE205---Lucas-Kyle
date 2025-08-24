using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefenderSpawnpoint : EnemyTankSpawnpoint
{
	//public GameObject controllerDefender;
	public List<Vector3> patrolPoints;
    // Start is called before the first frame update
    void Start()
    {
        team = 1;
		pawn = GameManager.inst.preTankPawn;

		base.Start();
    }

	public override GameObject Spawn()
	{

		GameObject objNewAI = base.Spawn();
		TankAIDefender AI = objNewAI.GetComponent<TankAIDefender>();
		//objNewAI;
		//patrolPoints.CopyTo(AI.patrolPoints);
		//just isnt working for no reason
		for(int i = 0; i < patrolPoints.Count; i++)
		{
			AI.patrolPoints.Add(patrolPoints[i]);
		}
		return objNewAI;
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
