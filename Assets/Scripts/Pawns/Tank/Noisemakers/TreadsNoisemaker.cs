using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadsNoisemaker : NoiseMaker
{
    // Start is called before the first frame update
    public override void Start()
    {
		base.Start();
        noise = GameManager.Noises.Movement;
		active = false;
    }


}
