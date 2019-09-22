using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrationMethods_Spring 
{

    static float damping = 2f;


    public static void CurrentIntegrationMethod(float h,
    Vector3 currentPosition,
    Vector3 currentVelocity,
    out Vector3 newPosition,
    out Vector3 newVelocity,
    float mass,
    float k,
    float methodsindex)
    {
        //MonoBehaviour.print("here");

        switch (methodsindex) {

            case 0: MidPointMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k);
                break;
            case 1:
                BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k);
                break; 
            case 2: ForthOrderRungeKuttaMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k);
                break;
            default: 
                MidPointMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k);
             break;

        }
        
        //BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity,ref acceleratingfactor, mass);
        
        //ForthOrderRungeKuttaMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass);

    }

    public static void BackwardEuler(float h,
       Vector3 currentPosition,
       Vector3 currentVelocity,
       out Vector3 newPosition,
       out Vector3 newVelocity,
       float mass,
       float k)
    {

        //out parameters must be assigned value in the called method
        //BackwardEuler
        Vector3 acceleratingFactor = CalculateForce(currentPosition, currentVelocity, k, mass)/ mass;
        newVelocity = currentVelocity + h * acceleratingFactor;
        newPosition = currentPosition + h * newVelocity;

    }

    public static void ForthOrderRungeKuttaMethod(float h,
       Vector3 currentPosition,
       Vector3 currentVelocity,
       out Vector3 newPosition,
       out Vector3 newVelocity,
       float mass,
       float k)
    {
        Vector3[] position = new Vector3[5];
        Vector3[] velocity = new Vector3[5];


        Vector3 acceleratingFactor = CalculateForce(currentPosition, currentVelocity, k, mass) / mass;

        ///need to improve
        position[0] = currentPosition;
        velocity[0] = currentVelocity;

        position[1] = position[0];
        velocity[1] = velocity[0];

        position[2] = position[0] + h / 2.0f * velocity[1];
        acceleratingFactor = CalculateForce(position[1], velocity[1], k, mass) / mass;   //accounting the velocity
        velocity[2] = velocity[0] + h / 2.0f * acceleratingFactor;

        position[3] = position[0] + h / 2.0f * velocity[2];
        acceleratingFactor = CalculateForce(position[2], velocity[2], k, mass) / mass;   //accounting the velocity
        velocity[3] = velocity[0] + h / 2.0f * acceleratingFactor;

        position[4] = position[0] + h * velocity[3];
        acceleratingFactor = CalculateForce(position[3], velocity[3], k, mass) / mass; //accounting the velocity
        velocity[4] = velocity[0] + h * acceleratingFactor;

        //combined derivates
        newPosition = position[0] + h / 6.0f * (velocity[1] + 2 * velocity[2] + 2 * velocity[3] + velocity[4]);
        newVelocity = velocity[0] + h * acceleratingFactor;   //const a

    }

    public static void MidPointMethod(float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        float mass,
        float k)
    {
        Vector3 acceleratingFactor = CalculateForce(currentPosition,currentVelocity,k, mass)/mass;

        Vector3 HalfnewVelocity = currentVelocity + acceleratingFactor * h / 2.0f;
        Vector3 HalfnewPosition = currentPosition + HalfnewVelocity * h / 2.0f;

        acceleratingFactor = CalculateForce(HalfnewPosition, HalfnewVelocity,k, mass) / mass;   //accounting the velocity

        newVelocity = currentVelocity + h * acceleratingFactor;
        newPosition = currentPosition + h * HalfnewVelocity;
    }


    public static Vector3 CalculateForce(Vector3 currentPosition, Vector3 currentVelocity,float k, float mass)
    {
        Vector3 forcevector = Vector3.zero;
        Vector3 springforce = -k * (currentPosition - SpringControl.fixedPosition);
        Vector3 dampingforce = -damping * currentVelocity;
        
        forcevector = Physics.gravity*mass +springforce+dampingforce;
        //MonoBehaviour.print("Resulting Force:" + forcevector);
        return forcevector;
    }
}
