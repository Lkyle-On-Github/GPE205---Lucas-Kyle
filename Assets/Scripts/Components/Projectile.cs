using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
	public float speed;
	public Controller shooter;
	public float awakeTime;
	//time in seconds until bullet disapears
	public float decayTime = 2;
	
	public virtual void Update() 
	{
		//if(Time.time - Time.fixedTime > decayTime) 
		if(Time.time - awakeTime > decayTime)
		{
			Destroy(gameObject);
		}
	}

    public virtual void Awake() 
	{
		rb = GetComponent<Rigidbody>();
		awakeTime = Time.time;
		
	}
	
    // Update is called once per frame

	public void Shoot(float force) 
	{
		rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
	}
}
