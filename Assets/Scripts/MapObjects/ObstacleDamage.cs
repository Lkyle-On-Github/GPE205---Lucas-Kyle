using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamage : Damage
{
    // Start is called before the first frame update
    //for objects which you know will only have one attack, defining a damage variable is acceptable
	public float damage;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void awake() 
	{
		//obstacles have no health

	}

	public override void OnHit(Damage attacker, float dmg) 
	{
		//team check, all on hit functionality added should have its own team check
	}
	

	public void OnCollisionEnter(Collision collision)
	{
		//Debug.Log("COLLIDING!!");
		Attack(collision.gameObject, damage);
	}
}
