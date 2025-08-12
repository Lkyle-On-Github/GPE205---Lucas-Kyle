using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    //how fast the object moves
    public float moveSpeed;
    //how fast the obect turns
    public float turnSpeed;
	//the controller associated with this pawn (needed in order to feed the bullet this info)
	public Controller controller;
	public Mover mover;
	public Shooter shooter;
	public Damage damage;
	public Health health;
	public GameObject leftSide;
	public GameObject leftAngle;
	public GameObject rightSide;
	public GameObject rightAngle;
	public Room roomLocation;
	public Damage lastAttacker;
	public float killScore;
	public Spawnpoint spawnpoint;
	public int spawnpointIndex;

    // Start is called before the first frame update
    public virtual void Start()
    {
		if (GameManager.inst.listPawns != null) 
		{
				GameManager.inst.listPawns.Add(this);
		}
		if (spawnpoint.listSpawnedPawns != null)
		{
			//I have to store this because calling it in destroy no worky
			spawnpointIndex = spawnpoint.listSpawnedPawns.IndexOf(this);
		}
    }

    // Update is called once per frame
    public virtual void Update()
    {
		//I think this doesnt do anything
		//Debug.Log(spawnpoint.listSpawnedPawns.IndexOf(this));
		
    }

    public abstract void RotateClockwise();
    public abstract void RotateCounterClockwise();
	public abstract void RotateTowardsPoint(Vector3 target);
    public abstract void MoveForward();
	public abstract void MoveForward(float speed);
    public abstract void MoveBackward();
	public abstract void Shoot();
	//I think you want me to do it like this?
	public abstract void Seek(Vector3 targetPos);
	public abstract void Seek(Transform target);
	public abstract void Seek(GameObject target);
	public abstract void Seek(Pawn target);
	public abstract void Seek(Controller target);
	
	public abstract void MakeNoise(GameManager.Noises noise);
	
    
	public void OnDestroy()
	{

		//Remove from player list
		if (GameManager.inst.listPawns != null) 
		{
				GameManager.inst.listPawns.Remove(this);
				
		}
		if (spawnpoint.listSpawnedPawns != null)
		{
			//make the index null to retain list order
			//Debug.Log(spawnpoint.listSpawnedPawns.IndexOf(this));
			spawnpoint.listSpawnedPawns[spawnpointIndex] = null;
		}
	}
}
