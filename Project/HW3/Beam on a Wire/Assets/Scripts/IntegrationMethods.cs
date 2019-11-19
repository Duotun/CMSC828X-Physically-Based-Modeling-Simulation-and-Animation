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
            default: BackwardEuler(h, radius, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass); break;
            case 0: BackwardEuler(h, radius, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass); break;        
            case 1: MidPointMethod(h, radius, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass); break;
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
        force += -currentVelocity * 0.22f;
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
        force += -HalfnewVelocity * 0.22f;  //add some frictions
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

        Vector3 externalforce = acceleratingfactor;
        Vector3 []acceleratingFactor = new Vector3[5];
        acceleratingFactor[0] = acceleratingfactor;
       
        
      
        position[0] = currentPosition;
        velocity[0] = currentVelocity;

        Vector3 force = externalforce + CalculateConstrainedForce(externalforce, radius, position[0], velocity[0], mass);
        force += -velocity[0] * 0.22f;  //add some frictions
        acceleratingFactor[1] = force/mass;
        velocity[1] = velocity[0]+ 0.5f  * h * force/mass;
        position[1] = position[0] + 0.5f * h * velocity[1];

        force = externalforce + CalculateConstrainedForce(externalforce, radius, position[1], velocity[1], mass);
        force += -velocity[1] * 0.22f;  //add some frictions
        acceleratingFactor[2] = force/mass;
        velocity[2] = velocity[0] + 0.5f * h * force / mass;
        position[2] = position[0] + 0.5f * h * velocity[2];

        force = externalforce + CalculateConstrainedForce(externalforce, radius, position[2], velocity[2], mass);
        force += -velocity[2] * 0.22f;  //add some frictions
        acceleratingFactor[3] = force/mass;
        velocity[3] = velocity[0] + 0.5f * h * force / mass;
        position[3] = position[0] + 0.5f * h * velocity[3];

        force = externalforce + CalculateConstrainedForce(externalforce, radius, position[3], velocity[3], mass);
        force += -velocity[3] * 0.22f;  //add some frictions
        acceleratingFactor[4] = force/mass;
        velocity[4] = velocity[0] + h * force / mass;
        position[4] = position[0] + h * velocity[4];

        newPosition = position[0] + (velocity[1] + 2 * velocity[2] + 2 * velocity[3] + velocity[4]) / 6 * h;
        newVelocity = velocity[0] + (acceleratingFactor[1] + 2 * acceleratingFactor[2] + 2 * acceleratingFactor[3] + acceleratingFactor[4]) / 6 * h;
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
