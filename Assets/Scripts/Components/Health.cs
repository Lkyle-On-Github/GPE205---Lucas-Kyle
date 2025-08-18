using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health : MonoBehaviour
{
	public float maxHp;
	public float hp;
	public Pawn pawn;
    // Start is called before the first frame update
    void Start()
    {
        //hp = MaxHp;
		pawn = GetComponent<Pawn>();
		UpdateHealthBar();
    }

	void Awake()
	{
		pawn = GetComponent<Pawn>();
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public virtual void Die() 
	{
		//attempt to give the controller that killed this pawn it's score
		if(pawn != null && pawn.lastAttacker != null && pawn.lastAttacker.pawn != null && pawn.lastAttacker.pawn.controller != null)
		{
			pawn.lastAttacker.pawn.controller.GainScore(pawn.killScore);
		}
		//check if this is a pawn, and if so inform it's controller
		if(pawn != null) 
		{
			//I think this second check has to be inside the first one to prevent null reference exception
			if(pawn.controller != null) 
			{
				pawn.controller.OnPawnDeath();
			}
			GameManager.inst.SpawnSoundEffect(pawn.deathSound, transform.position);
		}
		
		Destroy(gameObject);
	}

	public virtual void TakeDamage(float dmg)
	{
		hp -= dmg;
		MaxHpCheck();
		UpdateHealthBar();
		if(hp <= 0) 
		{
			Die();
		}
	}

	public virtual void TakeHealing(float healing)
	{
		hp += healing;
		MaxHpCheck();
		UpdateHealthBar();
		if(hp <= 0) 
		{
			Die();
		}
	}

	//you might want to call this whenever you do a healing effect, to ensure they dont go over their max hp.
	public virtual void MaxHpCheck()
	{
		hp = Mathf.Clamp(hp, 0, maxHp);
	}

	public virtual void UpdateHealthBar()
	{
		if(pawn.healthDisplay != null)
		{
			pawn.healthDisplay.SetHealth(hp);
		} else
		{
			//check with the controller if it has a health display for the pawn to use
			if(pawn.controller.FetchHealthDisplay())
			{
				pawn.healthDisplay.SetHealth(hp);
			}  else
			{
				//otherwise, default to the local health display
				pawn.healthDisplay = pawn.LocalHealthDisplay.GetComponent<HealthDisplay>();
				pawn.healthDisplay.gameObject.SetActive(true);
			}
		}
	}
}
