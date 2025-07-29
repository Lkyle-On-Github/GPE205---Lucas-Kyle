using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : AIController
{
	public float chaseDistance;
	public float fleeDistance;
	public float lowHealth;
	public GameObject target;
	public GameObject testOrb;

    // Start is called before the first frame update
    void Start()
    {
		StateStart();
		//perhaps I should make this pick the closest player?
        target = GameManager.inst.listPlayers[0].pawn.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
		base.Update();
        MakeDecisions();
    }

	
	protected void MakeDecisions() 
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
				GetScared();
				break;
			case States.Chase:
			//state behaviour
				DoChaseState();
			//state change check
				if(!IsTargetInChaseDistance())
				{
					SwapState(States.Idle);
				}
				GetScared();
				break;
			case States.Flee:
			//state behaviour
				//Debug.Log(Vector3.Distance(pawn.transform.position, target.transform.position));
				DoFleeState();
			//state change check
				//not for now
				break;
		}
	}

	protected virtual void StateStart() 
	{
		//this is how I write my switch statements it makes it easier for me to read but I can change it if you really need me to
		switch (state)
		{
			case States.Idle:
				ToggleSenses(true,true);
				break;
		}
	}
	protected virtual void StateEnd()
	{
		switch (state)
		{
			case States.Idle:
				ToggleSenses(false,false);
				break;
		}
	}
	
	protected void SwapState(States newState)
	{
		StateEnd();
		state = newState;
		StateStart();

		lastStateSwapTime = Time.time;
	}

//STATE FUNCTIONS
	protected void DoChaseState()
	{
		if(target != null) 
		{
		//UpdateTargetPos();
		SeekSmart(target.transform.position);
		//pawn.Shoot();
		}
	}
	protected void DoIdleState()
	{
		
	}


//I thought I would include how I made and executed this plan so that in case you didnt like how I did it maybe you would respect the work I did and decide its fine.
	//the local position of the target from the AI should be the direction and magnitude of the target if I understand vectors correctly
	//so, if I invert that vector, localize it to the target and then set its magnitude to the flee distance, that should work fine?
	//Steps
	/*
	variable calc target pos

	target location - my location = localized target vector

	*-1 = flipped

	calc target pos.Normalize = normalized

	*flee distance = set magnitude

	+ target pos = localized to target

	this works I think
	*/
	protected void DoFleeState()
	{
		if(target != null) 
		{
			if (Vector3.Distance(pawn.transform.position, target.transform.position) <= fleeDistance) 
			{
			//target location - my location = localized target vector
			Vector3 calcTargetPos = target.transform.position - pawn.transform.position;
			//*-1 = flipped, calc target pos.normalized = normalized
			calcTargetPos = calcTargetPos.normalized * -1;
			//*flee distance = set magnitude
			calcTargetPos = calcTargetPos * fleeDistance;
			//+ target pos = localized to target
			calcTargetPos += target.transform.position;
			SeekSmart(calcTargetPos);
			}
		}

	}

//STATE SWAP FUNCTIONS
	//I didnt see a reason to create such a generalized function for such a specific task
	protected bool IsTargetInChaseDistance()
    {
		if(target != null)
		{
			if (Vector3.Distance (pawn.transform.position, target.transform.position) < chaseDistance ) 
			{
				return true;
			}
		}
        return false;
    }
	//this could be one function but I felt like it was breaking conventions or smth
	protected void GetScared() 
	{
		if(IsScared())
		{
			SwapState(States.Flee);
		}
	}
	protected bool IsScared()
	{
		if(pawn.health.hp <= lowHealth)
		{
			return true;
		}
		return false;
	}

	
}
