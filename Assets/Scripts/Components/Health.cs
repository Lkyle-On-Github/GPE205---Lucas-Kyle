using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health : MonoBehaviour
{
	public float maxHp;
	public float hp;
    // Start is called before the first frame update
    void Start()
    {
        //hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public virtual void Die() 
	{
		//check if this is a pawn, and if so inform it's controller
		Pawn pawn = GetComponent<Pawn>();
		if(pawn != null) 
		{
			//I think this second check has to be inside the first one to prevent null reference exception
			if(pawn.controller != null) 
			{
				pawn.controller.OnPawnDeath();
			}
		}
		Destroy(gameObject);
	}

	public virtual void TakeDamage(float dmg)
	{
		hp -= dmg;
		MaxHpCheck();
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
}
