using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSwapGameState : MonoBehaviour
{
	//TitleScreen, MainMenu, Options, Game, GameOver, Credits
	public KeyCode TitleScreenKey;
	public KeyCode MainMenuKey;
	public KeyCode OptionsKey;
	public KeyCode GameKey;
	public KeyCode GameOverKey;
	//public KeyCode CreditsKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(TitleScreenKey))
		{
			GameManager.inst.SwapState(GameManager.GameStates.TitleScreen);
		}
		if(Input.GetKeyDown(MainMenuKey))
		{
			GameManager.inst.SwapState(GameManager.GameStates.MainMenu);
		}
		if(Input.GetKeyDown(OptionsKey))
		{
			GameManager.inst.SwapState(GameManager.GameStates.Options);
		}
		if(Input.GetKeyDown(GameKey))
		{
			GameManager.inst.SwapState(GameManager.GameStates.Game);
		}
		if(Input.GetKeyDown(GameOverKey))
		{
			GameManager.inst.SwapState(GameManager.GameStates.GameOver);
		}
		/*
		if(Input.GetKeyDown(CreditsKey))
		{
			GameManager.inst.SwapState(GameManager.GameStates.Credits);
		}
		*/
    }
}
