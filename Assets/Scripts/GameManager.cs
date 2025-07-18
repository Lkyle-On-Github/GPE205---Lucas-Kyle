using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //idk why it says to do this at the bottom so I guess we'll find out
    // Prefabs
    public GameObject prePlayerController;
	public GameObject preAIControllerTank;
    public GameObject preTankPawn;


	//Instances
    //I just feel like this is a better name because it is technically a reference to an object.
    public Transform playerSpawn;


	//Misc
	public List<PlayerController> listPlayers;
	public List<Controller> listControllers;
	public List<Pawn> listPawns;
	public List<Spawnpoint> listSpawns;

    //reference to self
	public static GameManager inst;

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
		 //Singleton Check
        if (inst == null) 
		{
            inst = this;

            DontDestroyOnLoad(gameObject);
        } else 
		{
			if(inst != this) 
			{
				Destroy(gameObject);	
			}
          }
		SpawnPlayer();
		SpawnAllEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpawnPlayer() 
    {
        //spawn the player and pawn at spawnpoint
        GameObject objNewPlayer = Instantiate(prePlayerController, Vector3.zero, Quaternion.identity) as GameObject;
        GameObject objNewPawn = Instantiate(preTankPawn, playerSpawn.position, playerSpawn.rotation) as GameObject;

        //find the controller and pawn components
        Controller compController = objNewPlayer.GetComponent<Controller>();
        Pawn compPawn = objNewPawn.GetComponent<Pawn>();

		//hook controller to pawn
		compController.pawn = compPawn;
		compPawn.controller = compController;
    }

	public void SpawnAllEnemies()
	{
		for(int i = 0; i < listSpawns.Count; i++)
		{
			Spawnpoint currSpawn = listSpawns[i];
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
}
