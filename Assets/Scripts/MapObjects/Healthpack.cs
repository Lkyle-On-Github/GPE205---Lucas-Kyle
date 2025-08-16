using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthpack : MonoBehaviour
{
	public float dealtHealing;
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
		GameManager.inst.SpawnSoundEffect(audioClip, transform.position);
		Health colliderHealth = collider.gameObject.GetComponent<Health>();
		//Debug.Log(colliderHealth != null);
		if(colliderHealth != null)
		{
			
			colliderHealth.TakeHealing(dealtHealing);
			Destroy(this.gameObject);
		}
		//Debug.Log("collider health was not null!");
		
	}
}
