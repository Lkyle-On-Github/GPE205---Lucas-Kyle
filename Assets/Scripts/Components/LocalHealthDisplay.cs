using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalHealthDisplay : HealthDisplay
{
	public Canvas canvas;
	//public Health pawnHealth;
	public Pawn pawn;
	//public Image healthBar;asd
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(GameManager.inst.hasMapGenerator.GetComponent<MapGenerator>().mapExists && pawn.roomLocation != null)
		{
			Vector3 targetDir = pawn.roomLocation.roomCamera.gameObject.transform.position - transform.position;
			//gameObject.transform.rotation = Vector3.Angle(gameObject.transform.position, pawn.roomLocation.roomCamera.transform.position);
			//Vector3.RotateTowards(transform.forward, targetDir, 180, 0);
			transform.forward = new Vector3(0, targetDir.y, targetDir.z);
			//gameObject.transform.forward = 
		}
    }

	public override void SetHealth(float hp)
	{
		//hp / 5 = scale
		RectTransform barTransform = healthBar.GetComponent<RectTransform>();
		barTransform.localScale = new Vector3(hp/5, 2, 1);
	}
	//I guess it will face the closest active camera with a z less than it?
	//it will face the camera of the current room it is in
}
