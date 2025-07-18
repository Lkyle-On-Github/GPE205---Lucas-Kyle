using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Controller : MonoBehaviour
{
    //the pawn instance assigned to this controller, should be initialized when this pawn is spawned
    public Pawn pawn;

    // Start is called before the first frame update
    public virtual void Start()
    {
		if (GameManager.inst.listControllers != null) 
		{
				GameManager.inst.listControllers.Add(this);
		}
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public virtual void ProcessInputs()
    {
        
    }
	//the only way I wrote it like this is because I got confused by the description of OnDeath and now I have to keep it cuz it could technically be useful
	public virtual void OnPawnDeath()
	{
		Destroy(gameObject);
	}
	public void OnDestroy()
	{

		//Remove from player list
		if (GameManager.inst.listControllers != null) 
		{
				GameManager.inst.listControllers.Remove(this);
		}
	}
}
