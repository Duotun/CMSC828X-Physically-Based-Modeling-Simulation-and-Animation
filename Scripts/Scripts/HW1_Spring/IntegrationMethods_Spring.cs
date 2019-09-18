using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrationMethods_Spring 
{

    static float damping = 1f;


    public static void CurrentIntegrationMethod(float h,
    Vector3 currentPosition,
    Vector3 currentVelocity,
    out Vector3 newPosition,
    out Vector3 newVelocity,
    float mass,
    float k)
    {
        //MonoBehaviour.print("here");


        //BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity,ref acceleratingfactor, mass);
        MidPointMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, mass,k);
        //ForthOrderRungeKuttaMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass);

    }

    public static void MidPointMethod(float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        float mass,
        float k)
    {
        Vector3 acceleratingFactor = CalculateForce(currentPosition,currentVelocity,k)/mass;

        Vector3 HalfnewVelocity = currentVelocity + acceleratingFactor * h / 2.0f;
        Vector3 HalfnewPosition = currentPosition + HalfnewVelocity * h / 2.0f;

        acceleratingFactor = CalculateForce(currentPosition, currentVelocity,k) / mass;   //accounting the velocity

        newVelocity = currentVelocity + h * acceleratingFactor;
        newPosition = currentPosition + h * HalfnewVelocity;
    }


    public static Vector3 CalculateForce(Vector3 currentPosition, Vector3 currentVelocity,float k)
    {
        Vector3 forcevector = Vector3.zero;
        Vector3 springforce = -k * (currentPosition - SpringControl.anchorposition);
        Vector3 dampingforce = -damping * currentVelocity;
        forcevector = Physics.gravity +springforce+dampingforce;

        return forcevector;
    }
}
