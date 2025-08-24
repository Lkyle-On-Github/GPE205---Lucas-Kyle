using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIDefender : AIController
{
	public List<Vector3> patrolPoints;
	public float guardTime;
	private float guardStartTime;
	public float guardDist;
	public int patrolPoint;
	public Vector3 patrolPos;
	private float rotateStart;
	public bool subStateRotate;
	//innefficient way to do this
	public float rotatedDegrees;
	public int spawnRoomX;
	public int spawnRoomZ;
	public bool spawnRoomGotten;

	void Start()
    {
		base.Start();
		//perhaps I should make this pick the closest player?
        target = GameManager.inst.listPlayers[0].pawn;
		subStateRotate = true;
		SwapState(States.Patrol);
		ToggleSenses(true,false);
		
    }

    // Update is called once per frame
    void Update()
    {
		base.Update();
        MakeDecisions();
    }

	public void ShiftPatrolPoints()
	{
		if(pawn.roomLocation != null)
			{
				spawnRoomGotten = true;
				spawnRoomX = pawn.roomLocation.x;
				spawnRoomZ = pawn.roomLocation.z;
				for(int i = 0; i < patrolPoints.Count; i++)
				{

					patrolPoints[i] += new Vector3(pawn.roomLocation.x * GameManager.inst.hasMapGenerator.GetComponent<MapGenerator>().roomSizeX, 0, pawn.roomLocation.z * GameManager.inst.hasMapGenerator.GetComponent<MapGenerator>().roomSizeZ);
				}
			}
	}
    protected override void MakeDecisions() 
	{
		switch (state) 
		{
			case States.Patrol:
			//state behaviour
				DoPatrolState();
			//state change check
				//done in sense update
				break;
			case States.Fire:
			//state behaviour
				if(target != null) 
				{
				RotateTowards(target.transform.position);
				pawn.Shoot();
				}
			//state change check
				//done in sense update
				break;
			case States.Investigate:
				if(IsFacing(lastTargetPos, 2.5f))
					{
						Debug.Log("facing last target pos!");
						SwapState(States.Guard);
					} else
					{
						RotateTowards(lastTargetPos);
					}
				break;
			case States.Guard:
			//state behaviour
				DoGuardState();
			//state change check
				if(guardTime + guardStartTime < Time.time)
				{
					SwapState(States.Patrol);
				}
				break;
		}
	}
	protected override void StateStart() 
	{
		//this is how I write my switch statements it makes it easier for me to read but I can change it if you really need me to
		switch (state)
		{
			case States.Patrol:
				//roundabout check for if this is the first cycle
				if(spawnRoomGotten == true)
				{
					subStateRotate = false;
				}
				break;
			case States.Fire:

				break;
			case States.Investigate:
				ToggleSenses(true,true);
				break;
			case States.Guard:
				UpdatePatrol(ClosestPatrolPoint());
				break;
		}
	}
	protected override void StateEnd()
	{
		switch (state)
		{
			case States.Patrol:
			
				break;
			case States.Fire:
			
				
				break;
			case States.Investigate:
				ToggleSenses(true,false);
				break;
			case States.Guard:
			
				
				break;
		}
	}

	protected virtual int ClosestPatrolPoint()
	{
		int closest = 0;
		float closestDist = Vector3.Distance(pawn.transform.position, patrolPoints[0]);
		for (int i = 1; i > patrolPoints.Count; i++)
		{
			if(Vector3.Distance(pawn.transform.position, patrolPoints[i]) < closestDist)
			{
				closest = i;
			}
		}
		return closest;
	}
	protected virtual void NextPatrolPoint()
	{
		//increase patrol point by one
		patrolPoint += 1;
		//check if it needs to loop
		if(patrolPoint >= patrolPoints.Count)
		{
			UpdatePatrol(0);
		} else
		{
			UpdatePatrol(patrolPoint);
		}
		
	}
	protected virtual void UpdatePatrol(int point)
	{
		
		patrolPoint = point;
		patrolPos = (patrolPoints[point]);
	}

	protected override void OnSenseUpdate()
	{
		switch(state)
		{
			case States.Patrol:
				if(ChooseVisibleTarget())
				{
					SwapState(States.Fire);
				}
				break;
			case States.Fire:
				if(!ChooseVisibleTarget())
				{
					SwapState(States.Investigate);
				}
				break;
			case States.Guard:
				if(ChooseVisibleTarget())
				{
					SwapState(States.Fire);
				}
				break;
			case States.Investigate:

				if(ChooseVisibleTarget())
				{
					SwapState(States.Fire);
				} else
				{
					if(ChooseNoiseByPrio())
					{
						lastTargetPos = targetNoisePos;
					}
				}
				break;
		}
	}

	protected virtual void DoPatrolState()
	{
		if(subStateRotate)
		{
			Vector3 startAng = pawn.transform.forward;
			RotateClockwise();
			//I think this measures in full rotations rather than degrees but im not changing the name
			if (rotatedDegrees > 360)
			{
				if(!spawnRoomGotten)
				{	
					rotatedDegrees = 0;
					subStateRotate = false;
					ShiftPatrolPoints();
					UpdatePatrol(patrolPoint);
				} else
				{
					NextPatrolPoint();
					rotatedDegrees = 0;
					subStateRotate = false;
				}
			}
			else
			{
				rotatedDegrees += Vector3.Angle(startAng, pawn.transform.forward);
			}
		} else
		{
			if (!DistanceCheck(patrolPos, seekDist))
			{
				SeekSmart(patrolPos);	
				//rotateStart = pawn.transform.rotation.y;
			}
			else
			{
				subStateRotate = true;
			}
		}
	}

	protected virtual void DoGuardState()
	{
		if(!DistanceCheck(patrolPos, guardDist))
			{
				SeekSmart(patrolPos);
				//technically a way to make it set the guardStartTime once it stops moving.
				guardStartTime = Time.time;
			} else
			{
				RotateClockwise();
			}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}
}
