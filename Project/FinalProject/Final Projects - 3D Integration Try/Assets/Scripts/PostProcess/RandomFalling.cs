using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFalling : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] objects;
    public float delay;
    void Start()
    {
        InvokeRepeating("Spawn", delay, delay);
    }

    // Update is called once per frame
    void Update()
    {

        //Cancel all Invoke Calls   --20.0 for large objects, 10 for cubes
        if (Time.realtimeSinceStartup>10.0f)
            CancelInvoke();
    }

    void Spawn()
    {
       
        for(int i=0;i<objects.Length;i++)
        {
            Quaternion myrotation = Quaternion.identity;
            myrotation.eulerAngles = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
            Instantiate(objects[i], new Vector3(Random.Range(-6, 6), Random.Range(20,40), 0), myrotation);
        }
    }
}
