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
    ref Vector3 acceleratingfactor)
    {
        //MonoBehaviour.print("here");
        //BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity,ref acceleratingfactor);
        //MidPointMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor);
        ForthOrderRungeKuttaMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor);

    }

    public static void BackwardEuler(float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        ref Vector3 acceleratingfactor)
    {
        Vector3 acceleratingFactor = acceleratingfactor;

        //out parameters must be assigned value in the called method
        //BackwardEuler
        newVelocity = currentVelocity + h * acceleratingFactor;
        newPosition = currentPosition + h * newVelocity;

    }

    // o(t^3)
    public static void MidPointMethod(float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        ref Vector3 acceleratingfactor)
    {
        Vector3 acceleratingFactor = acceleratingfactor;

        Vector3 HalfnewVelocity = currentVelocity + acceleratingfactor*h/2.0f;
        Vector3 HalfnewPosition = currentPosition + HalfnewVelocity*h/2.0f;
        newVelocity = currentVelocity + h * acceleratingFactor;
        newPosition = currentPosition + h * HalfnewVelocity;
    }

    public static void ForthOrderRungeKuttaMethod(float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity,
        ref Vector3 acceleratingfactor)
    {
        Vector3[] position = new Vector3[5];
        Vector3[] velocity = new Vector3[5];


        ///need to improve
        position[0] = currentPosition;
        velocity[0] = currentVelocity;

        position[1] = position[0];
        velocity[1] = velocity[0];

        position[2] = position[0] + h / 2.0f * velocity[1];
        velocity[2] = velocity[0] + h / 2.0f * acceleratingfactor;

        position[3] = position[0] + h / 2.0f * velocity[2];
        velocity[3] = velocity[0] + h / 2.0f * acceleratingfactor;

        position[4] = position[0] + h * velocity[3];
        velocity[4] = velocity[0] + h * acceleratingfactor;

        //combined derivates
        newPosition = position[0] + h / 6.0f * (velocity[1] + 2*velocity[2] + 2*velocity[3] + velocity[4]);
        newVelocity = velocity[0] + h * acceleratingfactor;   //const a

    }

}
