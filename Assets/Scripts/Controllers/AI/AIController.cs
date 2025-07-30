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
	public List<Pawn> visiblePawns;

	public NoiseMaker targetNoise;
	public Vector3 targetNoisePos;


	public enum TurnSetting {None, Clockwise, CounterClockwise};
	public float moveCheckDist;
	public TurnSetting chosenDir;
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

		
    }

    //public void MakeDecisions()
    //{
    //    Debug.Log("Making Decisions");
    //}

	
	public float lastStateSwapTime;
	
	public enum States {Idle, Chase, Flee, Patrol, ChooseTarget, Investigate, Guard, Follow, Fire, SpotPlayer};
	
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
			if(Vector3.Distance(transform.position, GameManager.inst.activeNoises[i].noiseLocation) < (hearDistance + GameManager.inst.activeNoises[i].volumeDistance))
			{
				//adds it to the local array of noises this instance can hear
				if(GameManager.inst.activeNoises[i].active = true)
				{
					audibleNoises.Add(GameManager.inst.activeNoises[i]);
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

	//the big mistake I made with this code was forgetting what turnsetting was and trying to use it as a toggle for what side of the AI the wall is on when that was not its intended purpose, and im not really sure what to do about it except starting over.
	protected virtual void SeekSmart(Vector3 seekPos)
	{
		//cache the current facing direction
		Quaternion cachedQuat = pawn.transform.rotation;
		RaycastHit hitInfoLeft;
		RaycastHit hitInfoRight;
		RaycastHit hitInfo;
		//rotate the pawn
		pawn.RotateTowardsPoint(seekPos);

		Vector3 calcPawnPos = pawn.transform.position;
		
		//I think this is unused
		switch(chosenDir)
						{
							case TurnSetting.Clockwise:
								calcPawnPos = pawn.leftSide.transform.position;
								break;
							case TurnSetting.CounterClockwise:
								calcPawnPos = pawn.rightSide.transform.position;
								break;
						}
		int hitCounter = 0;
		bool hitLeft = Physics.Raycast(pawn.transform.position, pawn.leftSide.transform.forward, out hitInfoLeft, moveCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
		bool hitRight = Physics.Raycast(pawn.transform.position, pawn.rightSide.transform.forward, out hitInfoRight, moveCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal);
		if(hitLeft && hitInfoLeft.transform.gameObject.GetComponent<Pawn>() == null)
		{
			hitInfo = hitInfoLeft;
			calcPawnPos = pawn.leftSide.transform.position;
			hitCounter += 1;
		}
		if(hitRight && hitInfoRight.transform.gameObject.GetComponent<Pawn>() == null)
		{
			hitInfo = hitInfoRight;
			calcPawnPos = pawn.rightSide.transform.position;
			hitCounter += 1;
		}
		
		
		//check if it hit anything
		//Debug.Log(Physics.Raycast(calcPawnPos, pawn.transform.forward, out hitInfo, moveCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal));
		if(Physics.Raycast(calcPawnPos, pawn.transform.forward, out hitInfo, moveCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal))
		{
			//check if that hit was a pawn
			if(hitInfo.transform.gameObject.GetComponent<Pawn>() == null)
			{
				//if it isnt already turning away, that means it might be turning in to a wall
				
				switch(chosenDir)
						{
							case TurnSetting.None:
								//if it hasn't, check if the original angle was facing the wall
								if(Physics.Raycast(calcPawnPos, pawn.transform.forward, out hitInfo, moveCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal))
								{
									//check if that hit was a pawn
									if(hitInfo.transform.gameObject.GetComponent<Pawn>() != null)
									{
										//since it doesnt want to turn away from the player, it shouldnt turn away from pawns
										//the cache does not need to be used, and turning does not need to be initiated
									} else
									{
										//if the previous direction and turn direction are both towards the wall, it needs assistance turning away from the wall
										//reset to cached angle
										pawn.transform.rotation = cachedQuat;
										
										
										//decide which direction to turn
										//I have come up with two solutions, and both of them require stored values but this one should go towards the target more consistently
										// Filter out the target's y position.
										Vector3 filteredTargetPos = new Vector3(seekPos.x, this.transform.position.y, seekPos.z);
										//find the vector and rotation to target
										Vector3 vectorToTarget = filteredTargetPos - transform.position;
										Quaternion rotToTarget = Quaternion.LookRotation(vectorToTarget, Vector3.up);
										//So I did some testing and thinking and I believe the way it works is that , if the target y rot is bigger, go clockwise, smaller go counterclockwise, and if the difference is > 180, flip the answer
										//the only problem with this is that if the AI's rotation is set outside of the -180 to 180 range this wont work, but I just wont do that so its fine!
										if(pawn.transform.rotation.y - rotToTarget.y > 0)
										{
											
											chosenDir = TurnSetting.Clockwise;
										} else
										{
											chosenDir = TurnSetting.CounterClockwise;
											
										} 
										if(pawn.transform.rotation.y - rotToTarget.y > 180 || pawn.transform.rotation.y - rotToTarget.y < -180) 
										{
											//chosenDir = !chosenDir; 
										
											//damn it
											if(chosenDir == TurnSetting.Clockwise) 
											{
												chosenDir = TurnSetting.CounterClockwise;
											} else 
											{
												chosenDir = TurnSetting.Clockwise;
											}
										}

									}
								} else
								{
									//the original angle wasnt a wall, so it must be trying to turn into a wall
									//return back to cached value
									pawn.transform.rotation = cachedQuat;
									//disable turning
									//chosenDir = TurnSetting.None;
								}
								break;
							//it is already turning away, keep turning


							case TurnSetting.Clockwise:
								pawn.transform.rotation = cachedQuat;
								pawn.RotateClockwise();
								break;
							case TurnSetting.CounterClockwise:
								pawn.transform.rotation = cachedQuat;
								pawn.RotateCounterClockwise();
								break;
						}
				
				

			} else
			{
				//it is facing a pawn, so it doesnt need to turn
				//Disable turning setting
				//chosenDir = TurnSetting.None;
			}
		} else
		{
			//if it didnt hit anything, then its all good
			//neither raycast hit anything, disable turning
			if(hitCounter == 0)
			{
				
				if(!Physics.Raycast(pawn.transform.position, pawn.transform.forward, out hitInfo, moveCheckDist, LayerMask.GetMask("Default"), QueryTriggerInteraction.UseGlobal) || hitInfo.transform.gameObject.GetComponent<Pawn>() == null)
				{
					//Debug.Log("dangit");
					switch(chosenDir)
					{
						case TurnSetting.Clockwise:
								pawn.transform.rotation = cachedQuat;
								pawn.RotateClockwise();
								break;
							case TurnSetting.CounterClockwise:
								pawn.transform.rotation = cachedQuat;
								pawn.RotateCounterClockwise();
								break;
					}
					chosenDir = TurnSetting.None;
				}
			}
			//Disable turning setting
			//chosenDir = TurnSetting.None;
		}
			//if it didnt hit anything, then the turn is good
		//move forward regardless of whether or not it hit
		pawn.MoveForward();
		//modify move speed?
		//I tested and the turn speed is fast enough to move away from an obstacle before it runs into it, so this shouldnt be necessary

		//this code shouldnt be able to avoid corners.
	}

	protected virtual Vector3 RandomMapPos()
	{
		return new Vector3(Random.Range(GameManager.inst.mapBoundsX[0], GameManager.inst.mapBoundsX[1]), 0, Random.Range(GameManager.inst.mapBoundsZ[0], GameManager.inst.mapBoundsZ[1]));
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