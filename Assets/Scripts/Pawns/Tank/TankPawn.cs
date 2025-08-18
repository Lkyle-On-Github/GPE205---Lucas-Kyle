using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    Rigidbody rb;
    //rigidbody = GetComponent<Rigidbody>();
    
	

    Vector3 moveVector = Vector3.forward;

	NoiseMaker treadsNoisemaker;
	NoiseMaker shotNoisemaker;
	NoiseMaker hitNoisemaker;


    // Start is called before the first frame update
    public override void Awake()
    {
		base.Awake();
        //base.Start();
        rb = GetComponent<Rigidbody>();
        mover = GetComponent<Mover>();
		shooter = GetComponent<Shooter>();
		damage = GetComponent<Damage>();
		health = GetComponent<Health>();

		treadsNoisemaker = GetComponent<TreadsNoisemaker>();
		shotNoisemaker = GetComponent<ShotNoisemaker>();
		hitNoisemaker = GetComponent<HitNoisemaker>();
		
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

	public override void RotateTowardsPoint(Vector3 target)
	{
		mover.RotateTowards(target);
	}

    public override void MoveForward()
    {
        mover.Move(true);
    }

	public override void MoveForward(float speed)
	{
		mover.Move(true, speed);
	}

    public override void MoveBackward()
    {
        mover.Move(false);
    }

	public override void Shoot()
	{
		shooter.FireTankBullet(damage.team);
	}
	#region Seek
	public override void Seek(Vector3 target)
	{
		mover.RotateTowards(target);
		mover.Move(true);
	}
	public override void Seek(Transform target)
	{

		mover.RotateTowards(target.position);
		mover.Move(true);
	}
	public override void Seek(GameObject target)
	{
		mover.RotateTowards(target.transform.position);
		mover.Move(true);
	}
	public override void Seek(Pawn target)
	{

		mover.RotateTowards(target.transform.position);
		mover.Move(true);
	}
	public override void Seek(Controller target)
	{
		mover.RotateTowards(target.pawn.transform.position);
		mover.Move(true);
	}
	#endregion

	public override void MakeNoise(GameManager.Noises noise)
	{
		switch (noise)
		{
			case GameManager.Noises.Movement:
				treadsNoisemaker.StartNoise(transform.position);
				break;
			case GameManager.Noises.Shot:
				shotNoisemaker.StartNoise(transform.position);
				break;
			case GameManager.Noises.Hit:
				hitNoisemaker.StartNoise(transform.position);
				break;
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		
	}
}