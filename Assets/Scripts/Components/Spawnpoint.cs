using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
	//this was from a janky solution I almost had to implement but Im keeping it around jsut in case I actually have to go with that solution
	public enum SpawnCategories {Player, Enemy};
	public SpawnCategories spawnCategory;
	public int team; //am I ever going to get around to making this an enum
	public int lives;
	public GameObject controller;
	public GameObject pawn;
	Vector3 spawnPos;

	public List<Controller> listSpawnedControllers;
	//wont be used but if im making a list for the controllers I should make a list for the pawns too
	public List<Pawn> listSpawnedPawns;

	public void Start() 
	{
		if (GameManager.inst.listSpawns != null) 
		{
			if(!GameManager.inst.listSpawns.Contains(this))
			{
				GameManager.inst.listSpawns.Add(this);
			}
		}
	}
	public void Awake()
	{
		if(GameManager.inst != null)
		{
			if (GameManager.inst.listSpawns != null) 
			{
				if(!GameManager.inst.listSpawns.Contains(this))
				{
					GameManager.inst.listSpawns.Add(this);
				}
			}
		}
	}
	public virtual void Spawn()
	{
		GameObject objNewAI = Instantiate(controller, Vector3.zero, Quaternion.identity) as GameObject;
		
		
        GameObject objNewPawn = Instantiate(pawn, this.transform.position, this.transform.rotation) as GameObject;
		

		//find the controller and pawn components
        Controller compController = objNewAI.GetComponent<Controller>();
        Pawn compPawn = objNewPawn.GetComponent<Pawn>();
		listSpawnedControllers.Add(compController);
		compController.spawnpoint = this;
		compController.lives = lives;
		listSpawnedPawns.Add(compPawn);
		compPawn.spawnpoint = this;
		//hook controller to pawn
		compController.pawn = compPawn;
		compPawn.controller = compController;
		
		Damage damage = objNewPawn.GetComponent<Damage>();
		damage.team = team;
		CleanLists();
	}

	public virtual void Respawn(Controller controller)
	{
		GameObject objNewPawn = Instantiate(pawn, this.transform.position, this.transform.rotation) as GameObject;
		//find the controller and pawn components
        Pawn compPawn = objNewPawn.GetComponent<Pawn>();

		//hook controller to pawn
		controller.pawn = compPawn;
		compPawn.controller = controller;
		compPawn.spawnpoint = this;


		Damage damage = objNewPawn.GetComponent<Damage>();
		damage.team = team;
		controller.OnPawnRespawn();
		//if it is being respawned from the same spawnpoint it used before, insert the pawn in the same point in the array as the controller. Otherwise, put them both at the end of the array
		if(listSpawnedControllers.Contains(controller)) 
		{
			listSpawnedPawns[listSpawnedControllers.IndexOf(controller)] = compPawn;
		} else
		{
			listSpawnedControllers.Add(controller);
			listSpawnedPawns.Add(compPawn);
		}
		CleanLists();
	}

	public virtual void CleanLists()
	{
		for(int i = 0; i < listSpawnedControllers.Count; i++)
		{
			if(listSpawnedPawns[i] == null && listSpawnedControllers[i] == null)
			{
				listSpawnedControllers.RemoveAt(i);
				listSpawnedPawns.RemoveAt(i);
			}
		}
	}


	public void OnDestroy()
	{

		//Remove from all pawn lists
		if (GameManager.inst.listSpawns != null) 
		{
			GameManager.inst.listSpawns.Remove(this);
			
		}
		if(GameManager.inst.listUsedSpawns != null && GameManager.inst.listUsedSpawns.Contains(this))
		{
			Debug.Log("removing used spawn!");
			GameManager.inst.listUsedSpawns.Remove(this);
		}
		if (GameManager.inst.listEnemySpawns != null && GameManager.inst.listEnemySpawns.Contains(this))
		{
			Debug.Log("removing enemy spawn!");
			GameManager.inst.listEnemySpawns.Remove(this);
		} else
		{
			if (GameManager.inst.listPlayerSpawns != null && GameManager.inst.listPlayerSpawns.Contains(this))
			{
				Debug.Log("removing player spawn!");
				GameManager.inst.listPlayerSpawns.Remove(this);
			}
		}
	}
}
