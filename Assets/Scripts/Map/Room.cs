using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {
	public GameObject roomCamera;
	//ulg
	public RoomCamera roomCameraRoomCamera;
	public GameObject doorNorth;
	public GameObject doorSouth;
	public GameObject doorEast;
	public GameObject doorWest;
	public int x;
	public int z;

	public void Awake()
	{
		if(GameManager.inst.listRooms != null)
		{
			GameManager.inst.listRooms.Add(this);
		}
	}
	//public string name;
	public void StartCamera()
	{
		roomCamera.SetActive(true);
		if(GameManager.inst.listActiveCams != null)
		{
			if(!GameManager.inst.listActiveCams.Contains(roomCamera))
			{
				GameManager.inst.listActiveCams.Add(roomCamera);
			}
		}
	}

	public void StopCamera()
	{
		roomCamera.SetActive(false);
		if(GameManager.inst.listActiveCams != null)
		{
			if(GameManager.inst.listActiveCams.Contains(roomCamera))
			{
				GameManager.inst.listActiveCams.Remove(roomCamera);
			}
		}
	}





}
