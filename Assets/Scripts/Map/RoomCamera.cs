using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
	public Camera cam;
	public Room room;
	
	//true if the camera is in twin mode where two cameras are being used to follow both players
	public bool isTwin;
	//used for the offset position
	public bool leftTwin;
	//the position it goes to during normal state
	private Vector3 basePosition;

	public Pawn pawn;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
		basePosition = transform.position;
    }

	void Awake()
	{
		cam = GetComponent<Camera>();
		basePosition = transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        if(isTwin)
		{
			DoTwinState();
		} else
		{
			DoNormalState();
		}
    }

	public virtual void DoTwinState()
	{
		//transform.position = new Vector3 (pawn.transform.position.x - transform.parent.position.x, transform.position.y, transform.position.z);
		transform.position = new Vector3 (pawn.transform.position.x, transform.position.y, transform.position.z);
		if(leftTwin)
		{
			cam.rect = new Rect(0f,0f,0.4995f,1);
		} else
		{
			cam.rect = new Rect(0.5005f,0f,0.4995f,1);
		}
	}

	public virtual void DoNormalState()
	{

	}

	public virtual void SwapTwin(bool twin)
	{//
		if(twin)
		{
			isTwin = true;
			//transform.position.x = pawn.transform.position.x - transform.parent.position.x; 
			foreach(PlayerController player in GameManager.inst.listPlayers)
			{
				if(player.pawn.roomLocation = room)
				{
					pawn = player.pawn;
				}
			}
			//cam.aspect = 8/9f;
			//cam.rect = new Rect(0f,0f,0.5f,1);
		} else
		{
			isTwin = false;
			transform.position = basePosition;
			cam.rect = new Rect(0f,0f,1,1);
		}
	}
}
