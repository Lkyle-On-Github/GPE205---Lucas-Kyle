using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
	public Text text;
	public HUDController hudController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

	void Awake()
	{
		text = Instantiate(hudController.defaultText, gameObject.transform);
		text.rectTransform.localPosition = new Vector3(0, -20, 0);
		text.gameObject.SetActive(true);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetScore(float score)
	{
		text.text = score.ToString();
	}
	public void SetScore(string score)
	{
		text.text = score;
	}

	public void ScreenSide(bool left)
	{
		if(left)
		{
			transform.localPosition = new Vector3(-360, 140, 0);
		} else
		{
			transform.localPosition = new Vector3(360, 140, 0);
		}
	}
}
