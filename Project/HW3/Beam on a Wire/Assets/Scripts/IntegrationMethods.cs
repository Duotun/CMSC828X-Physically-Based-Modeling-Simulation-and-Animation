using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrationMethods
{
    // Start is called before the first frame update

    static float ks = 1;
    static float kd = 1;

    public static void CurrentIntegrationMethod(float h,
    float radius,
    Vector3 currentPosition,
    Vector3 currentVelocity,
    out Vector3 newPosition,
    out Vector3 newVelocity,
    ref Vector3 acceleratingfactor,    // rightnow transfer the gravity into it.
    float mass,
    int methodsindex)
    {
        
        switch(methodsindex)
        {
            default: MidPointMethod(h, radius, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass); break;
            case 0: MidPointMethod(h, radius, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass); break;
            case 1: BackwardEuler(h, radius, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass); break;
            case 2: ForthOrderRungeKuttaMethod(h, radius, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass);break;

        }
       
    }




    public static void BackwardEuler(float h,
        float radius,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        ref Vector3 acceleratingfactor,
        float mass)
    {
        Vector3 externalforce = acceleratingfactor;

        //out parameters must be assigned value in the called method
        //BackwardEuler
        Vector3 force = externalforce + CalculateConstrainedForce(externalforce, radius, currentPosition, currentVelocity, mass);
        Vector3 newacceleration = force / mass;
        newVelocity = currentVelocity + h * newacceleration;
        newPosition = currentPosition + h * newVelocity;

    }

    // o(t^3)
    public static void MidPointMethod(float h,
          float radius,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        ref Vector3 acceleratingfactor,
        float mass)
    {
        Vector3 externalforce = acceleratingfactor;

        //out parameters must be assigned value in the called method
        //BackwardEuler
        Vector3 force = externalforce + CalculateConstrainedForce(externalforce, radius, currentPosition, currentVelocity, mass);
        force += -currentVelocity * 0.22f;  //add some frictions
        Vector3 HalfnewVelocity = currentVelocity + force/mass*h/2.0f;
        Vector3 HalfnewPosition = currentPosition + HalfnewVelocity*h/2.0f;

        force = externalforce + CalculateConstrainedForce(externalforce, radius, HalfnewPosition, HalfnewVelocity, mass);
        force += -currentVelocity * 0.22f;  //add some frictions
        newVelocity = currentVelocity + h * force / mass;
        newPosition = currentPosition + h * HalfnewVelocity;

        //MonoBehaviour.print(acceleratingFactor);
    }

    public static void ForthOrderRungeKuttaMethod(float h,
          float radius,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        ref Vector3 acceleratingfactor,
        float mass)
    {
        Vector3[] position = new Vector3[5];
        Vector3[] velocity = new Vector3[5];


        Vector3 []acceleratingFactor = new Vector3[5];
        acceleratingFactor[0] = acceleratingfactor;
       
        
        

        ///need to improve
        position[0] = currentPosition;
        velocity[0] = currentVelocity;
       // if (airdrag)
            acceleratingFactor[0] = acceleratingfactor + CalculateDrag(currentVelocity, mass);
        position[1] = position[0] + h * velocity[0];

        velocity[1] = velocity[0] + h* acceleratingFactor[0];

        position[2] = position[0] + h / 2.0f * velocity[1];
        //if(airdrag)
            acceleratingFactor[1] = acceleratingfactor + CalculateDrag(velocity[1], mass);   //accounting the velocity
        velocity[2] = velocity[0] + h / 2.0f * acceleratingFactor[1];

        position[3] = position[0] + h / 2.0f * velocity[2];
        //if (airdrag)
            acceleratingFactor[2] = acceleratingfactor+ CalculateDrag(velocity[2], mass);   //accounting the velocity
        velocity[3] = velocity[0] + h / 2.0f * acceleratingFactor[2];

        position[4] = position[0] + h * velocity[3];
        //if (airdrag)
            acceleratingFactor[3] = acceleratingfactor+ CalculateDrag(velocity[3], mass);   //accounting the velocity
        velocity[4] = velocity[0] + h * acceleratingFactor[3];

        //combined derivates
        newPosition = position[0] + h / 6.0f * (velocity[1] + 2*velocity[2] + 2*velocity[3] + velocity[4]);
        newVelocity = velocity[0] + h / 6.0f * (acceleratingFactor[0] + 2 * acceleratingFactor[1] + 2 * acceleratingFactor[2] + acceleratingFactor[3]);    //const a

    }

    public static Vector3 CalculateDrag(Vector3 velocityVec, float mass)
    {
        //F_drag = 0.5 * rho *c_d * A * v^2,

        float rho = 1.225f; // kg/m^3
        float m = mass;  
        float A = Mathf.PI * 0.05f * 0.05f;   // 0.1 - r
        float c_d = 0.5f;


        // add 50 m/s vec, 
        Vector3 airdragvec = new Vector3(3f, 0, 4f);
        float vsqr = (velocityVec - airdragvec.normalized*4.47f).sqrMagnitude;

        //acceleration
        float airdrag = 0.5f * vsqr * rho * c_d * A/m*10f;   //effect is hard to see here because mass is too big, *300f to magnify this effect

        Vector3 dragvec = airdrag * velocityVec.normalized * -1f;

        //MonoBehaviour.print(dragvec);
        //simplified one
        //dragvec = -50f * velocityVec.normalized / m;
        return dragvec;

    }

    public static Vector3 CalculateConstrainedForce(Vector3 force,float radius, Vector3 position, Vector3 velocity,float mass)
    {
        Vector3 constrainedforce = new Vector3();
        float fx = Vector3.Dot(force, position);
        float vv = Vector3.Dot(velocity, velocity);
        float xx = Vector3.Dot(position, position);

        float kdforce_feedback = kd *(xx - radius*radius) / 2;
        float ksforce_feedback = ks * Vector3.Dot(position, velocity);
        float lambda = -1 * (fx + mass * vv + kdforce_feedback*mass + ksforce_feedback*mass)/xx;
        constrainedforce = lambda * position;

        return constrainedforce;
    }

}
