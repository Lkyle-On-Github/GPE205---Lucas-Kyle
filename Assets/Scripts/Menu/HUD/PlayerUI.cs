using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{

	//this script exists to provide access to all of the parts of the players UI that other scripts may need to access
	//to avoid any null reference errors, **ALL** player UI elements should be driven by the PlayerController's PlayerUIHandler
	public ScoreDisplay scoreDisplay;
	public BuffsTable buffsTable;
	public LivesDisplay livesDisplay;
	public HealthDisplay healthDisplay;

	public HUDController hudController;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetSide(bool left)
	{
		scoreDisplay.ScreenSide(left);
		//buffsTable.ScreenSide(left);
		livesDisplay.ScreenSide(left);
		healthDisplay.ScreenSide(left); 
	}
}
