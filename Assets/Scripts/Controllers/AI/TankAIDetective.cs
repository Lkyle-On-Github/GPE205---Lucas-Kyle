using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIDetective : AIController
{
	private Vector3 wanderPos;
	public float wanderDist;
	public Pawn target;
	public List<Pawn> ignoreList;
	public float investigateDist;
    // Start is called before the first frame update
    void Start()
    {
		noisePrio.Add(GameManager.Noises.Shot);
		noisePrio.Add(GameManager.Noises.Hit);
		noisePrio.Add(GameManager.Noises.Movement);
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
			//state behaviour
				SeekSmart(wanderPos);
			//state change check
				if(DistanceCheck(wanderPos, wanderDist))
				{
					if(PointVisible(wanderPos))
					{
						SwapState(States.Idle);
					}
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
		if(target != null)
		{
			switch(state)
			{
				case States.Idle:
					for(int i = 0; i < visiblePawns.Count; i++)
					{
						if(visiblePawns[i] == target)
						{
							SwapState(States.Chase);
						}
						else 
						{
							if(!ignoreList.Contains(visiblePawns[i]))
							{
							ignoreList.Add(visiblePawns[i]);
							}
						}
					}
					for(int i = 0; i < audibleNoises.Count; i++)
					{
						if(!ignoreList.Contains(audibleNoises[i].pawn))
						{
							targetNoise = audibleNoises[i];
							targetNoisePos = GetNoisePos(targetNoise);
							SwapState(States.Investigate);
						}
					}
					break;
				case States.Investigate:
					for(int i = 0; i < visiblePawns.Count; i++)
					{
						if(visiblePawns[i] == target)
						{
							SwapState(States.Chase);
						}
						else 
						{
							if(!ignoreList.Contains(visiblePawns[i]))
							{
							ignoreList.Add(visiblePawns[i]);
							}
						}
						
					}
					for(int i = 0; i < audibleNoises.Count; i++)
					{
						if(!ignoreList.Contains(audibleNoises[i].pawn))
						{
							if(CheckNoisePrio(audibleNoises[i]))
							{
								targetNoise = audibleNoises[i];
								targetNoisePos = GetNoisePos(targetNoise);
							}
						}
					}
					break;
				case States.Chase:
					if(!visiblePawns.Contains(target))
					{
						wanderPos = target.transform.position;
						SwapState(States.Follow);
					}
					break;
				case States.Follow:
					for(int i = 0; i < visiblePawns.Count; i++)
					{
						if(visiblePawns[i] == target)
						{
							SwapState(States.Chase);
						}
						else 
						{
							if(!ignoreList.Contains(visiblePawns[i]))
							{
							ignoreList.Add(visiblePawns[i]);
							}
						}
					}
					for(int i = 0; i < audibleNoises.Count; i++)
						{
						if(!ignoreList.Contains(audibleNoises[i].pawn))
						{
							if(audibleNoises[i].noise == GameManager.Noises.Movement)
							{
								targetNoise = audibleNoises[i];
								targetNoisePos = GetNoisePos(targetNoise);
								SwapState(States.Investigate);
							}
						}
					}
					break;
					
			}
		}
	}
}
