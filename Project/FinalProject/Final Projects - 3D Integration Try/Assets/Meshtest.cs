using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meshtest : MonoBehaviour
{
    // Start is called before the first frame update
    Mesh tmp;
    void Start()
    {
        tmp = this.GetComponent<MeshFilter>().mesh;
        print(transform.TransformPoint(tmp.vertices[0]));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
