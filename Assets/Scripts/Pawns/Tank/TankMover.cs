using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{
    Rigidbody rb;
    Pawn tank;
    private int dir;
    // Start is called before the first frame update
    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        tank = GetComponent<Pawn>();
    }

    //
    public override void Move(bool forward) {
        dir = (forward) ? 1 : -1;
        rb.MovePosition(rb.position + (transform.forward * tank.moveSpeed * Time.deltaTime * dir));
    }

    public override void Turn(bool clockwise) {
        dir = (clockwise) ? -1 : 1;
        transform.Rotate(0, tank.turnSpeed * dir * Time.deltaTime, 0);
    }

}
