using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNoisemaker : NoiseMaker
{
    // Start is called before the first frame update
    void Start()
    {
        noise = GameManager.Noises.Hit;
    }

}
