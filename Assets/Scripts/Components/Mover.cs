using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Mover : MonoBehaviour
{
    public abstract void Start();
    public abstract void Move(bool forward);
	public abstract void Move(bool forward, float speed);
    public abstract void Turn(bool clockwise);
	public abstract void RotateTowards(Vector3 targetPos);
	//public abstract void LoopRotation();
}
