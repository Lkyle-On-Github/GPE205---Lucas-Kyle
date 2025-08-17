using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesDisplay : MonoBehaviour
{
	public Image lifeOne;
	public Image lifeTwo;
	public Image lifeThree;
	public int lives;
    // Start is called before the first frame update
    void Start()
    {
        //lifeOne = new Image();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	//in most cases increment lives should be used as it is more efficient
	public void SetLives(int newLives)
	{
		switch(newLives)
		{
			case 0:
				lifeOne.gameObject.SetActive(false);
				lifeTwo.gameObject.SetActive(false);
				lifeThree.gameObject.SetActive(false);
				lives = 0;
				break;
			case 1:
				lifeOne.gameObject.SetActive(true);
				lifeTwo.gameObject.SetActive(false);
				lifeThree.gameObject.SetActive(false);
				lives = 1;
				break;
			case 2:
				lifeOne.gameObject.SetActive(true);
				lifeTwo.gameObject.SetActive(true);
				lifeThree.gameObject.SetActive(false);
				lives = 2;
				break;
			case 3:
				lifeOne.gameObject.SetActive(true);
				lifeTwo.gameObject.SetActive(true);
				lifeThree.gameObject.SetActive(true);
				lives = 3;
				break;
		}
	}
	public void Decrement()
	{
		switch(lives)
		{
			case 3:
				lifeThree.gameObject.SetActive(false);
				break;
			case 2:
				lifeTwo.gameObject.SetActive(false);
				break;
			case 1:
				lifeOne.gameObject.SetActive(false);
				break;
			case 0:
				Debug.Log("object died with no lives");
				break;

		}
		lives -= 1;
	}
	public void ScreenSide(bool left)
	{
		if(left)
		{
			transform.localPosition = new Vector3(-214, -190, 0);
		} else
		{
			transform.localPosition = new Vector3(214, -190, 0);
		}
	}
}
