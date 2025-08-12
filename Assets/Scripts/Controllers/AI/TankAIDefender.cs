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
	private Vector3 patrolPos;
	private float rotateStart;
	public bool subStateRotate;
	//innefficient way to do this
	public float rotatedDegrees;
	public int spawnRoomX;
	public int spawnRoomZ;
	public bool spawnRoomGotten;

	public Pawn target;
	void Start()
    {
		//perhaps I should make this pick the closest player?
        target = GameManager.inst.listPlayers[0].pawn;
		SwapState(States.Patrol);
		ToggleSenses(true,false);
		
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
				subStateRotate = false;
				break;
			case States.Fire:
				
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
		if(patrolPoint >= patrolPoints.Count - 1)
		{
			UpdatePatrol(0);
		} else
		{
			UpdatePatrol(patrolPoint + 1);
		}
		
	}
	protected virtual void UpdatePatrol(int point)
	{
		
		patrolPoint = point;
		patrolPos = (patrolPoints[point] + new Vector3(spawnRoomX * GameManager.inst.roomSize, 0 , spawnRoomZ * GameManager.inst.roomSize));
	}

	protected override void OnSenseUpdate()
	{
		switch(state)
		{
			case States.Patrol:
			if(target != null)
				{
					if(visiblePawns.Contains(target))
					{
						SwapState(States.Fire);
					}
				}
				break;
			case States.Fire:
				if(target != null)
				{
					if(!visiblePawns.Contains(target))
					{
						SwapState(States.Guard);
					}
				}
				break;
			case States.Guard:
			if(target != null)
				{
					if(visiblePawns.Contains(target))
					{
						SwapState(States.Fire);
					}
				}
				break;
		}
	}

	protected virtual void DoPatrolState()
	{
		if(spawnRoomGotten)
		{
			if(subStateRotate)
			{
				Vector3 startAng = pawn.transform.forward;
				RotateClockwise();
				//I think this measures in full rotations rather than degrees but im not changing the name
				if (rotatedDegrees > 360)
				{
					subStateRotate = false;
					NextPatrolPoint();
				}
				else
				{
					rotatedDegrees += Vector3.Angle(startAng, pawn.transform.forward);
				}
			} else
			{
				if (DistanceCheck(patrolPos, 1.5f))
				{
					subStateRotate = true;
					rotateStart = pawn.transform.rotation.y;
				}
				else
				{
					SeekSmart(patrolPos);
					rotatedDegrees = 0;
				}
			}
		} else
		{
			if(pawn.roomLocation != null)
			{
				spawnRoomGotten = true;
				spawnRoomX = pawn.roomLocation.x;
				spawnRoomZ = pawn.roomLocation.z;
				UpdatePatrol(0);
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
}
