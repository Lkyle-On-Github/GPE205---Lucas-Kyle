using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFloor : MonoBehaviour
{
	public Room room;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
	/*
	public void OnCollisionStay(Collision collisionInfo)
	{
		Pawn collidingPawn = collisionInfo.gameObject.GetComponent<Pawn>();
		if(collidingPawn != null)
		{
			if(GameManager.inst.listPlayers.Contains(collidingPawn.controller as PlayerController))
			{
				collidingPawn.roomLocation = room;
				room.StartCamera();
			}
		}
	}
	public void OnCollisionExit(Collision collisionInfo)
	{
		Pawn collidingPawn = collisionInfo.gameObject.GetComponent<Pawn>();
		if(collidingPawn != null)
		{
			if(GameManager.inst.listPlayers.Contains(collidingPawn.controller as PlayerController))
			{
			collidingPawn.roomLocation = room;
			room.StopCamera();
			}
		}
	}
	*/
}
