using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : Controller
{
	//movement keys
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateClockwiseKey;
    public KeyCode rotateCounterClockwiseKey;

	public KeyCode shootKey;

	public int playerID;
	public bool isRespawing;
    // Start is called before the first frame update
    public override void Start()
    {
		//add self to player list
	    //I kinda feel like its better if the game crashes with a null reference exception if the gamemanager doesnt exist so that I can read the error message and know what the problem is, since the entire game wouldnt function without the manager anyways.
		if (GameManager.inst.listPlayers != null && !GameManager.inst.listPlayers.Contains(this)) 
		{
				GameManager.inst.listPlayers.Add(this);
		}
		

		base.Start();
    }

	public override void Awake()
	{
		if (GameManager.inst.listPlayers != null && !GameManager.inst.listPlayers.Contains(this)) 
		{
				GameManager.inst.listPlayers.Add(this);
		}
	}
    // Update is called once per frame
    public override void Update()
    {   
        // Process our Keyboard Inputs
        ProcessInputs();

        base.Update();        
    }

    public override void ProcessInputs()
    {
		if(GameManager.inst.gameState == GameManager.GameStates.Game)
		{
			//while opposite directions would likely cancel out, I think doing it like that may cause some problems with things like inertia and it would be better to just make the inputs exclusive.
			if (Input.GetKey(moveForwardKey) && !Input.GetKey(moveBackwardKey))
			{
				pawn.MoveForward();
			} else if (Input.GetKey(moveBackwardKey))
			{
				pawn.MoveBackward();
			}

			if (Input.GetKey(rotateClockwiseKey) && !Input.GetKey(rotateCounterClockwiseKey)) 
			{
				pawn.RotateClockwise();
			} else if (Input.GetKey(rotateCounterClockwiseKey)) 
			{
				pawn.RotateCounterClockwise();
			}

			if(Input.GetKeyDown(shootKey)) {
				pawn.Shoot();
			}
		}
    }

	public override void OnPawnDeath()
	{
		lives -= 1;
		if(lives <= 0)
		{
			Destroy(gameObject);
		} else
		{
			isRespawing = true;
			spawnpoint.Respawn(this);
		}
		uiHandler.LoseLife();
	}

	public override void OnPawnRespawn()
	{
		//Debug.Log("pawn respawn was called");
		GameManager.inst.UpdateCams();
		GameManager.inst.ColorPlayer(playerID, pawn);
		isRespawing = false;
	}

	public override void GainScore(float toGain)
	{
		base.GainScore(toGain);
		uiHandler.UpdateScore(score);
	}

	public void OnDestroy()
	{
		if(GameManager.inst != null)
		{
			
			//Add score to total
			GameManager.inst.earnedScore += score;
			//Remove from player list
			if (GameManager.inst.listPlayers != null) 
			{
					GameManager.inst.listPlayers.Remove(this);
			}
			GameManager.inst.OnPlayerDeath();
			base.OnDestroy();
		}
	}

	public void BindDisplay (PlayerUI playerUI)
	{
		uiHandler.boundUI = playerUI;
		playerUI.gameObject.SetActive(true);
		uiHandler.BindDisplay();
		DisplayDefaultValues();
	}

	public void DisplayDefaultValues()
	{
		uiHandler.DisplayDefaultValues();
	}
}
