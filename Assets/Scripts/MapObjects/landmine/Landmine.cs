using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
	public GameObject safeIndicator;
	public GameObject dangerIndicator;
	//public shooter placer;
	public float lastSwapTime;
	public float swapTime;
	public float armTime;
	public float spawnTime;
	public float dealtDamage;
	private bool safe;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
		safe = true;
    }
	void Awake()
	{
		spawnTime = Time.time;
		safe = true;
	}
    // Update is called once per frame
    void Update()
    {
        if(Time.time > swapTime + lastSwapTime && safe == true)
		{
			SwapIndicator();
			lastSwapTime = Time.time;
		}
		if(spawnTime + armTime < Time.time)
		{
			safe = false;
		}
    }

	public void SetIndicatorSafe(bool safe)
	{
		if(safe)
		{
			safeIndicator.SetActive(true);
			dangerIndicator.SetActive(false);
		} else
		{
			safeIndicator.SetActive(false);
			dangerIndicator.SetActive(true);
		}
	}
	public void SwapIndicator()
	{
		safeIndicator.SetActive(!safeIndicator.activeSelf);
		dangerIndicator.SetActive(!dangerIndicator.activeSelf);
	}
}
