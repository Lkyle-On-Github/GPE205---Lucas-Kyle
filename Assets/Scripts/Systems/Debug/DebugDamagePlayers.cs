using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDamagePlayers : MonoBehaviour
{
	public KeyCode p1DamageKey;
	public KeyCode p2DamageKey;
	//public KeyCode allEnemiesKey;
	public KeyCode damageIncreaseKey;
	public KeyCode damageDecreaseKey;
	int dmg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(p1DamageKey))
		{
			GameManager.inst.listPlayers[0].pawn.health.TakeDamage(dmg);
		}
		if(Input.GetKeyDown(p2DamageKey))
		{
			GameManager.inst.listPlayers[1].pawn.health.TakeDamage(dmg);
		}
		/*
		if(Input.GetKeyDown(allEnemiesKey))
		{
			foreach (Pawn pawn in GameManager.inst.listPawns)
			{
				if(pawn.spawnpoint == null)
				{
					pawn.health.TakeDamage(dmg);
				}
			}
		}
		*/
		if(Input.GetKeyDown(damageIncreaseKey))
		{
			dmg +=1;
			Debug.Log("Will do " + dmg + " damage to player");
		}
		if(Input.GetKeyDown(damageDecreaseKey))
		{
			dmg -=1;
			Debug.Log("Will do " + dmg + " damage to player");
		}
    }



}
