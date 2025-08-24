using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIAgressor : AIController
{
	public float investigateDist;
	public float wanderDist;
	public float idleTime;
	public int lowhealth;

	private float idleStartTime;
	private Vector3 wanderPos;
	private bool subStateWander;

	
	public override void Start()
    {
		base.Start();
		StateStart();
		//perhaps I should make this pick the closest player?
        target = GameManager.inst.listPlayers[0].pawn;
		
    }

    // Update is called once per frame
    void Update()
    {
		
		base.Update();
        MakeDecisions();
    }

    protected override void MakeDecisions() 
	{
		switch (state) 
		{
			case States.Idle:
			//state behaviour
				DoIdleState();
			//state change check
				//done in sense check
				break;
			case States.Investigate:
			
			//state change check
				if(DistanceCheck(targetNoisePos,investigateDist))
				{
					if(IsFacing(targetNoisePos, 2.5f))
					{
						SwapState(States.Idle);
					} else
					{
						RotateTowards(targetNoisePos);
					}
				} else
				{
			//state behaviour
					DoInvestigateState();
				}
				break;
			case States.Chase:
			//state behaviour
				if(target != null)
				{
					SeekSmart(target.transform.position);
					Shoot();
				}
				//Shoot();
			//state change check
				if(pawn.health.hp <= lowhealth)
				{
					SwapState(States.Flee);
				}
				break;
			case States.Flee:
			//state behaviour
				if(target != null)
				{
					RotateTowards(target.transform.position);
					MoveBackward();
					Shoot();
				}
			//state change check
				
				break;
		}
	}
	protected override void StateStart() 
	{
		//this is how I write my switch statements it makes it easier for me to read but I can change it if you really need me to
		switch (state)
		{
			case States.Idle:
				EndWander();
				ToggleSenses(true,true);
				break;
			case States.Investigate:
				ToggleSenses(true,true);
				break;
			case States.Chase:
				ToggleSenses(true,false);
				break;
			case States.Flee:
				ToggleSenses(true,false);
				break;
		}
	}
	protected override void StateEnd()
	{
		switch (state)
		{
			case States.Idle:

				break;
		}
	}

	protected virtual void DoIdleState()
	{
		if(!subStateWander)
		{
			if(Time.time > idleTime + idleStartTime) 
			{
				StartNewWander();
			}
		} else
		{
			SeekSmart(wanderPos);
			if(DistanceCheck(wanderPos, wanderDist))
			{
				EndWander();
			}
		}
	}
	protected virtual void DoInvestigateState()
	{
		SeekSmart(targetNoisePos);
	}

	protected virtual void StartNewWander()
	{
		subStateWander = true;
		wanderPos = RandomRoomPos();
	}
	protected virtual void EndWander()
	{
		idleStartTime = Time.time;
		subStateWander = false;
	}

	protected override void OnSenseUpdate()
	{
		if(target != null)
		{
			Debug.Log((target.controller as PlayerController).playerID);
		}
		//Debug.Log("on sense updating!!!");
		switch(state)
		{
			case States.Idle:
				
				if(ChooseVisibleTarget())
				{
					SwapState(States.Chase);
				} else
				{
					if(GetNoiseOfType(GameManager.Noises.Hit))
					{
						SwapState(States.Investigate);
					}
				}
				break;
			case States.Investigate:
				if(ChooseVisibleTarget())
				{
					SwapState(States.Chase);
				} else {
					if(ChooseNoiseByPrio())
					{
						//SwapState(States.Investigate);
					}
				}
				break;
			case States.Chase:
				if(!ChooseVisibleTarget())
				{
					targetNoisePos = lastTargetPos;
					targetNoise = null;
					SwapState(States.Investigate);
				}
				break;
			case States.Flee:
				if(!ChooseVisibleTarget())
				{
						targetNoisePos = lastTargetPos;
						SwapState(States.Investigate);
				}
				break;
		}
	}
}
