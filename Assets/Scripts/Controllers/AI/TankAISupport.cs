using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAISupport : AIController
{
    public Pawn friend;
	public Pawn target;
	public float followDist;
	public float investigateDist;
	public float fleeDist;
	bool friendVisible;
	bool targetVisible;
	Vector3 fleePos;
	AIController friendAI;

	public override void Start()
    {
		base.Start();
		noisePrio.Add(GameManager.Noises.Hit);
		noisePrio.Add(GameManager.Noises.Shot);
		noisePrio.Add(GameManager.Noises.Movement);
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
				//No behaviour
			//state change check
				//done in sense check
				break;
			case States.Investigate:
			//state behaviour
				SeekSmart(targetNoisePos);
				
			//state change check
				if(DistanceCheck(targetNoisePos, investigateDist))
				{
					SwapState(States.Idle);
				}
				break;
			case States.Follow:
			//state behaviour
				if(friend != null)
				{
					targetNoisePos = friend.transform.position;
					if(!DistanceCheck(friend.transform.position, followDist)) 
					{
						SeekSmart(friend.transform.position);
					} else
					{
						if(friendVisible == false)
						{
							SeekSmart(friend.transform.position);
						}
					}
				} else
				{
					//targetNoisePos = friend.transform.position;
					SwapState(States.Investigate);
				}
			//state change check
				
				break;
			case States.Fire:
			//state behaviour
				if(target != null)
				{
					if(targetVisible)
					{
					RotateTowards(target.transform.position);
					Shoot();
					} else
					{
						SeekSmart(target.transform.position);
					}
				}
			//state change check
				//done in sense check
				break;
			case States.Flee:
			//state behaviour
				SeekSmart(fleePos);
			//state change check
				if(DistanceCheck(fleePos, fleeDist))
				{
					SwapState(States.Idle);
				}
				break;
			case States.Celebrate:
			//state behaviour
				SeekSmart(friend.transform.position);
				RotateClockwise();
			//state change check
				//done in sense check
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
			case States.Follow:
				ToggleSenses(true,false);
				break;
			case States.Fire:
				ToggleSenses(true,false);
				break;
			case States.Flee:
				fleePos = RandomMapPos();
				ToggleSenses(true,false);
				break;
			case States.Celebrate:
				ToggleSenses(false,false);
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
			switch (state) 
			{
				case States.Idle:
					for(int i = 0; i < visiblePawns.Count; i++)
					{
						if(visiblePawns[i] == target)
						{
							SwapState(States.Flee);
						} else 
						{
							friend = visiblePawns[i];
							friendAI = friend.controller.GetComponent<AIController>();
							SwapState(States.Follow);
						}
					}
					for(int i = 0; i < audibleNoises.Count; i++)
					{
						if(friend == null)
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
							SwapState(States.Flee);
						} else 
						{
							friend = visiblePawns[i];
							friendAI = friend.controller.GetComponent<AIController>();
							SwapState(States.Follow);
						}
						
					}
					for(int i = 0; i < audibleNoises.Count; i++)
					{
						if(CheckNoisePrio(audibleNoises[i]))
						{
							targetNoise = audibleNoises[i];
							targetNoisePos = GetNoisePos(targetNoise);
						}
					}
					break;
				case States.Follow:
					//if friend can see target
					if(friendAI.visiblePawns.Contains(target))
					{
						SwapState(States.Fire);
					}
					//if it can see friend
					if(visiblePawns.Contains(friend))
					{
						friendVisible = true;
					}else 
					{
						//Debug.Log("where frend I cant find :(((");
						friendVisible = false;
					}
					break;
				case States.Fire:
					//if friend can not see target
					if(!friendAI.visiblePawns.Contains(target))
					{
						SwapState(States.Follow);
					}
					//if it can see target
					if(visiblePawns.Contains(target))
					{
						targetVisible = true;
					} else
					{
						targetVisible = false;
					}
					break;
				case States.Flee:
				for(int i = 0; i < visiblePawns.Count; i++)
					{
						if(visiblePawns[i] != target)
						{
							friend = visiblePawns[i];
							friendAI = friend.controller.GetComponent<AIController>();
							SwapState(States.Follow);
						}
					}
					break;
				case States.Celebrate:
				
					break;
				}
				
		}
	}
	
}
