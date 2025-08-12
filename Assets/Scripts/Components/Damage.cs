using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
	public Health health;
	public Pawn pawn;
	public bool damageIgnore;

	//0 is player team, 1 is enemy team
	public int team;


    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
		pawn = GetComponent<Pawn>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


	//public abstract void attack();

	#region why orange ew gross
	//must be overriden if object doesnt have health

	public virtual void awake() 
	{
		health = GetComponent<Health>();
	}

	
	//called when an object gets hit
	public virtual void OnHit(Damage attacker, float dmg) 
	{
		//Debug.Log(this);
		if(pawn != null) 
		{
			pawn.lastAttacker = attacker;
			pawn.MakeNoise(GameManager.Noises.Hit);
		}
		this.health.TakeDamage(dmg);
			
			//death check
			
	}

	public virtual void Instakill(GameObject target) 
	{
		Health targetHealth = target.GetComponent<Health>();
		targetHealth.Die();
	}
	#endregion
	
	//this should make attacking something easier and more intuitive I hope
	//returns true if the attack goes through
	public bool Attack(GameObject target, float dmg)
	{
		Damage targetDamage = target.GetComponent<Damage>();
		if(targetDamage != null)
		{
			//team check, alternate modes of attacking should have their own team check
			if(targetDamage.team != this.team) 
			{	
				targetDamage.OnHit(this, dmg);
				return true;
			} 
		}
		return false;
	}
	public bool Attack(GameObject target, float dmg, Damage attacker)
	{
		Damage targetDamage = target.GetComponent<Damage>();
		if(targetDamage != null)
		{
			//team check, alternate modes of attacking should have their own team check
			if(targetDamage.team != this.team) 
			{	
				targetDamage.OnHit(attacker, dmg);
				return true;
			} 
		}
		return false;
	}
}
