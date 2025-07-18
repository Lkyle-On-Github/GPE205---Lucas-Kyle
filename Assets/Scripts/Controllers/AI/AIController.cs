using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        //MakeDecisions();
        
		base.Update();
    }

    //public void MakeDecisions()
    //{
    //    Debug.Log("Making Decisions");
    //}

	
	public float lastStateSwapTime;
	
	//public enum States {StateZero, StateOne, StateTwo, StateThree, StateFour};
	//I'm gonna put this code in here because this is how I design the AI in my games but I can remove it if the instructions aren't compatible

	//I have to comment all of this out cuz otherwise im like serializing the same variable twice or something and its really annoying but whatever
	/*
	public States state;


	public virtual void MakeDecisions() 
	{
		switch (state) 
		{
			case States.StateZero:

				break;
		}
	}

	public virtual void StateStart() 
	{
		//this is how I write my switch statements it makes it easier for me to read but I can change it if you really need me to
		switch (state)
		{
			case States.StateZero:

				break;
		}
	}
	public virtual void StateEnd()
	{
		switch (state)
		{
			case States.StateZero:

				break;
		}
	}
	public virtual void SwapState(States newState)
	{
		StateEnd();
		state = newState;
		StateStart();

		lastStateSwapTime = Time.time;
	}
	*/

}
