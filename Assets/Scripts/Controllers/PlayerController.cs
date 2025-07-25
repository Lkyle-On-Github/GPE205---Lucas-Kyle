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
    // Start is called before the first frame update
    public override void Start()
    {
		//add self to player list
	    //I kinda feel like its better if the game crashes with a null reference exception if the gamemanager doesnt exist so that I can read the error message and know what the problem is, since the entire game wouldnt function without the manager anyways.
		if (GameManager.inst.listPlayers != null) 
		{
				GameManager.inst.listPlayers.Add(this);
		}
		

		base.Start();
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

	public void OnDestroy()
	{

		//Remove from player list
		if (GameManager.inst.listPlayers != null) 
		{
				GameManager.inst.listPlayers.Remove(this);
		}
		base.OnDestroy();
	}
}
