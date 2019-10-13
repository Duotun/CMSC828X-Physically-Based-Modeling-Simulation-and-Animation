using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    public GameObject bulletObj;
    public Transform bulletParent;
    //public float bulletSpeed;
    void Start()
    {      
        StartCoroutine(FireBullet());
    }

    public IEnumerator FireBullet()
    {
        while (true)
        {
            //Create a new bullet
            Vector3 tmp=new Vector3(transform.position.x, transform.position.y, transform.position.z);
            GameObject newBullet = Instantiate(bulletObj, tmp, transform.rotation) as GameObject;

            //Parent it to get a less messy workspace
            newBullet.transform.parent = bulletParent;

            //Add velocity to the bullet with a rigidbody
            newBullet.GetComponent<Rigidbody>().velocity = Ballistics.bulletSpeed * transform.forward;

            yield return new WaitForSeconds(2f);
        }
    }
}
