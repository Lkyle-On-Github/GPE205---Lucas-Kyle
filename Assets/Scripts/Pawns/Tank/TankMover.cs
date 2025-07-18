using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{
    Rigidbody rb;
    Pawn pawn;
    private int dir;
    // Start is called before the first frame update
    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        pawn = GetComponent<Pawn>();
    }

    //
    public override void Move(bool forward) {
        dir = (forward) ? 1 : -1;
        rb.MovePosition(rb.position + (transform.forward * pawn.moveSpeed * Time.deltaTime * dir));
    }

    public override void Turn(bool clockwise) {
        dir = (clockwise) ? -1 : 1;
        transform.Rotate(0, pawn.turnSpeed * dir * Time.deltaTime, 0);
    }

	public override void RotateTowards(Vector3 targetPos) 
	{
		// Filter out the target's y position.
		Vector3 filteredTargetPos = new Vector3(targetPos.x, this.transform.position.y, targetPos.z);
		//find the vector and rotation to target
        Vector3 vectorToTarget = targetPos - transform.position;
		Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
		// Rotate closer to that vector, but don't rotate more than our turn speed allows in one frame
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pawn.turnSpeed * Time.deltaTime);

	}

}
