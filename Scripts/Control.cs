using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Turret;
    public GameObject bulletObj;
    public GameObject output;
    public Transform bulletParent;

    public float KgofPowderper = 1000f;   //N
    public float MassofBall = 1000f;    //kg
    static float stepsize = 0f;  //step size


    GameObject newBullet;
    private void Awake()
    {
        stepsize = Time.fixedDeltaTime * 1f;

    }

    void Start()
    {
        

    }

    private void FixedUpdate()
    {
        RotatingControl();
    }


    //Turret Rotating
    void RotatingControl()
    {
        if(Input.GetKey(KeyCode.W))
        {

            Turret.localRotation = Quaternion.Euler(Turret.localRotation.eulerAngles.x+2f, Turret.localRotation.eulerAngles.y, Turret.localRotation.eulerAngles.z);
        }

        if(Input.GetKey(KeyCode.S))
        {
            Turret.localRotation = Quaternion.Euler(Turret.localRotation.eulerAngles.x - 2f, Turret.localRotation.eulerAngles.y, Turret.localRotation.eulerAngles.z);
        }

        if(Input.GetKey(KeyCode.A))
        {
            Turret.localRotation = Quaternion.Euler(Turret.localRotation.eulerAngles.x, Turret.localRotation.eulerAngles.y, Turret.localRotation.eulerAngles.z + 2f);
        }

        if(Input.GetKey(KeyCode.D))
        {
            Turret.localRotation = Quaternion.Euler(Turret.localRotation.eulerAngles.x, Turret.localRotation.eulerAngles.y, Turret.localRotation.eulerAngles.z - 2f);
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 tmp = new Vector3(output.transform.position.x, output.transform.position.y, output.transform.position.z);

            GameObject newBullet = Instantiate(bulletObj, tmp, transform.rotation) as GameObject;

            //Parent it to get a less messy workspace
            newBullet.transform.parent = bulletParent;
            Vector3 newPosition = Vector3.zero;
            Vector3 newVelocity = Vector3.zero;

            //may probably use the startcoroutine 

            //Add velocity to the bullet with a rigidbody, Unity internal physics engine
            //newBullet.GetComponent<Rigidbody>().velocity = KgofPowderper / MassofBall * output.transform.up;
            newBullet.GetComponent<Rigidbody>().isKinematic = true;
            Vector3 acceleratingfactor = Physics.gravity;
            StartCoroutine(IntegrationMethods_Prepare(newBullet, tmp, KgofPowderper / MassofBall * output.transform.up, newPosition, newVelocity, acceleratingfactor));
        }
    }

    //Parameters Preparation and then the waiting for a while the ball will be delete
    IEnumerator IntegrationMethods_Prepare(GameObject Bullet, Vector3 currentPosition, Vector3 currentVelocity, Vector3 newPosition, Vector3 newVelocity, Vector3 acceleratingFactor)
    {
        Vector3 BulletPosition = Bullet.transform.position;     
        

        while(true)  //still in the flying
        {
            if (Bullet.transform.position.y <= 0.3f)
            {

                Bullet.GetComponent<Rigidbody>().isKinematic = false;
                break;
            }
            yield return new WaitForEndOfFrame();
            //out meanse it is the output value and muse be initilized or updated.
            IntegrationMethods.CurrentIntegrationMethod(stepsize, currentPosition, currentVelocity, out newPosition, out newVelocity,ref acceleratingFactor);
            currentPosition = newPosition;
            currentVelocity = newVelocity;
            Bullet.GetComponent<Rigidbody>().MovePosition(currentPosition);
            //Bullet.transform.position = currentPosition;
            BulletPosition = currentPosition;
            
        }

        yield return new WaitForSeconds(0.8f);
        Destroy(Bullet);

    }

    // wait for a while, and then disapper
}
