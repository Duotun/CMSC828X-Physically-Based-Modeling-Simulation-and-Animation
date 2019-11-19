using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
   
    GameObject lineGen;
    [SerializeField]
    GameObject Beam;

    [SerializeField]
    Toggle Methods;   //implicite or not

    [SerializeField]
    Toggle IntegrationMethods_RK4;

    [SerializeField]
    Toggle IntegrationMethods_Mid;

    [SerializeField]
    Toggle IntegrationMethods_Euler;
    int Methodsflag = 0;   //0 ->parametric methods, 1 -> implicit methods

    int IntegerationMethodsIndex = 0;
    float mass = 5.0f;
    private void Awake()
    {
        lineGenerator();
    }

    void Start()
    {

        Positionset();
        if (Methods.isOn)
        {
            
            implicit_methods(force);
        }
        else
        {
            Vector3 Pos = Beam.GetComponent<Transform>().position;
            theta = Mathf.Atan2(Pos.z, Pos.x);
            parameter_methods(force);
        }

    }


    // Update is called once per frame
    void Update()
    {

        //Methods Choose
        IntegrationMethodsChoose();

        //Force Simulation
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
        //change to UI Control now
        if (Methods.isOn)
        {
            if (Methodsflag == 1)
            {
                currentposition = Beam.GetComponent<Transform>().position;
                currentvelocity = Vector3.zero;
                Methodsflag = 0;
            }
                implicit_methods(force);
            
        }
        else
        {
            if (Methodsflag == 0)
            {
                Vector3 Pos = Beam.GetComponent<Transform>().position;
                theta = Mathf.Atan2(Pos.z, Pos.x);
                currentTranspose = Vector3.zero;
                IntegrationMethods_theta.dangle = 0.0f;
                Methodsflag = 1;
            }
            parameter_methods(force);
        }
    }

    void parameter_methods(Vector3 force)
    {

        float timestep = 0.02f;
        float newtheta = 0.0f;
        //Vector3 newposition = new Vector3();
        IntegrationMethods_theta.CurrentIntegrationMethod(timestep,radius_ball,theta,out newtheta,currentTranspose, out newTranspose, ref force, mass, IntegerationMethodsIndex);

        currentTranspose = newTranspose;
        theta = newtheta;
        float x = radius_ball * Mathf.Cos(theta);
        float z = radius_ball * Mathf.Sin(theta);

        Beam.GetComponent<Transform>().position = new Vector3(x, 0, z);
        //this.gameObject.GetComponent<Transform>().position = newposition;

    }


    void implicit_methods(Vector3 extForce)
    {
        float timestep = 0.02f;
        Vector3 newPosition = Vector3.zero;
        Vector3 newVelocity = Vector3.zero;

        IntegrationMethods.CurrentIntegrationMethod(timestep,radius_ball, currentposition, currentvelocity, out newPosition, out newVelocity, ref extForce, mass, IntegerationMethodsIndex);
        currentposition = newPosition;
        currentvelocity = newVelocity;
        currentvelocity.y = 0.0f;
        //this.gameObject.GetComponent<Transform>().position = currentposition;
        Beam.GetComponent<Rigidbody>().MovePosition(currentposition);

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

        Beam.GetComponent<Transform>().position = new Vector3(x, 0, z);
        theta += 1f; //adding 1 degree

    }

    void ForceUpdate()   //by mouse click
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 pos = Beam.transform.position;   //sphere transformation
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
        IRend.SetPosition(0, Beam.transform.position);
        IRend.SetPosition(1, Beam.transform.position);
        
    }

    void lineupdate(Vector3 dirpos)
    {
        if (lineGen == null) lineGen = GameObject.Find("ForceLine");

        LineRenderer IRend = lineGen.GetComponent<LineRenderer>();

        IRend.SetPosition(0, Beam.transform.position);
        IRend.SetPosition(1, dirpos);
    }

    public void RadiusChange(string radius)
    {
        R = float.Parse(radius);
        Positionset();

    }

    public void MassChange(string Massstring)
    {
        mass = float.Parse(Massstring);
    }

    
    void IntegrationMethodsChoose()
    {
        if(IntegrationMethods_Euler.isOn)
        {
            IntegerationMethodsIndex = 0;
        }

        if(IntegrationMethods_Mid.isOn)
        {
            IntegerationMethodsIndex = 1;
        }

        if(IntegrationMethods_RK4.isOn)
        {
            IntegerationMethodsIndex = 2;
        }

    }


    void Positionset()
    {
        radius_ball = R;
        theta = Mathf.Atan2(Beam.GetComponent<Transform>().position.z, Beam.GetComponent<Transform>().position.x);
        float x = radius_ball * Mathf.Cos(theta);
        float z = radius_ball * Mathf.Sin(theta);
        Beam.GetComponent<Transform>().position = new Vector3(x, 0, z);
        //currentposition = this.gameObject.GetComponent<Transform>().position;
        currentposition = new Vector3(x, 0, z);
        currentvelocity = Vector3.zero;  //a random initial?
        force = Vector3.zero;
    }

}
