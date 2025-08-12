using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
The difference between Enemy Types is their FSM - (States, Conditions, Connections.) 
ALL "Actions" and "Senses" are contained in the parent AIController class.
*/

public class AIController : Controller
{
	public float hearDistance;
	public float viewAngle;
	public float viewDistance;
	//use ToggleSenses unless you want to keep the stale references
	public bool canSee;
	public bool canHear;
	//accessable to developers incase they want to make a less important AI take up less resources
	public float senseUpdateInterval;
	public float seekDist;
	private float lastSenseUpdate;

	public List<NoiseMaker> audibleNoises;
	//used in check noise prio, noises further to the left in the list (lower index number) have a higher priority. Noises this AI ignores can be left out of the prio list, but you dont have to use noisePrio
	public List<GameManager.Noises> noisePrio;
	public List<Pawn> visiblePawns;//

	public NoiseMaker targetNoise;
	public Vector3 targetNoisePos;


	public enum TurnSetting {None, Clockwise, CounterClockwise};
	public float moveCheckDist;
	public float shortCheckDist;
	//direction simply used to reduce the number of raycast checks
	public TurnSetting turnDir;
	//used to help it navigate around large obstacles
	public TurnSetting navDir;
	public float navResetTime;
	private float lastNavTrigger;
	private float lastStuckCheckTime;
	private Vector3 stuckCheckPos;

    // Start is called before the first frame update
    public override void Start()
    {
		
        base.Start();
		state = States.Idle;
		StateStart();
    }

    // Update is called once per frame
    public override void Update()
    {
        //MakeDecisions();
		base.Update();

        //this only even reduces lag if they all start on different frames right? otherwise it would just be stuttering? I mean I dont really know how lag works so whatever
		RunSenses();

		if(lastNavTrigger + navResetTime < Time.time)
		{
			navDir = TurnSetting.None;
		}
		
		//Debug.Log(pawn.transform.forward);
    }

    //public void MakeDecisions()
    //{
    //    Debug.Log("Making Decisions");
    //}

	
	public float lastStateSwapTime;
	
	public enum States {Idle, Chase, Flee, Patrol, ChooseTarget, Investigate, Guard, Follow, Fire, SpotPlayer, Celebrate};
	
	//since this should only be changed through the SwapState method, it is private to avoid accidental modification
	public States state;

	

	protected virtual void MakeDecisions() 
	{
		switch (state) 
		{
			case States.Idle:

				break;
		}
	}

	protected virtual void StateStart() 
	{
		//this is how I write my switch statements it makes it easier for me to read but I can change it if you really need me to
		switch (state)
		{
			case States.Idle:

				break;
		}
	}
	protected virtual void StateEnd()
	{
		switch (state)
		{
			case States.Idle:

				break;
		}
	}
	protected virtual void SwapState(States newState)
	{
		if(state != newState)
		{
		StateEnd();
		state = newState;
		StateStart();

		lastStateSwapTime = Time.time;
		}
	}

	protected virtual void Hearing()
	{
		audibleNoises.Clear();
		//iterates through the global array of active noises
		for(int i = 0; i < GameManager.inst.activeNoises.Count; i++)
		{
			if(GameManager.inst.activeNoises[i].pawn != pawn)
			{
				if(Vector3.Distance(transform.position, GameManager.inst.activeNoises[i].noiseLocation) < (hearDistance + GameManager.inst.activeNoises[i].volumeDistance))
				{
					//adds it to the local array of noises this instance can hear
					if(GameManager.inst.activeNoises[i].active == true)  
					{
						audibleNoises.Add(GameManager.inst.activeNoises[i]);
					}
				}
			}
		}

	}
	//this code could cause some lag I think. hopefully its fine
	protected virtual void Seeing()
	{
		visiblePawns.Clear();
		List<Pawn> listOfPawns = GameManager.inst.listPawns;
		//removes itself from array first
		listOfPawns.Remove(pawn);
		for(int i = 0; i < listOfPawns.Count; i++)
		{
			/*
			if(listOfPawns[i] == pawn) 
			{
				listOfPawns.RemoveAt(i);
			}
			*/
			Vector3 pawnVector = listOfPawns[i].transform.position - pawn.transform.position;
			float pawnAngle = Vector3.Angle(pawnVector, pawn.transform.forward);
			RaycastHit hitInfo;
			if(Physics.Raycast(pawn.transform.position, pawnVector, out hitInfo, viewDistance, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal));
			{
				//can not see through other tanks unfortunately
				if(hitInfo.transform.gameObject.GetComponent<Pawn>() == listOfPawns[i])
					//pawns on the same team can be seen at any angle, this should hopefully make them easier to track
					if(listOfPawns[i].damage.team == pawn.damage.team)
					{
						visiblePawns.Add(listOfPawns[i]);

					} else
					{
						//pawns on the other teams must be in the view angle
						if(pawnAngle < viewAngle)
						{
							//adds it to the local array of pawns
							visiblePawns.Add(listOfPawns[i]);
						}

					}
			}
		}
		
	}
	protected virtual void RunSenses()
	{
		if(lastSenseUpdate + senseUpdateInterval <= Time.time)
		{
			if(canSee)
			{
				Seeing();
			}
			if(canHear)
			{
				Hearing();
			}
			lastSenseUpdate = Time.time;
			OnSenseUpdate();
		}
	}

