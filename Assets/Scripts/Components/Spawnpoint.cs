using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
	public int team;
	public GameObject controller;
	public GameObject pawn;
	Vector3 spawnPos;

	public void Start() 
	{
		if (GameManager.inst.listSpawns != null) 
		{
				GameManager.inst.listSpawns.Add(this);
		}
	}
	public void Spawn()
	{
		GameObject objNewAI = Instantiate(controller, Vector3.zero, Quaternion.identity) as GameObject;
        GameObject objNewPawn = Instantiate(pawn, this.transform.position, this.transform.rotation) as GameObject;

		//find the controller and pawn components
        Controller compController = objNewAI.GetComponent<Controller>();
        Pawn compPawn = objNewPawn.GetComponent<Pawn>();

		//hook controller to pawn
		compController.pawn = compPawn;
		compPawn.controller = compController;

		Damage damage = objNewPawn.GetComponent<Damage>();
		damage.team = team;
	}

	public void OnDestroy()
	{

		//Remove from player list
		if (GameManager.inst.listSpawns != null) 
		{
				GameManager.inst.listSpawns.Remove(this);
		}
	}
}
