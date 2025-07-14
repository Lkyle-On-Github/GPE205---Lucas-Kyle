using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    //how fast the object moves
    public float moveSpeed;
    //how fast the obect turns
    public float turnSpeed;
	//the controller associated with this pawn (needed in order to feed the bullet this info)
	public Controller controller;

    // Start is called before the first frame update
    public virtual void Start()
    {
		if (GameManager.inst.listPawns != null) 
		{
				GameManager.inst.listPawns.Add(this);
		}
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public abstract void RotateClockwise();
    public abstract void RotateCounterClockwise();
    public abstract void MoveForward();
    public abstract void MoveBackward();
	public abstract void Shoot();
    
	public void OnDestroy()
	{

		//Remove from player list
		if (GameManager.inst.listPawns != null) 
		{
				GameManager.inst.listPawns.Remove(this);
		}
	}
}