	protected virtual void ToggleSenses(bool seeing, bool hearing)
	{
		//makes sure that I dont clear arrays erroniously if swapping between two different sensing states
		if(canSee != seeing)
		{
			visiblePawns.Clear();
			canSee = seeing;
		}
		if(canHear != hearing)
		{
			audibleNoises.Clear();
			canHear = hearing;
		}
		
	}

	protected bool DistanceCheck(Vector3 checkTarget, float checkDist)
    {
		if (Vector3.Distance(pawn.transform.position, checkTarget) < checkDist) 
		{
			{
				return true;
			}
		}
        return false;
    }

	protected virtual bool CheckNoisePrio(NoiseMaker noise)
	{
		if (noisePrio.IndexOf(noise.noise) < noisePrio.IndexOf(targetNoise.noise))
		{
			return true;
		}
		return false;
	}

	//this prob shouldnt exist?
	protected virtual Vector3 GetNoisePos(NoiseMaker noise) 
	{
		return noise.GetNoisePos();
	} 

	protected virtual void MoveForward()
	{
		pawn.MoveForward();
	}
	protected virtual void MoveBackward()
	{
		pawn.MoveBackward();
	}

	protected virtual void RotateTowards(Vector3 rotatePos)
	{
		pawn.RotateTowardsPoint(rotatePos);
	}

	protected virtual void RotateClockwise()
	{
		pawn.RotateClockwise();
	}
	protected virtual void Shoot()
	{
		pawn.Shoot();
	}

	protected virtual void SeekPoint(Vector3 seekPos)
	{
		pawn.Seek(seekPos);
	}
	protected virtual void SeekPoint(GameObject seekObj)
	{
		pawn.Seek(seekObj);
	}

