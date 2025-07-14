using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    Rigidbody rb;
    //rigidbody = GetComponent<Rigidbody>();
    public Mover mover;
	public Shooter shooter;
	public Damage damage;
	

    Vector3 moveVector = Vector3.forward;


    // Start is called before the first frame update
    public override void Start()
    {
        //base.Start();
        rb = GetComponent<Rigidbody>();
        mover = GetComponent<Mover>();
		shooter = GetComponent<Shooter>();
		damage = GetComponent<Damage>();

		base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void RotateClockwise()
    {
        mover.Turn(true);
    }

    public override void RotateCounterClockwise()
    {
        mover.Turn(false);
    }
    public override void MoveForward()
    {
        mover.Move(true);
    }

    public override void MoveBackward()
    {
        mover.Move(false);
    }

	public override void Shoot()
	{
		shooter.FireTankBullet(damage.team);
	}


}