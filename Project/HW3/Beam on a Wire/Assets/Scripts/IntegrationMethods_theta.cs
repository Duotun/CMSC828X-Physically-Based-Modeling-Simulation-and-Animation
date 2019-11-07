using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrationMethods_theta 
{

    static float damping = 2f;
    static float dangle = 0.0f;      //1st angle

    static float deg2rad_ratio = 3.1415926535897932384626433832795f / 180.0f;
    public static void CurrentIntegrationMethod(float h,
    float radius,
    float currenttheta,
    out float newtheta,
    Vector3 currentTranspose,
    out Vector3 newTranspose,
    ref Vector3 force,
    float mass,
    float methodsindex)
    {
        //MonoBehaviour.print("here");

        switch (methodsindex) {

           /* case 0: MidPointMethod(h, radius, currenttheta, out newtheta, mass);
                break;
            case 1:
                BackwardEuler(h, radius, currenttheta, out newtheta, mass);
                break; 
            case 2: ForthOrderRungeKuttaMethod(h, radius, currenttheta, out newtheta, mass);
                break;
            */
            default:
                //MidPointMethod(h, radius, currenttheta, currenttheta, out newtheta, mass);
                BackwardEuler(h, radius, currenttheta, out newtheta, currentTranspose, out newTranspose, ref force, mass);
                break;

        }
        
        //BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity,ref acceleratingfactor, mass);
        
        //ForthOrderRungeKuttaMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingfactor, mass);

    }

    public static void BackwardEuler(float h,
    float radius,
    float currenttheta,
    out float newtheta,
    Vector3 currentTranspose,
    out Vector3 newTranspose,
    ref Vector3 force,
    float mass)
    {

        //out parameters must be assigned value in the called method
        //BackwardEuler x(n+1) = x(n) + delta_t*(vn+1)
        Vector3 T = new Vector3(-radius * Mathf.Cos(currenttheta + 90 * deg2rad_ratio), 0, radius * Mathf.Sin(currenttheta + 90 * deg2rad_ratio));
        force += T * -dangle * 0.22f;   //add some frictions here
        float theta_2st = 1.0f * (Vector3.Dot(T, force) / mass - Vector3.Dot(T, currentTranspose) * dangle) / Vector3.Dot(T, T);
        newTranspose = currentTranspose - T * dangle*h;   //update transpose_1st first (utlize chain rules)
        dangle += theta_2st * h;   //Then angle_1st
        newtheta = currenttheta+dangle * h;    //finally angle  
        // just for rotation counting, doesn't change the actual angle at all
        newtheta %= 2*Mathf.PI;  
    }

   /* public static void ForthOrderRungeKuttaMethod(float h,
    Vector3 currentPosition,
    float currenttheta,
    out float newtheta,
    float mass)
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
    float radius,
    float currenttheta,
    out float newtheta,
    float mass)
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
        Vector3 springforce = -k * (currentPosition - new Vector3(0,0,0)); //SpringControl.fixedPosition
        Vector3 dampingforce = -damping * currentVelocity;
        
        forcevector = Physics.gravity*mass +springforce+dampingforce;
        //MonoBehaviour.print("Resulting Force:" + forcevector);
        return forcevector;
    }
    */
}
