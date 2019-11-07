using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationPrep: MonoBehaviour
{
    // Start is called before the first frame update
    float radius_ball = 3f;
    public static float R = 3.0f;
    float theta = Mathf.PI/4;
    float position_x = 0.0f;
    float position_y = 0.0f;
    Vector3 currentposition;
    Vector3 currentvelocity;

    Vector3 currentTranspose = Vector3.zero;
    Vector3 newTranspose = Vector3.zero;
    Vector3 force=Vector3.zero;

    bool clickflag = false;
    [SerializeField]
    GameObject lineGen;

    private void Awake()
    {
        lineGenerator();
    }

    void Start()
    {
        R = radius_ball;
        float x = radius_ball * Mathf.Cos(theta);
        float z = radius_ball * Mathf.Sin(theta);
        this.gameObject.GetComponent<Transform>().position = new Vector3(x, 0, z);
        //currentposition = this.gameObject.GetComponent<Transform>().position;
        currentposition = new Vector3(x, 0, z);
        currentvelocity = Vector3.zero;  //a random initial?
        force = Vector3.zero;
        //force = new Vector3(0.0f, 0, -9.8f);
        implicit_methods(force);
        //parameter_methods(force);
        //force.z=-3.3f;
        //force.x = 0.3f;


    }


    // Update is called once per frame
    void Update()
    {

        if(!clickflag)
        {
            if(Input.GetMouseButtonDown(1))
            {
                clickflag = true;
                ForceUpdate();
                
            }

        }
        else
        {
            ForceUpdate();          
            if(Input.GetMouseButtonDown(1))  //clik again, stop add force
            {
                clickflag=false;
                ForceUpdate();  //to zero force

            }
        }
        //simplesimulation_directly();
        //parameter_methods(force);
        implicit_methods(force);
    }

    void parameter_methods(Vector3 force)
    {
       
        float timestep = 0.02f;
        float newtheta = 0.0f;
        //Vector3 newposition = new Vector3();
        IntegrationMethods_theta.CurrentIntegrationMethod(timestep,radius_ball,theta,out newtheta,currentTranspose, out newTranspose, ref force, 1.0f,0);

        currentTranspose = newTranspose;
        theta = newtheta;
        float x = radius_ball * Mathf.Cos(theta);
        float z = radius_ball * Mathf.Sin(theta);

        //float x = radius_ball * Mathf.Cos(theta);
        //float z = radius_ball * Mathf.Sin(theta);

        this.gameObject.GetComponent<Transform>().position = new Vector3(x, 0, z);
        //this.gameObject.GetComponent<Transform>().position = newposition;

    }


    void implicit_methods(Vector3 extForce)
    {
        float timestep = 0.02f;
        Vector3 newPosition = Vector3.zero;
        Vector3 newVelocity = Vector3.zero;

        float mass = 10.0f;
        //Vector3 externalforce =  new Vector3(0.3f,0,-9.8f)*mass;
        IntegrationMethods.CurrentIntegrationMethod(timestep,radius_ball, currentposition, currentvelocity, out newPosition, out newVelocity, ref extForce, mass, 0);
        currentposition = newPosition;
        currentvelocity = newVelocity;
        //this.gameObject.GetComponent<Transform>().position = currentposition;
        this.gameObject.GetComponent<Rigidbody>().MovePosition(currentposition);

    }




    //no constraints methods
    void simplesimulation_directly()
    {
        // x = r[cos\theta, sin\thets]
        if (R != radius_ball)
        {
            R = radius_ball;   //keep updates
            print(R);
        }
     
        float x = radius_ball * Mathf.Cos(theta / 360 * 2 * Mathf.PI);
        float z = radius_ball * Mathf.Sin(theta / 360 * 2 * Mathf.PI);

        this.gameObject.GetComponent<Transform>().position = new Vector3(x, 0, z);
        theta += 1f; //adding 1 degree

    }

    void ForceUpdate()   //by mouse click
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 pos = this.transform.position;   //sphere transformation
            Vector3 dir = hit.point;

            if (clickflag)
            {
                lineupdate(dir);
                force = dir - pos;
            }
            else
            {
                lineupdate(pos);
                force = Vector3.zero;
            }
        }
    }

 
    void lineGenerator()
    {
        if (lineGen == null) lineGen = GameObject.Find("ForceLine");
        LineRenderer IRend = lineGen.GetComponent<LineRenderer>();
        IRend.positionCount = 2;
        IRend.SetPosition(0, this.transform.position);
        IRend.SetPosition(1, this.transform.position);
        
    }

    void lineupdate(Vector3 dirpos)
    {
        if (lineGen == null) lineGen = GameObject.Find("ForceLine");

        LineRenderer IRend = lineGen.GetComponent<LineRenderer>();

        IRend.SetPosition(0, this.transform.position);
        IRend.SetPosition(1, dirpos);
    }

}
