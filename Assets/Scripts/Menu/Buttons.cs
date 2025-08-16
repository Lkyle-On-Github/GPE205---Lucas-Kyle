using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
	//Bro how the fuck do u turn off a button
	public GameObject startButtonObject;
	public GameObject multiplayerButtonObject;
	public GameObject creditsButtonObject;
	public GameObject settingsButtonObject;
	public GameObject backButtonObjectAsd;
	public GameObject titleScreen;
	public GameObject creditsScreen;
	public GameObject settingsScreen;

	public enum MenuStates {Title, Main, Credits, Settings, Game};
	public MenuStates menuState;
	private MenuStates returnState;

	public List<GameObject> menuButtonObjects;
	//upublic List<GameObject> arrayThatJustHoldsTheUselessObjectThatHoldsTheBackButtonBecauseTheButtonsInThisProgramAreAPieceOfShit;
	//I have never used this once in any script
	float lastStateSwapTime;
    // Start is called before the first frame update
    void Start()
    {
        StateStart();
    }	

    // Update is called once per frame
    void Update()
    {
        
    }
	public void EnableBackButton()
	{
		backButtonObjectAsd.SetActive(true);
		returnState = menuState;
		
	}
	public void DisableBackButton()
	{
		backButtonObjectAsd.SetActive(false);
		returnState = menuState;
		
	}
	public void SetButton(bool active)
	{
		if(active)
		{
			
		}
	}

	public void EnableButtons(List<GameObject> buttons)
	{
		foreach (GameObject button in buttons)
		{
			button.SetActive(true);
		}
	}

	public void DisableButtons(List<GameObject> buttons)
	{
		foreach (GameObject button in buttons)
		{
			button.SetActive(false);
		}
	}

	public void StartButton()
	{
		GameManager.inst.multiplayer = false;
		GameManager.inst.SwapState(GameManager.GameStates.Game);
		SwapState(MenuStates.Game, false);
		//DisableBackButton();
	}

	public void MultiplayerButton()
	{
		GameManager.inst.multiplayer = true;
		GameManager.inst.SwapState(GameManager.GameStates.Game);
		SwapState(MenuStates.Game, false);
	}

	public void CreditsButton()
	{
		//EnableBackButton();
		SwapState(MenuStates.Credits, true);
		DisableButtons(menuButtonObjects);
	}

	public void SettingsButton()
	{
		//EnableBackButton();
		settingsScreen.SetActive(true);
		SwapState(MenuStates.Settings, true);
		DisableButtons(menuButtonObjects);
	}

	public void BackButton()
	{
		
		EnableButtons(menuButtonObjects);
		SwapState(returnState, false);
		DisableBackButton();
		
	}

//I should have put the state machine for the menu in a different script, lesson learned.
	protected virtual void StateStart() 
	{
		//this is how I write my switch statements it makes it easier for me to read but I can change it if you really need me to
		switch (menuState)
		{
			case MenuStates.Title:
				titleScreen.SetActive(true);
				backButtonObjectAsd.SetActive(false);
				DisableButtons(menuButtonObjects);
				//DisableButtons(arrayThatJustHoldsTheUselessObjectThatHoldsTheBackButtonBecauseTheButtonsInThisProgramAreAPieceOfShit);
				break;
			case MenuStates.Main:
				EnableButtons(menuButtonObjects);
				break;
			case MenuStates.Credits:
				creditsScreen.gameObject.SetActive(true);
				break;
			case MenuStates.Settings:

				break;
			case MenuStates.Game:
				DisableButtons(new List<GameObject>{startButtonObject, multiplayerButtonObject, creditsButtonObject});
				break;
		}
	}
	protected virtual void StateEnd()
	{
		switch (menuState)
		{
			case MenuStates.Title:
				titleScreen.SetActive(false);
				break;
			case MenuStates.Main:

				break;
			case MenuStates.Credits:
			//DisableButtons(new List<GameObject>{backButtonObject});
				creditsScreen.SetActive(false);
				break;
			case MenuStates.Settings:
				settingsScreen.SetActive(false);
			//DisableButtons(new List<GameObject>{backButtonObject});
				break;
			case MenuStates.Game:

				break;
		}
	}
	public virtual void SwapState(MenuStates newState, bool backButton)
	{
		
		if(menuState != newState)
		{
			//unique state functionality to suit the menu better
			if(backButton)
			{
				EnableBackButton();
			}
			StateEnd();
			menuState = newState;
			StateStart();

			lastStateSwapTime = Time.time;
		}
	}
}
