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

	public Pawn target;
	void Start()
    {
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
			//state behaviour
				DoInvestigateState();
			//state change check
				if(DistanceCheck(targetNoisePos,investigateDist))
				{
					if(PointVisible(targetNoisePos))
					{
						SwapState(States.Idle);
					}
				}
				break;
			case States.Chase:
			//state behaviour
				if(target != null)
				{
					SeekSmart(target.transform.position);
				}
				pawn.Shoot();
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
					pawn.Shoot();
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
				ToggleSenses(true,false);
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
		wanderPos = RandomMapPos();
	}
	protected virtual void EndWander()
	{
		idleStartTime = Time.time;
		subStateWander = false;
	}

	protected override void OnSenseUpdate()
	{
		//Debug.Log("on sense updating!!!");
		switch(state)
		{
			case States.Idle:
				for(int i = 0; i < audibleNoises.Count; i++)
				{
					if(audibleNoises[i].noise == GameManager.Noises.Shot)
					{
						targetNoise = audibleNoises[i];
						targetNoisePos = GetNoisePos(targetNoise);
						SwapState(States.Investigate);
					}
				}
				if(target != null)
				{
					if(visiblePawns.Contains(target))
					{
						SwapState(States.Chase);
					}
				}
				/*
				for (int i = 0; i < visiblePawns.Count; i++)
				{
					if(visiblePawns[i] == target) 
					{
						SwapState(States.Chase);
					}
				}
				*/
				break;
			case States.Investigate:
				if(target != null)
				{
					if(visiblePawns.Contains(target))
					{
						SwapState(States.Chase);
					}
				}
					break;
			case States.Chase:
				if(target != null)
				{
					if(!visiblePawns.Contains(target))
					{
						targetNoisePos = target.transform.position;
						SwapState(States.Investigate);
					}
				}
				break;
			case States.Flee:
				if(target != null)
				{
					if(!visiblePawns.Contains(target))
					{
						targetNoisePos = target.transform.position;
						SwapState(States.Investigate);
					}
				}
				break;
		}
	}
}
