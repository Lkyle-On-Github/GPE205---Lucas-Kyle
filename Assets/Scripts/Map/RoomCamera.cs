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
	float twinX;
	public float twinClamp;
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
		if(pawn != null)
		{
			//transform.position = new Vector3 (pawn.transform.position.x - transform.parent.position.x, transform.position.y, transform.position.z);
			twinX = Mathf.Clamp(pawn.transform.position.x, basePosition.x - twinClamp, basePosition.x + twinClamp);
			transform.position = new Vector3 (twinX, transform.position.y, transform.position.z);
			if(leftTwin)
			{
				cam.rect = new Rect(0f,0f,0.4995f,1);
			} else
			{
				cam.rect = new Rect(0.5005f,0f,0.4995f,1);
			}
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
			//cam.aspect = 8/9f;
			//cam.rect = new Rect(0f,0f,0.5f,1);
		} else
		{
			isTwin = false;
			transform.position = basePosition;
			cam.rect = new Rect(0f,0f,1,1);
		}
	}

	public void OnDestroy()
	{
		if(GameManager.inst.listActiveCams != null)
			{
				if(GameManager.inst.listActiveCams.Contains(this.gameObject))
				{
					GameManager.inst.listActiveCams.Remove(this.gameObject);
				}
			}
	}
}
