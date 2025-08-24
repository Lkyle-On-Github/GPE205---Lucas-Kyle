using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAISupport : AIController
{
    public Pawn friend;
	public float followDist;
	public float investigateDist;
	public float fleeDist;
	public float wanderDist;
	bool friendVisible;
	bool targetVisible;
	Vector3 fleePos;
	AIController friendAI;
	public bool friendSeesTarget;
	public float idleTime;
	private float idleStartTime;
	private Vector3 wanderPos;
	private bool subStateWander;

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
		CelebrateCheck();
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
						} else 
						{
							if(friendSeesTarget && target != null)
							{
								RotateTowards(target.transform.position);
								MoveForward();
							}
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
					RotateTowards(target.transform.position);
					Shoot();
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
				if(friend != null)
				{
					SeekSmart(friend.transform.position);
					RotateClockwise();
				}
			//state change check
				//This party lasts FOREVEEEEEER!!!! WOOOOOOOOOOO
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
		
		switch (state) 
		{
			case States.Idle:
				if(CanSeeFriend())
				{
					SwapState(States.Follow);
				} else
				{
					if(ChooseVisibleTarget())
					{
						SwapState(States.Flee);
					} else
					{
						FilterSupportNoises();
						if(ChooseNoiseByPrio())
						{
							SwapState(States.Investigate);
						}
					}
				}
				break;
			case States.Investigate:
				if(CanSeeFriend())
				{
					SwapState(States.Follow);
				} else
				{
					if(ChooseVisibleTarget())
					{
						SwapState(States.Flee);
					} else
					{
						FilterSupportNoises();
						if(ChooseNoiseByPrio())
						{
							//SwapState(States.Investigate);
						}
					}
				}
				break;
			case States.Follow:
				if(CheckFriendDead())
				{
					SwapState(States.Idle);
				} else
				{
					//if friend can see target
					if(friendAI.target != null)
					{
						friendSeesTarget = true;
						target = friendAI.target;
						//lets it look for the pawn unconstrained by its normal view angle
						//also if it is close enough to friend to not move, just give it the player position
						if(PawnVisible(friendAI.target))
						{
							target = friendAI.target;
							SwapState(States.Fire);
						}
					} else
					{
						friendSeesTarget = false;
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
				}
				break;
			case States.Fire:
				if(CheckFriendDead())
				{
					SwapState(States.Idle);
				} else
				{
					//if friend can not see target
					if(friendAI.target == null)
					{
						//if it can see target and friend, continue attacking, if not friend, flee
						if(target != null && PawnVisible(target))
						{
							//I had to split this because my brain was having trouble parsing it
							if(visiblePawns.Contains(friend))
							{

							} else
							{
								SwapState(States.Flee);
							}
						} else
						{
							//if friend can still see pawn, just try to find it, otherwise, return to follow
							target = null;
							SwapState(States.Follow);
						}
					} else
					//if it can see target
					if(visiblePawns.Contains(target))
					{
						targetVisible = true;
					} else
					{
						targetVisible = false;
					}
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
	public bool CanSeeFriend()
	{
		FilterSupportPawns();
		if(friend == null)
		{
			//if it can see a pawn that isnt the player, assign its friend and return true
			foreach(Pawn pawn in visiblePawns)
			{
				if(!GameManager.inst.listPlayers.Contains(pawn.controller as PlayerController) && pawn.controller as TankAISupport == null)
				{
					friend = pawn;
					friendAI = friend.controller.GetComponent<AIController>();
					return true;
				}
			}
		} else
		{
			if(visiblePawns.Contains(friend))
			{
				return true;
			}
			//return false;
		}
		//if friend is not visible and no new friend can be found, return false
		return false;
	}

	public bool CheckFriendDead()
	{
		if(friend == null)
		{
			friendAI = null;
			return true;
		}
		return false;
	}
	public void CelebrateCheck()
	{
		if(GameManager.inst.listPlayers.Count == 0)
		{
			SwapState(States.Celebrate);
		}
	}
	public void FilterSupportPawns()
	{
		List<Pawn> toRemove = new List<Pawn>();
		foreach(Pawn pawn in visiblePawns)
		{
			if(pawn.controller as TankAISupport != null)
			{
				toRemove.Add(pawn);
			}
		}
		foreach(Pawn pawn in toRemove)
		{
			visiblePawns.Remove(pawn);
		}
	}
	public void FilterSupportNoises()
	{
		List<NoiseMaker> toRemove = new List<NoiseMaker>();
		foreach(NoiseMaker noiseMaker in audibleNoises)
		{
			if(noiseMaker.pawn.controller as TankAISupport != null)
			{
				toRemove.Add(noiseMaker);
			}
		}
		foreach(NoiseMaker noiseMaker in toRemove)
		{
			audibleNoises.Remove(noiseMaker);
		}
	}
}
