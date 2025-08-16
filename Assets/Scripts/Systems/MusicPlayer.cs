using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	public GameManager.GameStates storedState;
	public List<GameManager.GameStates> ContinuousListMenuTheme;
	private AudioSource audioSource;
	public AudioClip titleTheme;
	public AudioClip gameTheme;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
		audioSource.clip = titleTheme;
		audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
		if(storedState != GameManager.inst.gameState)
		{
	        SwapMusic(GameManager.inst.gameState);
			storedState = GameManager.inst.gameState;
	    }
	}
	public void SwapMusic(GameManager.GameStates state)
	{
		switch(state)
		{
			case GameManager.GameStates.TitleScreen:
				audioSource.clip = titleTheme;
				audioSource.Play();
				break;
			case GameManager.GameStates.MainMenu:
			if(ContinuousListMenuTheme.Contains(storedState)) 
			{

			} else
			{
				audioSource.Stop();
				audioSource.clip = titleTheme;
				audioSource.Play();
			}
				break;
			case GameManager.GameStates.Options:
			
			//now that I'm thinking about it I'm not implementing this ok I thought about it for 2 more seconds and I could make it work
				if(storedState == GameManager.GameStates.Game)
				{
					audioSource.Pause();
				}
				break;
			case GameManager.GameStates.Game:
				//if coming from the options menu, just unpause the music and skip the other steps
				if(storedState == GameManager.GameStates.Options)
				{
					audioSource.UnPause();
				}else
				{
				audioSource.Stop();
				audioSource.clip = gameTheme;
				audioSource.Play();
				}
				break;
			case GameManager.GameStates.GameOver:
				if(storedState == GameManager.GameStates.Options)
				{
					audioSource.UnPause();
				}
				break;
			case GameManager.GameStates.Credits:
				if(ContinuousListMenuTheme.Contains(storedState)) 
			{

			} else
			{
				audioSource.Stop();
				audioSource.clip = titleTheme;
				audioSource.Play();
			}
				break;
		}
	}
}
