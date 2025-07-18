using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : AIController
{
	public int chaseDistance;
	public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
		//perhaps I should make this pick the closest player?
        target = GameManager.inst.listPlayers[0].pawn.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        MakeDecisions();
    }

	public enum States {Idle, Chase};
	
	//since this should only be changed through the SwapState method, it is private to avoid accidental modification
	public States state = States.Idle;
	public void MakeDecisions() 
	{
		switch (state) 
		{
			case States.Idle:
			//state behaviour
				DoIdleState();
			//state change check
				if(IsTargetInChaseDistance()) 
				{
					SwapState(States.Chase);
				}
				break;
			case States.Chase:
			//state behaviour
				DoChaseState();
			//state change check
				if(!IsTargetInChaseDistance())
				{
					SwapState(States.Idle);
				}
				break;
		}
	}

	public void StateStart() 
	{
		//this is how I write my switch statements it makes it easier for me to read but I can change it if you really need me to
		switch (state)
		{
			case States.Chase:

				break;
		}
	}
	public void StateEnd()
	{
		switch (state)
		{
			case States.Chase:

				break;
		}
	}
	public void SwapState(States newState)
	{
		StateEnd();
		state = newState;
		StateStart();

		lastStateSwapTime = Time.time;
	}

	protected void DoChaseState()
	{
		if(target != null) 
		{
		//UpdateTargetPos();
		pawn.Seek(target);
		}
	}
	protected void DoIdleState()
	{
		
	}

	//I didnt see a reason to create such a generalized function for such a specific task
	protected bool IsTargetInChaseDistance()
    {
        if (Vector3.Distance (pawn.transform.position, target.transform.position) < chaseDistance ) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
}
