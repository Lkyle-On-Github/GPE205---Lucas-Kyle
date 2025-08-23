using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIDetective : AIController
{
	private Vector3 wanderPos;
	public float wanderDist;
	public List<Pawn> ignoreList;
	public float investigateDist;
    // Start is called before the first frame update
    void Start()
    {
		base.Start();
		StateStart();
		ignoreList.Add(pawn);
		//perhaps I should make this pick the closest player?
        target = GameManager.inst.listPlayers[0].pawn;
		//
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
				SeekSmart(wanderPos);
				if(DistanceCheck(wanderPos, wanderDist))
				{
					wanderPos = RandomMapPos();
				}
			//state change check
				//in sense check.
				break;
			case States.Investigate:
			//state behaviour
				SeekSmart(targetNoisePos);
				if(DistanceCheck(targetNoisePos, investigateDist))
				{
					if(PointVisible(targetNoisePos))
					{
					wanderPos = RandomMapPos();
					SwapState(States.Idle);
					}
				}
			//state change check
				//in sense check.				
				break;
			case States.Chase:
			//state behaviour
				if(target != null)
				{
					SeekSmart(target.transform.position);
					Shoot();
				}		
			//state change check
				//in sense check.
				break;
			case States.Follow:
			
			//state change check
				if(DistanceCheck(wanderPos, wanderDist) && PointVisible(wanderPos))
				{
					if(IsFacing(wanderPos, 0.5f))
					{
						SwapState(States.Idle);
					} else
					{
						RotateTowards(wanderPos);
					}
				} else
				{
				//state behaviour
					SeekSmart(wanderPos);
				}
				break;
		}
	}
	protected override void StateStart() 
	{
		//this is how I write my switch statements it makes it easier for me to read but I can change it if you really need me to
		switch (state)
		{
			case States.Idle:
				
				ToggleSenses(true,true);
				break;
			case States.Investigate:
				ToggleSenses(true,true);
				break;
			case States.Chase:
				ToggleSenses(true,false);
				break;
			case States.Follow:
				wanderPos = lastTargetPos;
				ToggleSenses(true,true);
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

	protected override void OnSenseUpdate()
	{
		CleanIgnoreList();
		switch(state)
		{
			case States.Idle:
				if(ChooseCustomTarget(DetectiveSight()))
				{
					SwapState(States.Chase);
				} else
				{
					if(ChooseCustomNoiseByPrio(DetectiveHearing()))
					{
						SwapState(States.Investigate);
					}
				}
				
				break;
			case States.Investigate:
				if(ChooseCustomTarget(DetectiveSight()))
				{
					SwapState(States.Chase);
				} else
				{
					if(ChooseCustomNoiseByPrio(DetectiveHearing()))
					{
						//SwapState(States.Investigate);
					}
				}
				break;
			case States.Chase:
				if(!ChooseCustomTarget(DetectiveSight()))
				{
					SwapState(States.Follow);
				}
				/*
				might re-enable if follow isnt very effective
					else
				{
					if(ChooseCustomNoiseByPrio(DetectiveHearing()))
					{
						SwapState(States.Investigate);
					}
				}
				*/
				break;
			case States.Follow:
				if(ChooseCustomTarget(DetectiveSight()))
				{
					SwapState(States.Chase);
				} else
				{
					if(ChooseCustomNoiseByPrio(DetectiveHearing()))
					{
						SwapState(States.Investigate);
					}
				}
				break;
		}
	}
	public List<Pawn> DetectiveSight()
	{
		//reset list
		List<Pawn> returnList = new List<Pawn>();
		foreach(Pawn pawn in visiblePawns)
		{
			//filters visible pawns by if they are players, adds non player pawns to ignore list.
			if(GameManager.inst.listPlayers.Contains(pawn.controller as PlayerController))
			{
				returnList.Add(pawn);
			}
			else 
			{
				if(!ignoreList.Contains(pawn))
				{
					ignoreList.Add(pawn);
				}
			}
		}
		return returnList;
	}
	public void CleanIgnoreList()
	{
		foreach (Pawn pawn in ignoreList)
		{
			if(pawn == null)
			{
				ignoreList.Remove(pawn);
			}
		}
	}
	public List<NoiseMaker> DetectiveHearing()
	{
		//clears target noise from the list if it is on ignore list
		if(targetNoise != null && ignoreList.Contains(targetNoise.pawn))
		{
			targetNoise = null;
		}
		//filters audible noises by the ignore list.
		List<NoiseMaker> returnList = new List<NoiseMaker>();
		foreach(NoiseMaker noiseMaker in audibleNoises)
		{

			if(!ignoreList.Contains(noiseMaker.pawn))
			{
				returnList.Add(noiseMaker);
			}
		}
		return returnList;
	}
}
