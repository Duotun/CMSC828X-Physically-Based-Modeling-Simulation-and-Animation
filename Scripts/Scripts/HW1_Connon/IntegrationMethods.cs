using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrationMethods
{
    // Start is called before the first frame update

   
    public static void CurrentIntegrationMethod(float h,
    Vector3 currentPosition,
    Vector3 currentVelocity,
    out Vector3 newPosition,
    out Vector3 newVelocity,
    ref Vector3 acceleratingfactor,
    float mass)
    {
        
        //BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity,ref acceleratingfactor, mass);
        MidPointMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass);
        //ForthOrderRungeKuttaMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass);

    }

    public static void BackwardEuler(float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        ref Vector3 acceleratingfactor,
        float mass)
    {
        Vector3 acceleratingFactor = acceleratingfactor;

        //out parameters must be assigned value in the called method
        //BackwardEuler
        acceleratingFactor += CalculateDrag(currentVelocity, mass);
        newVelocity = currentVelocity + h * acceleratingFactor;
        newPosition = currentPosition + h * newVelocity;

    }

    // o(t^3)
    public static void MidPointMethod(float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        ref Vector3 acceleratingfactor,
        float mass)
    {
        Vector3 acceleratingFactor = acceleratingfactor+CalculateDrag(currentVelocity,mass);

        Vector3 HalfnewVelocity = currentVelocity + acceleratingFactor*h/2.0f;
        Vector3 HalfnewPosition = currentPosition + HalfnewVelocity*h/2.0f;

        acceleratingFactor += CalculateDrag(HalfnewVelocity, mass);   //accounting the velocity

        newVelocity = currentVelocity + h * acceleratingFactor;
        newPosition = currentPosition + h * HalfnewVelocity;
    }

    public static void ForthOrderRungeKuttaMethod(float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        ref Vector3 acceleratingfactor,
        float mass)
    {
        Vector3[] position = new Vector3[5];
        Vector3[] velocity = new Vector3[5];


        Vector3 acceleratingFactor = acceleratingfactor + CalculateDrag(currentVelocity, mass);

        ///need to improve
        position[0] = currentPosition;
        velocity[0] = currentVelocity;

        position[1] = position[0];
        velocity[1] = velocity[0];

        position[2] = position[0] + h / 2.0f * velocity[1];
        acceleratingFactor += CalculateDrag(velocity[1], mass);   //accounting the velocity
        velocity[2] = velocity[0] + h / 2.0f * acceleratingFactor;

        position[3] = position[0] + h / 2.0f * velocity[2];
        acceleratingFactor += CalculateDrag(velocity[2], mass);   //accounting the velocity
        velocity[3] = velocity[0] + h / 2.0f * acceleratingFactor;

        position[4] = position[0] + h * velocity[3];
        acceleratingFactor += CalculateDrag(velocity[3], mass);   //accounting the velocity
        velocity[4] = velocity[0] + h * acceleratingFactor;

        //combined derivates
        newPosition = position[0] + h / 6.0f * (velocity[1] + 2*velocity[2] + 2*velocity[3] + velocity[4]);
        newVelocity = velocity[0] + h * acceleratingFactor;   //const a

    }

    public static Vector3 CalculateDrag(Vector3 velocityVec, float mass)
    {
        //F_drag = 0.5 * rho *c_d * A * v^2,

        float rho = 1.225f; // kg/m^3
        float m = mass;  
        float A = Mathf.PI * 0.1f * 0.1f;   // 0.1 - r
        float c_d = 0.5f;


        // add 50 m/s vec
        Vector3 airdragvec = new Vector3(30f, 0, 40f);
        float vsqr = (velocityVec+airdragvec).sqrMagnitude;

        //acceleration
        float airdrag = 0.5f * vsqr * rho * c_d * A / m;

        Vector3 dragvec = airdrag * velocityVec.normalized * -1f;


        //simplified one
        //dragvec = -50f * velocityVec.normalized / m;
        return dragvec;

    }

}
