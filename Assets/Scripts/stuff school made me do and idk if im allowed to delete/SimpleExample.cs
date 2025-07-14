using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleExample : MonoBehaviour
{
    
    //variable definitions  
    public string theText = "Hello World";

    // Start is called before the first frame update
    private void Start()
    {
        // Write the value stored in our variable "theText" to the console window
        Debug.Log(theText);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