	//completely rewrote it and it works great now!
	protected virtual void SeekSmart(Vector3 seekPos)
	{
		Quaternion cachedQuat = pawn.transform.rotation;
		Vector3 cachedForward = pawn.transform.forward;
		Vector3 cachedPos = pawn.transform.position;
		pawn.RotateTowardsPoint(seekPos);
		
		RaycastHit hitInfo;
		//initial rayCast check
		bool didHit = Physics.Raycast(pawn.transform.position, pawn.transform.forward, out hitInfo, moveCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
		switch(turnDir)
		{
			case TurnSetting.None:
				//Debug.Log(hitInfo.transform.gameObject);
			
				if(DidHitWall(didHit, hitInfo))
				{
					//it might be turning in to a wall, check if the original rotation would be turning in to a wall
					
					didHit = Physics.Raycast(pawn.transform.position, cachedForward, out hitInfo, moveCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
					if(DidHitWall(didHit, hitInfo)) 
					{
						//why is the code reaching here while it is out in the open
						//Debug.Log("didhitwall!!!");
						pawn.transform.rotation = cachedQuat;
						//this means it isnt turning towards a wall, it is just facing a wall. decide which direction to turn to face away from the wall.
						//if it already has its navDir set, use that
						switch(navDir)
						{
							case TurnSetting.None:
								Vector3 filteredTargetPos = new Vector3(seekPos.x, this.transform.position.y, seekPos.z);
								//find the vector and rotation to target
								Vector3 vectorToTarget = filteredTargetPos - transform.position;
								Quaternion rotToTarget = Quaternion.LookRotation(vectorToTarget, Vector3.up);
								//So I did some testing and thinking and I believe the way it works is that , if the target y rot is bigger, go clockwise, smaller go counterclockwise, and if the difference is > 180, flip the answer
								//the only problem with this is that if the AI's rotation is set outside of the -180 to 180 range this wont work, but I just wont do that so its fine!
								if(pawn.transform.rotation.y - rotToTarget.y > 0)
								{
									turnDir = TurnSetting.Clockwise;
									navDir = TurnSetting.Clockwise;
									lastNavTrigger = Time.time;
								} else
								{
									turnDir = TurnSetting.CounterClockwise;
									navDir = TurnSetting.CounterClockwise;
									lastNavTrigger = Time.time;
								}
								if(pawn.transform.rotation.y - rotToTarget.y > 180 || pawn.transform.rotation.y - rotToTarget.y < -180) 
								{
									//chosenDir = !chosenDir; 
									//damn it
									if(turnDir == TurnSetting.Clockwise) 
									{
										turnDir = TurnSetting.CounterClockwise;
									} else 
									{
										turnDir = TurnSetting.Clockwise;
									}
								}
								break;
							case TurnSetting.Clockwise:
								//if it was trailing a wall, and its about to turn into a wall, that means it hit a corner! reverse turning direction

								turnDir = TurnSetting.Clockwise;
								//if the wall in front is very close, this check is to prevent it from seeing doorways as corners
								didHit = Physics.Raycast(pawn.transform.position, pawn.leftAngle.transform.forward, out hitInfo, moveCheckDist * 1.1f, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
								if(DidHitWall(didHit, hitInfo))
								{
									//if it is perpendicular to a wall that is in its turning direction
									didHit = Physics.Raycast(pawn.transform.position, pawn.rightSide.transform.forward, out hitInfo, shortCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);	
									if(DidHitWall(didHit,hitInfo))
									{
										turnDir = TurnSetting.CounterClockwise;
										//pawn.RotateCounterClockwise();
									}
								}
								break;
							case TurnSetting.CounterClockwise:
								turnDir = TurnSetting.CounterClockwise;
								didHit = Physics.Raycast(pawn.transform.position, pawn.rightAngle.transform.forward, out hitInfo, moveCheckDist * 1.1f, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
								if(DidHitWall(didHit, hitInfo))
								{
									didHit = Physics.Raycast(pawn.transform.position, pawn.leftSide.transform.forward, out hitInfo, shortCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);	
									if(DidHitWall(didHit,hitInfo))
									{
										turnDir = TurnSetting.Clockwise;
										//pawn.RotateClockwise();
									}
								}
								break;
						}
					} else
					{
						//this means it is turning in to a wall, so it should just turn back
						if(Vector3.SignedAngle(cachedForward, pawn.transform.forward, Vector3.up) > 0)
						{
							navDir = TurnSetting.Clockwise;
							lastNavTrigger = Time.time;
						} else
						{
							navDir = TurnSetting.CounterClockwise;
							lastNavTrigger = Time.time;
						}

						pawn.transform.rotation = cachedQuat;
						//set navDir to the direction of this turn
						//we dont have the direction of this turn :(((
						
					}
				} else
				{
					//movement was uninterrupted!!
					
				}
				break;
			case TurnSetting.Clockwise:
				if(DidHitWall(didHit, hitInfo))
				{
					//it still needs to be turning, continue in chosen rotate direction
					pawn.transform.rotation = cachedQuat;
					pawn.RotateClockwise();
				} else
				{
					//it doesnt need to be turning anymore, return to normal movement
					//Debug.Log("didnt hit wall!");
					turnDir = TurnSetting.None;
				}
				break;
			case TurnSetting.CounterClockwise:
				if(DidHitWall(didHit, hitInfo))
				{
					//it still needs to be turning, continue in chosen rotate direction
					pawn.transform.rotation = cachedQuat;
					pawn.RotateCounterClockwise();
				} else
				{
					//it doesnt need to be turning anymore, return to normal movement
					//Debug.Log("didnt hit wall!");
					turnDir = TurnSetting.None;
				}
				break;

		}
		//move forward regardless of everything
		pawn.MoveForward();
		//fix for getting stuck on corners
		DoStuckCheck();
		
	}
	protected virtual bool DidHitWall(bool didHit, RaycastHit hitInfo)
	{
		if(didHit)
		{
			//this should be impossible but it appears to be whats causing the no turning bug
			if(hitInfo.transform.gameObject != null)
			{
				if(hitInfo.transform.gameObject.GetComponent<Pawn>() == null)
				{
					return true;
				} else 
				{
					return false;
				}
			} else
			{
				return false;
			}
		} else
		{
			return false;
		}
	}

	protected virtual void DoStuckCheck()
	{
		if(lastStuckCheckTime + 1 < Time.time)
		{
			if(Vector3.Distance(pawn.transform.position, stuckCheckPos) < 0.001f && turnDir == TurnSetting.None)
			{
				Debug.Log("unstucking!");
				pawn.RotateClockwise();
			}
			stuckCheckPos = pawn.transform.position;
			lastStuckCheckTime = Time.time;
		}
	}



	protected virtual Vector3 RandomRoomPos()
	{
		//get a valid position in the pawn's current room
		int randomX = UnityEngine.Random.Range(-20,20) + pawn.roomLocation.x * 50;
		int randomZ = UnityEngine.Random.Range(-20,20) + pawn.roomLocation.z * 50;
		//return
		return new Vector3(randomX, pawn.transform.position.y, randomZ);
	}

	protected virtual Vector3 RandomMapPos()
	{
		//return a random room from roomList
		Room randomRoom = GameManager.inst.listRooms[UnityEngine.Random.Range(0, GameManager.inst.listRooms.Count)];
		//get a random valid position in the room
		int randomX = UnityEngine.Random.Range(-20,20) + randomRoom.x * 50;
		int randomZ = UnityEngine.Random.Range(-20,20) + randomRoom.z * 50;
		//return
		return new Vector3(randomX, pawn.transform.position.y, randomZ);
	}

	//code that the AI will execute on every sense update
	protected virtual void OnSenseUpdate()
	{

	}

	protected virtual bool PointVisible(Vector3 point)
	{
		RaycastHit hitInfo;
		Vector3 toPoint = point - pawn.transform.position;
		float pointDist = Vector3.Distance(point, pawn.transform.position);
		//if it hit nothing
		return !Physics.Raycast(pawn.transform.position, toPoint, out hitInfo, pointDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
			
	}
}
//TO DO
//make sure pawns can't hear themselves