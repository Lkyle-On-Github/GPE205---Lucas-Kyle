using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuffDisplay : MonoBehaviour
{
	public Image powerupImage;
	public Image buffBar;
	public RectTransform barTransform;
	public Powerup powerup;
    public BuffsTable buffsTable;

	
	void Update()
	{
		if(powerup != null)
		{
			barTransform.localScale = new Vector3(powerup.duration/(powerup.displayMaxDuration), 2, 1);
		}
		if(powerup.duration <= 0)
		{
			Destroy(gameObject);
		}
	}
    public void ScreenSide(bool left)
	{

		RectTransform barTransform = buffBar.GetComponent<RectTransform>();
		if(left)
		{
			

			//placing and setting the scale mode of the bar
			barTransform.pivot = new Vector2(0, 0.5f);
			barTransform.anchoredPosition = new Vector2(34, 5);
		} else
		{
			

			//placing and setting the scale mode of the bar
			barTransform.pivot = new Vector2(1, 0.5f);
			barTransform.anchoredPosition = new Vector2(-34, 5);
		}
	}

	void OnDestroy()
	{
		if(buffsTable != null)
		{
			buffsTable.listBuffs.Remove(this);
			buffsTable.UpdateVerticalPositions();
		}
	}
}
