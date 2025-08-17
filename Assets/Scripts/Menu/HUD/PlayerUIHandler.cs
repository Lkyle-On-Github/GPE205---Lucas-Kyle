using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHandler : UIHandler
{
	public PlayerController controller;
	public ScoreDisplay scoreDisplay;
	public LivesDisplay livesDisplay;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//sets the pawn's health display when it respawns. - replaced with less expensive solution
        /*
		if(controller.pawn != null)
		{
			if(controller.pawn.healthDisplay == null)
			{
				controller.pawn.healthDisplay = boundUI.healthDisplay;
			}
		}
		*/
		//if the game is in multiplayer, determine if my UI should be on the right or left
		if(GameManager.inst.multiplayer && GameManager.inst.listPlayers.Count == 2)
		{
			Controller otherPlayer;
			if(GameManager.inst.listPlayers[0] == controller)
			{
				otherPlayer = GameManager.inst.listPlayers[1];
			} else
			{
				otherPlayer = GameManager.inst.listPlayers[0];
			}
			//If I'm the one on the left
			if(controller.pawn.transform.position.x < otherPlayer.pawn.transform.position.x)
			{
				boundUI.SetSide(true);
			} else
			{
				//if I'm the one on the right
				if(controller.pawn.transform.position.x > otherPlayer.pawn.transform.position.x)
				{
					boundUI.SetSide(false);
				} else
				{
					//players have the same horizontal position, override to default positions.
					GameManager.inst.OverrideUIPositions();
				}

			}
		} else
		{
			//there is only one player remaining or the game is in singleplayer, override to default positions.
			GameManager.inst.OverrideUIPositions();
		}
    }
	public override void BindDisplay ()
	{
		//uiHandler.boundUI = playerUI;
		controller.pawn.healthDisplay = boundUI.healthDisplay;
		scoreDisplay = boundUI.scoreDisplay;
		livesDisplay = boundUI.livesDisplay;
		healthDisplay = boundUI.healthDisplay;
		DisplayDefaultValues();
	}
	public override void DisplayDefaultValues()
	{
		livesDisplay.SetLives(controller.lives);
		scoreDisplay.SetScore("0");
		healthDisplay.SetHealth(10);
	}

	public override void UpdateScore(float score)
	{
		if(scoreDisplay != null)
		{
			scoreDisplay.SetScore(score);
		}
	}

	public override void LoseLife()
	{
		if(livesDisplay != null)
		{
			livesDisplay.Decrement();
		}
	}
}
