using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : Damage
{
	
	//for objects which you know will only have one attack, defining a damage variable is acceptable
	public float damage;
	public Projectile proj;
    // Start is called before the first frame update
    void Start()
    {
        proj = GetComponent<Projectile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void awake() 
	{
		//bullets have no health
		proj = GetComponent<Projectile>();
	}

	public override void OnHit(Damage attacker, float dmg) 
	{
		//team check, all on hit functionality added should have its own team check
	}
	/*
	gonna test to see if I need this
	public override void attack(GameObject target, float dmg)
	{
		base.attack();
	}
	*/

	public void OnTriggerEnter(Collider collider)
	{
		//ignores collision in the following cases:
		//if what it is colliding with is a projectile 
		Projectile colliderProj = collider.gameObject.GetComponent<Projectile>();
		if(colliderProj == null) 
		{
			
			//Debug.Log(collider.gameObject);
			//Debug.Log(proj);

			//if what it is colliding with is it's shooter
			if(collider.gameObject != proj.shooter.pawn.gameObject) 
			{
				//Debug.Log("COLLIDING!!");
				Attack(collider.gameObject, damage);
			}
			//destroyed on any collision
			Destroy(gameObject);
		}
		
		
	}
}
