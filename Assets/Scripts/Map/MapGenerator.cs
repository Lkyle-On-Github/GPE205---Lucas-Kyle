using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGenerator : MonoBehaviour
{
	public GameObject[] rooms;
	public bool mapExists;
	//measured in rooms
	public int mapWidth;
	public int mapHeight;
	public int roomSizeX;
	public int roomSizeZ;
	public Room[ , ] mapGrid;
    // Start is called before the first frame update
    void Start()
    {
        mapExists = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public GameObject GetRandomRoom() {
    	return rooms[UnityEngine.Random.Range(0, rooms.Length)];
 	}

	public void GenerateMap()
	{
		if(!mapExists)
		{
			//Debug.Log("Generating map!");
			mapGrid = new Room[mapWidth, mapHeight];

			for(int currRow = 0; currRow < mapHeight; currRow++ )
			{
				for(int currColumn = 0; currColumn < mapWidth; currColumn++)
				{
					//prepare the rooms position
					float currRoomX = currRow * roomSizeX;
					float currRoomZ = currColumn * roomSizeZ;
					Vector3 currRoomPos = new Vector3(currRoomX, 0.0f, currRoomZ);
					//choose a random room and place it
					GameObject currRoom = Instantiate(GetRandomRoom(), currRoomPos, Quaternion.identity) as GameObject;

					//center the room on the generator
					//I dont think it will be centered actually which I dont like
					currRoom.transform.parent = this.transform;
					//Allow identification of the room
					currRoom.name = "Room_" +currColumn+","+currRow;

					//what have I done
					Room currRoomRoom = currRoom.GetComponent<Room>();
					currRoomRoom.x = currRow;
					currRoomRoom.z = currColumn;
					mapGrid[currColumn,currRow] = currRoomRoom;
					//oDebug.Log(mapGrid[currColumn,currRow]);
					InitializeDoors(currColumn, currRow, currRoomRoom);
				}
			}
		} else
		{
			Debug.Log("A map already exists!");
		}
		mapExists = true;

	}
	public void InitializeDoors(int col, int row, Room room)
	{
		if(col != 0)
		{
			Destroy(room.doorSouth);
		}
		if(col != mapWidth - 1)
		{
			Destroy(room.doorNorth);
		}
		if(row != 0)
		{
			Destroy(room.doorWest);
		}
		if(row != mapHeight - 1)
		{
			Destroy(room.doorEast);
		}
	}

	public void DeleteMap()
	{
		mapExists = false;
		foreach (Room room in mapGrid)
		{
			Destroy(room.gameObject);
		}
		
	}
}
