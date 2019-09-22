using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Turret;
    public GameObject bulletObj;
    public GameObject output;
    public Transform bulletParent;

    public float MassofPowder = 1f;
    private float KgofPowderper = 10000f;   //N
    public float MassofBall = 1000f;    //kg
    static float stepsize = 0f;  //step size

    private Vector3 winddrag = new Vector3(50f, 0f, 0f);

    public TMP_Dropdown integrationmethodsUI;
    public TMP_InputField input_powder;
    public TMP_InputField input_ball;

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
      
    }

    private void Update()
    {
        RotatingControl();
        information_submit();
    }

    void information_submit()
    {
        if (input_powder.isFocused && input_powder.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            MassofPowder = int.Parse(input_powder.text);
            print("a" + MassofPowder);
        }

        if (input_ball.isFocused && input_ball.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            MassofBall = int.Parse(input_ball.text);
        }
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
            float DeltatimeforAcceleration = 0.02f;   //self-defined acceleration for initial velocities
            //acceleratingfactor += winddrag;
            //new thread to control
            int methodsindex = integrationmethodsUI.value;
            StartCoroutine(IntegrationMethods_Prepare(newBullet, MassofBall, tmp, KgofPowderper * MassofPowder * DeltatimeforAcceleration/ MassofBall * output.transform.up, newPosition, newVelocity, acceleratingfactor, methodsindex));
        }
    }


    //Parameters Preparation and then the waiting for a while the ball will be delete
    IEnumerator IntegrationMethods_Prepare(GameObject Bullet, float mass, Vector3 currentPosition, Vector3 currentVelocity, Vector3 newPosition, Vector3 newVelocity, Vector3 acceleratingFactor, int methodsindex)
    {
        Vector3 BulletPosition = Bullet.transform.position;

        //plusing wind here is not very proper
        //currentVelocity += new Vector3(50, 0f, 0f);   
        while(true)  //still in the flying
        {
            /*   //use internel physics to resume
            if (Bullet.transform.position.y <= 0.3f)
            {
                Bullet.GetComponent<Rigidbody>().velocity = currentVelocity;
                Bullet.GetComponent<Rigidbody>().isKinematic = false;
                break;
            }
            */

            //simulation methods
            if(Bullet.transform.position.y<=0.3f && currentVelocity.y <=0f)
            {
                float theta = 0.8f;
                currentVelocity = new Vector3(newVelocity.x,-theta*newVelocity.y,newVelocity.z);
                if (Mathf.Round(currentVelocity.magnitude) == 0 && Mathf.Round(currentVelocity.x) ==0f) break;
            }

            yield return new WaitForSeconds(stepsize);
            //out meanse it is the output value and muse be initilized or updated.
            mass = MassofBall;  // need to update according to the ui input
            IntegrationMethods.CurrentIntegrationMethod(stepsize, currentPosition, currentVelocity, out newPosition, out newVelocity,ref acceleratingFactor,mass, methodsindex,true);
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
