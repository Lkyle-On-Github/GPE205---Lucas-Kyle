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
		/*
		GameObject objNewAI = Instantiate(controller, Vector3.zero, Quaternion.identity) as GameObject;
        GameObject objNewPawn = Instantiate(pawn, this.transform.position, this.transform.rotation) as GameObject;

		//find the controller and pawn components
        Controller compController = objNewAI.GetComponent<Controller>();
        Pawn compPawn = objNewPawn.GetComponent<Pawn>();

		//hook controller to pawn
		compController.pawn = compPawn;
		compPawn.controller = compController;

		Damage damage = objNewPawn.GetComponent<Damage>();
		damage.team = team;
		*/

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
