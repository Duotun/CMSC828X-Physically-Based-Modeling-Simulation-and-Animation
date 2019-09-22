using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamRot : MonoBehaviour
{
    public float SmoothRotval = 0.3f;

    void Start()
    {
 
    }

    void Update()
    {
        float Rot = Input.GetAxis("Mouse Y") * SmoothRotval;
        float Rot2 = Input.GetAxis("Mouse X") * SmoothRotval;

        transform.Rotate(new Vector3(-Rot * Time.deltaTime, Rot2 *Time.deltaTime, 0f));
      
    }
}
