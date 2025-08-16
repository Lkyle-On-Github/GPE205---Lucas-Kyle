using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoost : MonoBehaviour
{
	public float value;
	public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter(Collider collider)
	{
		Pawn colliderPawn = collider.gameObject.GetComponent<Pawn>();
		
		//Debug.Log(colliderHealth != null);
		//only a player can get a score pickup
		if(colliderPawn != null && GameManager.inst.listPlayers.Contains(colliderPawn.controller as PlayerController))
		{
			GameManager.inst.SpawnSoundEffect(audioClip, transform.position);
			colliderPawn.controller.GainScore(value);
			Destroy(this.gameObject);
		}
		//Debug.Log("collider health was not null!");
		
	}
}
