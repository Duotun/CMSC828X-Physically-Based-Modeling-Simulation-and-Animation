using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public int collisionflag = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name =="Terrain")
        {
            collisionflag = 1;
        }

        if (collision.gameObject.tag == "ball")
        {
            collisionflag = 2;
        }
    }
}
