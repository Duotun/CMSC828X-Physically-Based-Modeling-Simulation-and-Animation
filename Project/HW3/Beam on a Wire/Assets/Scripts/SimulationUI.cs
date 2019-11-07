using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationUI : MonoBehaviour
{
    // Start is called before the first frame update
    float radius_ball = 3f;
    public static float R = 3.0f;
    float theta = Mathf.PI/2;
    float position_x = 0.0f;
    float position_y = 0.0f;
    Vector3 currentposition;
    Vector3 currentvelocity;

    Vector3 currentTranspose = Vector3.zero;
    Vector3 newTranspose = Vector3.zero;
    Vector3 force;
    void Start()
    {
        R = radius_ball;
        float x = radius_ball * Mathf.Cos(theta);
        float z = radius_ball * Mathf.Sin(theta);
        this.gameObject.GetComponent<Transform>().position = new Vector3(x, 0, z);
        //currentposition = this.gameObject.GetComponent<Transform>().position;
        currentposition = new Vector3(0,0,R);
        currentvelocity = Vector3.zero;  //a random initial?
        force = new Vector3(0.3f, 0, -9.8f);
        parameter_preparation(force);
        //force = Vector3.zero;

        //print(-8 % 5);

    }

    // Update is called once per frame
    void Update()
    {
        //simplesimulation_directly();
        parameter_preparation(force);
        //implicit_methods();
    }

    void parameter_preparation(Vector3 force)
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


    void implicit_methods()
    {
        float timestep = 0.02f;
        Vector3 newPosition = Vector3.zero;
        Vector3 newVelocity = Vector3.zero;

        float mass = 10.0f;
        Vector3 externalforce =  new Vector3(0,0,-1.8f)*mass;
        IntegrationMethods.CurrentIntegrationMethod(timestep, radius_ball, currentposition, currentvelocity, out newPosition, out newVelocity, ref externalforce, mass, 0);
        currentposition = newPosition;
        currentvelocity = newVelocity;
        this.gameObject.GetComponent<Rigidbody>().MovePosition(currentposition);

    }


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


}
