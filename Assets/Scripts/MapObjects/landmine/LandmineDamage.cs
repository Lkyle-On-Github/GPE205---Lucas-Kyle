using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmineDamage : Damage
{
	
	//for objects which you know will only have one attack, defining a damage variable is acceptable
	public float damage;
	public Landmine mine;
    // Start is called before the first frame update
    void Start()
    {
		//mine = GetComponent<Landmine>();
		team = -1;
		damage = mine.dealtDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void awake() 
	{
		team = -1;
		damage = mine.dealtDamage;
	}

	public override void OnHit(Damage attacker, float dmg) 
	{
		//team check, all on hit functionality added should have its own team check
	}
	/*
	
	*/
	
	public void OnTriggerEnter(Collider collider)
	{
		//isnt on any team, can damage anyone.
		
		//ignores collision in the following cases:
		//if what it is colliding with is a projectile 
		//Projectile colliderProj = collider.gameObject.GetComponent<Projectile>();
		Pawn colliderPawn = collider.gameObject.GetComponent<Pawn>();
		Damage colliderDamage = collider.gameObject.GetComponent<Damage>();
		if(colliderPawn != null) 
		{
			
			//Debug.Log(collider.gameObject);
			//Debug.Log(proj);

			//if what it is colliding with is it's shooter
			//if(mine.placer.pawn == null || collider.gameObject != mine.placer.gameObject) 
			
				//Debug.Log("COLLIDING!!");
			if (Attack(collider.gameObject, damage))
			{
				Destroy(gameObject);
			}
		}
		
	}
}