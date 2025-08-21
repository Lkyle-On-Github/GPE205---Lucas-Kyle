using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ControlSetButton : MonoBehaviour
{
	//since it is initialized before the player exists, it cant use a player reference
	public bool isP1;
	public Controls control;
	public GameManager.PlayerKeys keyBinding;
	public KeyCode defaultKey;
	public HUDController hudController;
	public Image secretInputDisabler;
	public AudioClip usedKeyFeedback;
	private Text text;
	//used for the string
	private KeyCode currKey;
	private bool isListening;

	public List<KeyCode> invalidKeys;
	private List<KeyCode> usedKeys;
    // Start is called before the first frame update
    public void Start()
	{
		
		usedKeys = new List<KeyCode>();
		text = Instantiate(hudController.defaultText, gameObject.transform);
		text.fontSize = 40;
		text.text = new string(keyBinding + "\n" + defaultKey);
		//text.rectTransform.localPosition = new Vector3(0, -20, 0);
		text.gameObject.SetActive(true);
		GameManager.inst.UpdateControls(isP1, keyBinding, defaultKey);
	}

	public void Update()
	{
		
	}

	public void OnClick()
	{
		isListening = true;
		secretInputDisabler.gameObject.SetActive(true);
		text.text = new string("Press Key");
	}

	void OnGUI()
	{
		//when a button is pressed
		if((Event.current.isKey) && isListening == true)
		{
			//if button is on invalid list, end event
			if(invalidKeys.Contains(Event.current.keyCode))
			{
				isListening = false;
				secretInputDisabler.gameObject.SetActive(false);
				text.text = new string(keyBinding + "\n" + currKey);
			} else
			{
				//if button is already in use, inform the player of this without ending the event
				//CompileUsedKeys();
				//if(usedKeys.Contains(Event.current.keyCode))
				//{
					//GameManager.
				//} else
				//{
				//ive decided that keys can overlap cuz I think its fun when games let u do that
					currKey = Event.current.keyCode;
					text.text = new string(keyBinding + "\n" + currKey);
					GameManager.inst.UpdateControls(isP1, keyBinding, currKey);
					isListening = false;
					secretInputDisabler.gameObject.SetActive(false);
				//}
			}
		}
	}

	void CompileUsedKeys()
	{
		usedKeys.Clear();
		foreach(KeyCode key in GameManager.inst.p1Controls.GetListKeys())
		{
			usedKeys.Add(key);
		}
		foreach(KeyCode key in GameManager.inst.p2Controls.GetListKeys())
		{
			usedKeys.Add(key);
		}
	}
}
