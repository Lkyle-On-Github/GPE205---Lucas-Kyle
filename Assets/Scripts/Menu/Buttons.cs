using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
	public AudioClip buttonPressSound;
	//Bro how the fuck do u turn off a button
	public GameObject startButtonObject;
	public GameObject multiplayerButtonObject;
	public GameObject creditsButtonObject;
	public GameObject settingsButtonObject;
	public GameObject backButtonObjectAsd;
	public GameObject quitButtonObject;
	public GameObject titleScreen;
	public GameObject creditsScreen;
	public GameObject settingsScreen;
	public GameObject controlsScreen;
	public GameObject gameOverScreen;
	public Text gameOverScore;

	public enum MenuStates {Title, Main, Credits, Settings, Game, GameOver};
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
	public void ButtonPressedSound()
	{
		GameManager.inst.SpawnSoundEffect(buttonPressSound, GameManager.inst.transform.position);
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
		SwapState(MenuStates.Game, false);
		//DisableBackButton();
		ButtonPressedSound();
	}

	public void MultiplayerButton()
	{
		GameManager.inst.multiplayer = true;
		SwapState(MenuStates.Game, false);
		ButtonPressedSound();
	}

	public void CreditsButton()
	{
		//EnableBackButton();
		SwapState(MenuStates.Credits, true);
		DisableButtons(menuButtonObjects);
		ButtonPressedSound();
	}

	public void SettingsButton()
	{
		//EnableBackButton();
		settingsScreen.SetActive(true);
		GameManager.inst.SwapState(GameManager.GameStates.Options);
		SwapState(MenuStates.Settings, true);
		DisableButtons(menuButtonObjects);
		ButtonPressedSound();
	}

	public void ControlsButton()
	{
		settingsScreen.SetActive(false);
		backButtonObjectAsd.SetActive(false);
		controlsScreen.SetActive(true);
	}
	public void ControlsBackButton()
	{
		settingsScreen.SetActive(true);
		backButtonObjectAsd.SetActive(true);
		controlsScreen.SetActive(false);
	}

	public void QuitButton()
	{
		Application.Quit();
		UnityEditor.EditorApplication.isPlaying = false;
	}

	public void BackButton()
	{
		if(returnState != MenuStates.Game)
		{
			EnableButtons(menuButtonObjects);
		} else
		{
			settingsButtonObject.SetActive(true);
		}
		SwapState(returnState, false);
		//
		DisableBackButton();
		ButtonPressedSound();
		
	}

	public void RestartButton()
	{
		//swapping from gameover state will delete the level, and swapping gamemanager to game state will generate a new level.
		SwapState(MenuStates.Game, false);
		settingsButtonObject.SetActive(true);
		ButtonPressedSound();
	}

	public void MenuButton()
	{
		SwapState(MenuStates.Main, false);
		//swapping from gameover state deletes the level.
		ButtonPressedSound();
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
				controlsScreen.SetActive(false);
				//DisableButtons(arrayThatJustHoldsTheUselessObjectThatHoldsTheBackButtonBecauseTheButtonsInThisProgramAreAPieceOfShit);
				break;
			case MenuStates.Main:
				EnableButtons(menuButtonObjects);
				GameManager.inst.SwapState(GameManager.GameStates.MainMenu);
				quitButtonObject.SetActive(true);
				break;
			case MenuStates.Credits:
				creditsScreen.gameObject.SetActive(true);
				break;
			case MenuStates.Settings:
				
				break;
			case MenuStates.Game:
				GameManager.inst.SwapState(GameManager.GameStates.Game);
				DisableButtons(new List<GameObject>{startButtonObject, multiplayerButtonObject, creditsButtonObject});
				break;
			case MenuStates.GameOver:
				settingsButtonObject.SetActive(false);
				gameOverScreen.gameObject.SetActive(true);
				quitButtonObject.SetActive(true);
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
				quitButtonObject.SetActive(false);
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
			case MenuStates.GameOver:
				gameOverScreen.gameObject.SetActive(false);
				GameManager.inst.RunMapDestruction();
				quitButtonObject.SetActive(false);
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
