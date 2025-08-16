using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputDetector : MonoBehaviour
{
	public Buttons menuScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetKey(KeyCode.None));
		
    }

	void OnGUI()
	{
		if((Event.current.isKey || Event.current.isMouse) && menuScript.menuState == Buttons.MenuStates.Title)
		{
			//Debug.Log("KeyEventHappening");
			GameManager.inst.SwapState(GameManager.GameStates.MainMenu);
			menuScript.SwapState(Buttons.MenuStates.Main, false);
		}
	}
}
