using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Controller : MonoBehaviour
{
    //the pawn instance assigned to this controller, should be initialized when this pawn/controller pair is spawned
    public Pawn pawn;
	public float score;
	public int lives;
	public Spawnpoint spawnpoint;
	public int spawnpointIndex;
	
	public UIHandler uiHandler;
	
    // Start is called before the first frame update
    public virtual void Start()
    {
		if (GameManager.inst.listControllers != null) 
		{
				GameManager.inst.listControllers.Add(this);
		}
		if (spawnpoint.listSpawnedControllers != null)
		{
			//I have to store this because calling it in destroy no worky
			spawnpointIndex = spawnpoint.listSpawnedControllers.IndexOf(this);
		}
    }
	public virtual void Awake()
	{
		
	}
    // Update is called once per frame
    public virtual void Update()
    {

    }

    public virtual void ProcessInputs()
    {
        
    }

	public virtual void GainScore(float points)
	{
		score += points;
		//Debug.Log(score);
	}
	//I was not confused by the description of OnDeath and it is useful.
	public virtual void OnPawnDeath()
	{
		lives -= 1;
		if(lives <= 0)
		{
			Destroy(gameObject);
		} else
		{
			spawnpoint.Respawn(this);
		}
	}
	public void OnDestroy()
	{

		//Remove from player list
		if (GameManager.inst.listControllers != null) 
		{
			GameManager.inst.listControllers.Remove(this);
		}
		if (spawnpoint != null && spawnpoint.listSpawnedControllers != null)
		{
			//make the index null to retain list order
			spawnpoint.listSpawnedControllers[spawnpointIndex] = null;
		}
	}

	public bool FetchHealthDisplay()
	{
		if(uiHandler != null && uiHandler.healthDisplay != null)
		{
			pawn.healthDisplay = uiHandler.healthDisplay;
			return true;
		} else
		{
			return false;
		}
	}

	public virtual void OnPawnRespawn()
	{
		if(pawn.spawnpoint == null)
		{
			pawn.spawnpoint = spawnpoint;
			pawn.spawnpointIndex = spawnpointIndex;
			Debug.Log("pawn spawnpoint wasn't set properly! correcting values, but the list may not be updated correctly");
		}
	}
}
