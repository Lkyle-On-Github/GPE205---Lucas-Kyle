using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
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

	public void OnTriggerEnter(Collider collisionInfo)
	{
		Pawn collidingPawn = collisionInfo.gameObject.GetComponent<Pawn>();
		if(collidingPawn != null)
		{
			if(GameManager.inst.listPlayers.Contains(collidingPawn.controller as PlayerController))
			{
				
				//room.StartCamera();
				GameManager.inst.UpdateCams();
			} else
			{
			//moving roomLocation update to the distance check thingy in the cam update
			//AI's can use simplified room detection
				collidingPawn.roomLocation = room;
			}
		}
	}
	/*
	was breaking the camera for some reason
	public void OnTriggerExit(Collider collisionInfo)
	{
		Pawn collidingPawn = collisionInfo.gameObject.GetComponent<Pawn>();
		if(collidingPawn != null)
		{
			if(GameManager.inst.listPlayers.Contains(collidingPawn.controller as PlayerController))
			{
				//room.StartCamera();
				//GameManager.inst.UpdateCams();
			}
		}
	}
	*/
}