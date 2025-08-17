using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
	public HealthDisplay healthDisplay;
	public PlayerUI boundUI;
	public virtual void BindDisplay ()
	{

	}
	public virtual void DisplayDefaultValues()
	{

	}

	public virtual void UpdateScore(float score)
	{

	}

	public virtual void LoseLife()
	{

	}

	public void OnDestroy()
	{
		if(boundUI != null)
		{
			boundUI.gameObject.SetActive(false);
		}
	}
}
