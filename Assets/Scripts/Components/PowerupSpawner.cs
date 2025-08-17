using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
	public GameObject powerup;
	public float spawnDelay;
	private float spawnReadyTime;
	public int numSpawns;
	public bool infSpawns;
	private GameObject powerupInst;
	private bool spawnReady;
	
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnReadyTime < Time.time && spawnReady && (numSpawns > 0 || infSpawns)) 
		{
			//lastSpawnTime = Time.time;
			Spawn();
		}
    }

	public void Spawn()
	{
		//spawns the powerup, sets its spawner, and disables spawning.
		spawnReady = false;
		powerupInst = Instantiate(powerup, this.transform.position, this.transform.rotation) as GameObject;
		powerupInst.GetComponent<Pickup>().spawner = this;
		if(!infSpawns)
		{
			numSpawns -= 1;
		}
		

	}

	public void OnPowerupDestroyed()
	{	
		//when powerup is destroyed, set timer to spawn the next powerup.
		spawnReadyTime = (Time.time + spawnDelay);
		spawnReady = true;
	}

	public void OnDestroy()
	{
		if(GameManager.inst.hasMapGenerator != null && !GameManager.inst.hasMapGenerator.GetComponent<MapGenerator>().mapExists && powerupInst != null)
		{
			Destroy(powerupInst);
		}
	}
}
