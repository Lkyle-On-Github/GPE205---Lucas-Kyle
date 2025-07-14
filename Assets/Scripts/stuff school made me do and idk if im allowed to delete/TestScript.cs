using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update

    //I named it spd because in another project I work on thats what its called and its just gonna get really confusing if I have to swap it all the time idk I just want to sorry. 
    public float spd;
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        transform.position += Vector3.up * spd;
    }
}
