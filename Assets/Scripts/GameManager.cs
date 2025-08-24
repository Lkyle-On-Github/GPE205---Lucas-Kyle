using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class GameManager : MonoBehaviour
{
	public enum GameStates {TitleScreen, MainMenu, Options, Game, GameOver, Win};
	public GameStates gameState;
	public float lastStateSwapTime;

	public int randomSeed;
    //idk why it says to do this at the bottom so I guess we'll find out
    // Prefabs
    public GameObject prePlayerController;
	public GameObject preAIControllerTank;
    public GameObject preTankPawn;
	public GameObject preTempSFXObject;
	public GameObject hasMapGenerator;
	public Buttons menuHandler;
	public enum MapGenSettings {Random, Custom, Daily};
	public MapGenSettings mapGenMode;
	public float roomSizeX;
	public float roomSizeZ;
	public int customSeed;
	//wasnt sure what else to call it, used in RunMapGeneration to ensure custom seeds work properly, set true in GameOver and false in MainMenu.
	public bool gameOverFlag;
	public bool multiplayer;
	public int numEnemies;

	public float earnedScore;
	public float winScore;
	public int currLevel;
	
	public UnityEngine.Random.State randomState;

	public enum Noises {Movement, Shot, Explosion, Hit};
	//Instances
    //I just feel like this is a better name because it is technically a reference to an object.
    //public Transform playerSpawn;

	
	//Misc
	//public int roomSize;
	public List<PlayerController> listPlayers;
	public List<Controller> listControllers;
	public List<Pawn> listPawns;
	public List<Spawnpoint> listSpawns;
	public List<Spawnpoint> listPlayerSpawns;
	public List<Spawnpoint> listEnemySpawns;
	public List<Spawnpoint> listUsedSpawns;
	public List<Room> listRooms;
	public List<GameObject> listActiveCams;

	public List<NoiseMaker> activeNoises;

	//if the controls can be set in a menu they would need to be stored here, for now this will just be used for default controls
	public enum PlayerKeys {Forward, Backward, Clockwise, CounterClockwise, Shoot};
	public Controls p1Controls;
	public Controls p2Controls;
	public HUDController hudController;
	public Material p1Color;
	public Material p2Color;
    //reference to self
	public static GameManager inst;

	public SettingsInput settingsInput;
	public GameObject defaultSFXAudioSource;

	public AudioMixer gameMixer;
	
    private void Awake() 
    {
        //Singleton Check
        if (inst == null) 
		{
            inst = this;

            DontDestroyOnLoad(gameObject);
        } else 
		if(inst != this) 
			{
				Destroy(gameObject);	
			}
    } 

    
    // Start is called before the first frame update
    void Start()
    {
		
		//foreach(int i in )
		//testarraydeleteplz = Enum.GetNames(typeof(PlayerKeys));
		//asdfg = Enum.Parse<PlayerKeys>(testarraydeleteplz[2]);
		//Debug.Log(PlayerKeys.ToString());
		 //Singleton Check
        if (inst == null || inst == this) 
		{
            inst = this;
			
            DontDestroyOnLoad(gameObject);
			listPawns = new List<Pawn>();
		 	listControllers = new List<Controller>();
			roomSizeX = hasMapGenerator.GetComponent<MapGenerator>().roomSizeX;
			roomSizeX = hasMapGenerator.GetComponent<MapGenerator>().roomSizeZ;
			
			//	UpdateControls(true, PlayerKeys.Forward, KeyCode.W);
			
        } else 
		{
			if(inst != this) 
			{
				Destroy(gameObject);	
			}
        }
	}
		

    // Update is called once per frame
    void Update()
    {
		if(gameState == GameStates.Options)
		{
		//
			float masterVolume = InterpretVolume(settingsInput.masterSlider.value);
			float sfxVolume = InterpretVolume(settingsInput.soundSlider.value);
			float musicVolume = InterpretVolume(settingsInput.musicSlider.value);
			gameMixer.SetFloat("MasterVolume", masterVolume);
			gameMixer.SetFloat("SFXVolume", sfxVolume);
			gameMixer.SetFloat("MusicVolume", musicVolume);
			//Debug.Log(masterVolume);
			//Debug.Log(sfxVolume);
			//Debug.Log(musicVolume);
		//
		}
		
		//gameMixer.GetFloat("Master", masterVolume);
		//gameMixer.GetFloat("SFX", sfxVolume);
		//gameMixer.GetFloat("Music", musicVolume);
		
        //foreach(PlayerController player in listPlayers)
		//{
			//player.pawn.roomLocation.roomCamera.SetActive(true);
		//}
		if(listActiveCams.Count == 0 && hasMapGenerator.GetComponent<MapGenerator>().mapExists)
		{
			//Debug.Log("updating cams!");
			UpdateCams();
		}
		//realising that camera manager should have been its own object
		//lesson learned! I think the game manager should exclusively be responsible for storing and organizing information, and any task more complicated than that should be handled by a seperate object.
		

    }
    
	public float InterpretVolume(float sliderPos)
	{
		if (sliderPos <= 0) {
            // If we are at zero, set our volume to the lowest value
            return -80;
        } else {
            // We are >0, so start by finding the log10 value 
            float posValue = Mathf.Log10(sliderPos);
            // Make it in the 0-20db range (instead of 0-1 db)
            return (posValue * 20);
        }
	}
    public void SpawnPlayer() 
    {
		int randomSpawn = UnityEngine.Random.Range(0, listPlayerSpawns.Count);
		//keep track of which spawn was used
        int usedSpawn;
		//if the randomly chosen spawn has already been used, offset the chosen spawn by 1
		if(listUsedSpawns.Contains(listPlayerSpawns[randomSpawn]))
		{
			//use 0 if it is at the last spawn to prevent out of bounds error
			if(randomSpawn == listPlayerSpawns.Count - 1)
			{
				//spawn the player and pawn at spawnpoint
				listPlayerSpawns[0].Spawn();
				usedSpawn = 0;
			} else
			{
				listPlayerSpawns[randomSpawn + 1].Spawn();
				usedSpawn = randomSpawn + 1;
			}
		} else
		{
			//spawn the player and pawn at spawnpoint
			listPlayerSpawns[randomSpawn].Spawn();
			usedSpawn = randomSpawn;
		}
		//add the used spawn to the used spawns array
		listUsedSpawns.Add(listPlayerSpawns[usedSpawn]);
    }
	
	public void SetPlayerControls(int player)
	{	

	//	player.moveForwardKey = PlayerTwoKeys[0];
	//	player.moveBackwardKey = PlayerTwoKeys[1];
	//	player.rotateClockwiseKey = PlayerTwoKeys[2];
	//	player.rotateCounterClockwiseKey = PlayerTwoKeys[3];
//
//		player.shootKey = PlayerTwoKeys[4];
	}

	public void ColorPlayer(int player, Pawn pawn)
	{
		MeshRenderer playerMeshRenderer = pawn.GetComponent<MeshRenderer>();
		Material[] newMaterials = new Material[3];
		//set these two to defaults
		newMaterials[0] = playerMeshRenderer.materials[0];
		newMaterials[1] = playerMeshRenderer.materials[1];
		if(player == 0)
		{
			newMaterials[2] = p1Color;
		}
		else 
		{
			newMaterials[2] = p2Color;
		}
		playerMeshRenderer.materials = newMaterials;
	}

	public void SpawnRandomEnemies()
	{
		//ensures that multiple enemies dont use the same spawn
		List<Spawnpoint> remainingSpawns = new List<Spawnpoint>();
		foreach(Spawnpoint spawn in listEnemySpawns)
		{
			remainingSpawns.Add(spawn);
		}
		for(int i = 0; i < numEnemies; i++)
		{
			if(remainingSpawns.Count > 0)
			{
				int randomSpawn = UnityEngine.Random.Range(0, remainingSpawns.Count);
				remainingSpawns[randomSpawn].Spawn();
				remainingSpawns.RemoveAt(randomSpawn);
			} else
			{
				//message for developers :)
				Debug.Log("Level ran out of spawners! Increase the size of the map, add more spawners to the rooms, or decrease the number of enemies you are trying to spawn");
			}
		}
	}

	public void SpawnAllEnemies()
	{
		for(int i = 0; i < listEnemySpawns.Count; i++)
		{
			Spawnpoint currSpawn = listEnemySpawns[i];
			currSpawn.Spawn();
		}
	}

	/*
	I spent so long trying to find a way to make this work :(
	public void GMListAdd(List<GameObject> _list) {
		if (GameManager.inst._list != null) {
				GameManager.inst._list.Add(this);
		}
	}

	public void GMListRemove(List<GameObject> _list) {
		if (GameManager.inst._list != null) {
				GameManager.inst._list.Remove(this);
		}
	}
	*/
	public int DateToInt ( DateTime dateToUse ) {
     // Add our date up and return it
     return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
 	}

	//custom and daily should behave differently if they arent run from the menu
	public void RunMapGeneration()
	{
		SyncMapGenSettings();
		switch(mapGenMode)
			{
				case MapGenSettings.Random:
					randomSeed = DateToInt(DateTime.Now);
					UnityEngine.Random.InitState(randomSeed);
					break;
				case MapGenSettings.Custom:
					if(!gameOverFlag)
					{
						randomSeed = customSeed;
						UnityEngine.Random.InitState(randomSeed);
						randomState = UnityEngine.Random.state;
					} else
					{
						Debug.Log("attempting to reuse seed");
						UnityEngine.Random.state = randomState;
					}
					break;
				case MapGenSettings.Daily:
					if(!gameOverFlag)
					{
						randomSeed = DateToInt(DateTime.Now.Date);
						UnityEngine.Random.InitState(randomSeed);
						randomState = UnityEngine.Random.state;
					} else
					{
						UnityEngine.Random.state = randomState;
					}
					break;
			} 
			
			
			if(hasMapGenerator != null)
			{
				if(hasMapGenerator.GetComponent<MapGenerator>() != null)
				{
					hasMapGenerator.GetComponent<MapGenerator>().GenerateMap();
				} else
				{
					Debug.Log("The object assigned as the map generator does not have the MapGenerator component.");
				}
			} else
			{
				Debug.Log("Assign an object with the MapGenerator component and set hasMapGenerator in the GameManager to generate a map");
			}
			foreach(Spawnpoint currSpawn in listSpawns)
			{
				//compile list of player spawnpoints
			if(currSpawn as PlayerSpawnpoint != null)
				{
					//Debug.Log("spawnpoint wasnt null");
					listPlayerSpawns.Add(currSpawn as PlayerSpawnpoint);
				} else
				{
					//if it doesnt count as a player spawnpoint, check if is an enemy spawnpoint and add it to that list instead
					if(currSpawn as EnemyTankSpawnpoint != null)
					{
						listEnemySpawns.Add(currSpawn);
					}
				}
			}
			//TODO: shorten this all to initialize player function
			SpawnPlayer();
			listPlayers[0].BindDisplay(hudController.p1UI);
			listPlayers[0].playerID = 0;
			ColorPlayer(0, listPlayers[0].pawn);
			p1Controls.SetPlayerControls(listPlayers[0]);
			//if multiplayer, spawn second player, and give them player 2 controls
			if(multiplayer)
			{
				SpawnPlayer();
				//SetPlayerTwoControls(listPlayers[1]);
				listPlayers[1].BindDisplay(hudController.p2UI);
				listPlayers[1].playerID = 1;
				ColorPlayer(1, listPlayers[1].pawn);
				p2Controls.SetPlayerControls(listPlayers[1]);
			}
			if(numEnemies < 0)
			{
				SpawnAllEnemies();
			} else
			{
				SpawnRandomEnemies();
			}

			UpdateCams();
		
	}

	public void RunMapDestruction()
	{
		
		hasMapGenerator.GetComponent<MapGenerator>().DeleteMap();
		DeletePairs();
		//I had to cave
		listSpawns.Clear();
		listEnemySpawns.Clear();
		listPlayerSpawns.Clear();
		activeNoises.Clear();
		hudController.p1UI.buffsTable.ClearBuffs();
		hudController.p2UI.buffsTable.ClearBuffs();
	}

	public void SyncMapGenSettings()
	{
		if(settingsInput.seededCheckBox.isOn)
		{
			mapGenMode = MapGenSettings.Custom;
			if(settingsInput.seededInputField.text != "")
			{
				customSeed = int.Parse(settingsInput.seededInputField.text);
			} else
			{
				//sets the seed to 0 if nothing is entered
				customSeed = 0;
			}
		} else
		{
			if(settingsInput.dailyCheckBox.isOn)
			{
				mapGenMode = MapGenSettings.Daily;
			} else
			{
				mapGenMode = MapGenSettings.Random;
			}
		}
	}
	public void DeletePairs()
	{
		foreach (Pawn pawn in listPawns)
		{
			Destroy(pawn.gameObject);
		}
		foreach (Controller controller in listControllers)
		{
			Destroy(controller.gameObject);
		}
	}

	//since this does a lot of array iterations, it is only called each time a player enters or exits a room's roomtrigger
	public void UpdateCams() 
	{
		foreach(Room room in listRooms)
			{
				room.StopCamera();
			}
		//iterate for each player in listPlayers, preparing for splitscreen
		foreach(PlayerController player in listPlayers)
		{
			//initialize variables to the first room to check
			Room closestRoom = listRooms[0];
			float closestRoomDist = Vector3.Distance(player.pawn.transform.position, listRooms[0].transform.position);  
			/*
			for(int i = 0; i < listRooms.Count; i++)
			{
				if(Vector3.Distance(player.pawn.transform.position, listRooms[i].transform.position))
			}
			*/
			//find the closest room by iterating through all of them and storing the closest one found so far
			foreach(Room room in listRooms)
			{
				float currRoomDist = Vector3.Distance(player.pawn.transform.position, room.transform.position);
				if(currRoomDist < closestRoomDist)
				{
					closestRoomDist = currRoomDist;
					closestRoom = room;
				}
			}
			//enable the cam for the calculated room
			closestRoom.StartCamera();
			player.pawn.roomLocation = closestRoom;
			//inform the camera that it is following this player
			player.pawn.roomLocation.roomCameraRoomCamera.pawn = player.pawn;
			Debug.Log("closestRoom is" + closestRoom);
		}
		
		//twin condition: if there are two players, and they are in different rooms
		//if a player is respawning, skip this frame of twin execution
		if(listPlayers.Count > 1)
		{
			if(!(listPlayers[0].isRespawing || listPlayers[1].isRespawing))
			{
				if(listPlayers[0].pawn.roomLocation != listPlayers[1].pawn.roomLocation)
				{
					foreach(GameObject camera in listActiveCams)
					{
						//Get the RoomCamera
						RoomCamera currCam = camera.GetComponent<RoomCamera>();
						//start twin for the RoomCamera
						currCam.SwapTwin(true);
						//if the room z of the room of this camera is lower than the z of the camera of the other pawn, this is the left camera, otherwise this is the right camera.
					}
					//I will take this as a lesson for the future to not be lazy and update variables for their new purpose as soon as I can, but it is too late now to change this
					if(listActiveCams[0].GetComponent<RoomCamera>().pawn.roomLocation.x <= listActiveCams[1].GetComponent<RoomCamera>().pawn.roomLocation.x)
					{
						listActiveCams[0].GetComponent<RoomCamera>().leftTwin = true;
						listActiveCams[1].GetComponent<RoomCamera>().leftTwin = false;
					} else
					{
						
						listActiveCams[0].GetComponent<RoomCamera>().leftTwin = false;
						listActiveCams[1].GetComponent<RoomCamera>().leftTwin = true;
					}
				} else
				{
					//there is only one camera, set it to default settings.
					foreach(GameObject camera in listActiveCams)
					{
						RoomCamera currCam = camera.GetComponent<RoomCamera>();
						currCam.SwapTwin(false);
					}
				}
			}
		} else
		{
			//there is only one camera, set it to default settings.
			foreach(GameObject camera in listActiveCams)
			{
				RoomCamera currCam = camera.GetComponent<RoomCamera>();
				currCam.SwapTwin(false);
			}
		}
	}

	public void OverrideUIPositions()
	{
		//Debug.Log("doing override!");
		//sets the first player or solo player to the left side, and the other player to the right side by default
		foreach(PlayerController player in listPlayers)
		{
			if(listPlayers.IndexOf(player) == 0)
			{
				player.uiHandler.boundUI.SetSide(true);
			} else
			{
				player.uiHandler.boundUI.SetSide(false);
			}
		}
	}

	//TitleScreen, MainMenu, Options, Game, GameOver, Credits
	protected virtual void StateStart() 
	{
		//this is how I write my switch statements it makes it easier for me to read but I can change it if you really need me to
		switch (gameState)
		{
			case GameStates.TitleScreen:

				break;
			case GameStates.MainMenu:
				gameOverFlag = false;
				earnedScore = 0;
				currLevel = 0;
				break;
			case GameStates.Options:
				//RunMapDestruction();
				break;
			case GameStates.Game:
				currLevel += 1;
				hudController.gameObject.SetActive(true);
				if(!hasMapGenerator.GetComponent<MapGenerator>().mapExists)
				{
					RunMapGeneration();
				}
				p1Controls.SetPlayerControls(listPlayers[0]);
				if(multiplayer)
				{
					p2Controls.SetPlayerControls(listPlayers[1]);
				}
				break;
			case GameStates.GameOver:
				earnedScore = 0;
				currLevel = 0;
				gameOverFlag = true;
				break;
			case GameStates.Win:
				gameOverFlag = true;
				RunMapDestruction();
				break;
		}
	}
	protected virtual void StateEnd()
	{
		switch (gameState)
		{
			case GameStates.TitleScreen:

				break;
			case GameStates.MainMenu:

				break;
			case GameStates.Options:
				
				break;
			case GameStates.Game:
				if(hudController != null)
				{
					hudController.gameObject.SetActive(false);
				}
				break;
			case GameStates.GameOver:

				break;
			case GameStates.Win:

				break;
		}
	}
	public virtual void SwapState(GameStates newState)
	{
		if(gameState != newState)
		{
		StateEnd();
		gameState = newState;
		StateStart();

		lastStateSwapTime = Time.time;
		}
	}

	//GENERAL UTILITY FUNCTIONS
	public virtual void SpawnSoundEffect(AudioClip audioClip, Vector3 pos)
	{
		GameObject currSFXObj = Instantiate(preTempSFXObject, pos, Quaternion.identity) as GameObject;
		currSFXObj.GetComponent<AudioSource>().clip = audioClip;
	}

	public void OnPlayerDeath()
	{
		if(listPlayers.Count == 0)
		{
			if(menuHandler != null)
			{
				menuHandler.gameOverScore.text = new string("Total Score: " + earnedScore);
				SwapState(GameStates.GameOver);
				menuHandler.SwapState(Buttons.MenuStates.GameOver, false);
				
			}
		} else
		{
			UpdateCams();
		}
	}

	public void UpdateControls(bool p1, PlayerKeys toChange, KeyCode newKey)
	{
		if(p1Controls == null)
		{
			p1Controls = new Controls();
			p1Controls.InitControls();
		}
		if(p2Controls == null)
		{
			p2Controls = new Controls();
			p2Controls.InitControls();			
		}
		if(p1)
		{
			p1Controls.ChangeKey(toChange, newKey);
		} else
		{
			p2Controls.ChangeKey(toChange, newKey);
		}
	}

	public void AddScore(float score)
	{
		earnedScore += score;
		if(earnedScore >= winScore * currLevel)
		{
			if(menuHandler != null)
			{
				SwapState(GameStates.Win);
				menuHandler.SwapState(Buttons.MenuStates.Win, false);
				menuHandler.winScore.text = new string("Current Score: " + earnedScore);
			}
		}
	}
}