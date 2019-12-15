using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDistance : MonoBehaviour
{
    // Start is called before the first frame update

    //Second objects to be falled
    public GameObject SecondObject;
    void Start()
    {
        SecondObject.GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
         if(Time.realtimeSinceStartup>10.0f)
            SecondObject.GetComponent<Rigidbody>().useGravity = true;

    }
}
