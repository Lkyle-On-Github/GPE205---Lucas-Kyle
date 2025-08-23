using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHandler : UIHandler
{
	public PlayerController controller;
	public ScoreDisplay scoreDisplay;
	public LivesDisplay livesDisplay;
	public BuffsTable buffsTable;
	
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
		//if a player is dead, use default UI positions
		if(GameManager.inst.listPlayers.Count > 1)
		{
			//if a player is respawning, skip UI updating for that frame
			if(!(GameManager.inst.listPlayers[0].isRespawing || GameManager.inst.listPlayers[1].isRespawing))
			{
				//the game is in multiplayer, determine if my UI should be on the right or left

					Controller otherPlayer;
					if(GameManager.inst.listPlayers[0] == controller)
					{
						otherPlayer = GameManager.inst.listPlayers[1];
						
					} else
					{
						otherPlayer = GameManager.inst.listPlayers[0];
					}
					//if players are in different rooms, use room location
					if(controller.pawn.roomLocation != otherPlayer.pawn.roomLocation)
					{
						//if im the one on the left
						if(controller.pawn.roomLocation.x < otherPlayer.pawn.roomLocation.x)
						{
							boundUI.SetSide(true);
						} else
						{
							//if I'm the one on the right
							if(controller.pawn.roomLocation.x > otherPlayer.pawn.roomLocation.x)
							{
								boundUI.SetSide(false);
							} else
							{
								//players have the same roomLocation, override to default positions.
								GameManager.inst.OverrideUIPositions();
							}

						}
					} else 
					{
						//if they are in the same room, use precise location
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
		buffsTable = boundUI.buffsTable;
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

	public override void AddBuff(Powerup powerup)
	{
		if(buffsTable != null)
		{
			buffsTable.Add(powerup);
		}
	}
	public override void LoseLife()
	{
		if(livesDisplay != null)
		{
			livesDisplay.Decrement();
		}
		ClearBuffs();
	}

	public void ClearBuffs()
	{
		buffsTable.ClearBuffs();
	}

}
