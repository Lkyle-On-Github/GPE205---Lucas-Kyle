using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuffsTable : MonoBehaviour
{
	public BuffDisplay preBuffDisplay;
	public List<BuffDisplay> listBuffs;
	public PlayerUI playerUI;
	public Sprite healthImage;
	public Sprite speedImage;
	public Sprite damageImage;

	public float vOffset;
    // Start is called before the first frame update
    void Start()
    {
		listBuffs = new List<BuffDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ScreenSide(bool left)
	{
		if(left)
		{
			transform.localPosition = new Vector3(-395, 66, 0);
		} else
		{
			transform.localPosition = new Vector3(395, 66, 0);
		}
		foreach(BuffDisplay buffDisplay in listBuffs)
		{
			buffDisplay.ScreenSide(left);
		}
	}

	public void Add(Powerup powerup)
	{
		//listBuffs.Add(powerup);
		//create 
		BuffDisplay newPowerup = Instantiate(preBuffDisplay, this.transform);
		newPowerup.powerup = powerup;
		newPowerup.ScreenSide(playerUI.isLeft);
		newPowerup.buffsTable = this;
		if(powerup as HealthPowerup != null)
		{
			newPowerup.powerupImage.sprite = healthImage;
		}
		if(powerup as DamagePowerup != null)
		{
			newPowerup.powerupImage.sprite = damageImage;
		}
		if(powerup as SpeedPowerup != null)
		{
			newPowerup.powerupImage.sprite = speedImage;
		}
		powerup.displayMaxDuration = powerup.duration;
		listBuffs.Add(newPowerup);
		UpdateVerticalPositions();
	}

	public void UpdateVerticalPositions()
	{
		for(int i = 0; i < listBuffs.Count; i++)
		{
			listBuffs[i].transform.localPosition = new Vector3(listBuffs[i].transform.localPosition.x, i * vOffset, listBuffs[i].transform.localPosition.z);
		}
	}

	public void ClearBuffs()
	{
		foreach (BuffDisplay buff in listBuffs)
		{
			Destroy(buff.gameObject);
		}
		listBuffs.Clear();
	}
}
