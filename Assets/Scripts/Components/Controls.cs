using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
//A list of controls that can be automatically applied to a player controller
public class Controls
{
	//how do I integrate this with the Enum
	private List<GameManager.PlayerKeys> listAllKeys;
	private List<KeyCode> listBoundKeys;
	
	public void InitControls()
	{
		listAllKeys = new List<GameManager.PlayerKeys>();
		listBoundKeys = new List<KeyCode>();
		foreach(string s in Enum.GetNames(typeof(GameManager.PlayerKeys)))
		{
			listAllKeys.Add(Enum.Parse<GameManager.PlayerKeys>(s));
			listBoundKeys.Add(KeyCode.None);
		}

	}

	public void SetPlayerControls(PlayerController player)
	{
		for(int i = 0; i < listAllKeys.Count; i++)
		{
			player.UpdateKey(listAllKeys[i], listBoundKeys[i]);
		}
	}

	public void ChangeKey(GameManager.PlayerKeys toChange, KeyCode newKey)
	{
		listBoundKeys[listAllKeys.IndexOf(toChange)] = newKey;
	}

	public List<KeyCode> GetListKeys()
	{
		return listBoundKeys;
	}
}
