using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
	public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        ScreenSide(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	//whether the bar is on the left or right of the heart, and the direction it goes down
	public void ScreenSide(bool left)
	{

		RectTransform barTransform = healthBar.GetComponent<RectTransform>();
		if(left)
		{
			//moving the heart and object into the right screen position
			transform.localPosition = new Vector3(-376, 190, 0);

			//placing and setting the scale mode of the bar
			barTransform.pivot = new Vector2(0, 0.5f);
			barTransform.anchoredPosition = new Vector2(45, 5);
		} else
		{
			//moving the heart and object into the right screen position
			transform.localPosition = new Vector3(376, 190, 0);

			//placing and setting the scale mode of the bar
			barTransform.pivot = new Vector2(1, 0.5f);
			barTransform.anchoredPosition = new Vector2(-45, 5);
		}
	}
	public virtual void SetHealth(float hp)
	{
		//hp / 5 = scale
		RectTransform barTransform = healthBar.GetComponent<RectTransform>();
		barTransform.localScale = new Vector3(hp/5, 2, 1);
	}
}
