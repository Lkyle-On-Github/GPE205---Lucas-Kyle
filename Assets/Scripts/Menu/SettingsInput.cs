using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInput : MonoBehaviour
{
	public Toggle seededCheckBox;
	public InputField seededInputField;
	public Toggle dailyCheckBox;

	public Slider masterSlider;
	public Slider soundSlider;
	public Slider musicSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(GameManager.inst.hasMapGenerator.GetComponent<MapGenerator>().mapExists)
		{
			seededCheckBox.interactable = false;
			dailyCheckBox.interactable = false;
			seededInputField.interactable = false;
		} else
		{
			seededCheckBox.interactable = true;
			dailyCheckBox.interactable = true;
        	seededInputField.interactable = seededCheckBox.isOn;
		}

    }

	bool SeededCheckBox()
	{
		//seededCheckBox.isOn = false;
		return true;
	}

	void DailyCheckBox(bool isOn)
	{

	}

}
