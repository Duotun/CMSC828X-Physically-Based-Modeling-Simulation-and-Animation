using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrationMethods_theta 
{

    static float damping = 2f;
    public static float dangle = 0.0f;      //1st angle

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

        switch (methodsindex) {

            case 0:
                BackwardEuler(h, radius, currenttheta, out newtheta, currentTranspose, out newTranspose, ref force, mass);
                
                break;
            case 1:
                MidPointMethod(h, radius, currenttheta, out newtheta, currentTranspose, out newTranspose, ref force, mass);
                break; 
            default:
                BackwardEuler(h, radius, currenttheta, out newtheta, currentTranspose, out newTranspose, ref force, mass);
                break;

        }

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
        Vector3 T = new Vector3(radius * Mathf.Cos(currenttheta + 90 * deg2rad_ratio), 0, radius * Mathf.Sin(currenttheta + 90 * deg2rad_ratio));
        force += T * -dangle * 0.12f;   //add some frictions here
        float theta_2st = 1.0f * (Vector3.Dot(T, force) / mass - Vector3.Dot(T, currentTranspose) * dangle) / Vector3.Dot(T, T);
        newTranspose = currentTranspose - T * dangle*h;   //update transpose_1st first (utlize chain rules)
        dangle += theta_2st * h;   //Then angle_1st
        newtheta = currenttheta+dangle * h;    //finally angle  
        // just for rotation counting, doesn't change the actual angle at all
        newtheta %= 2*Mathf.PI;  
    }

    public static void MidPointMethod(float h,
    float radius,
    float currenttheta,
    out float newtheta,
    Vector3 currentTranspose,
    out Vector3 newTranspose,
    ref Vector3 force,
    float mass)
    {
        Vector3 baseforce = force;

        Vector3 T = new Vector3(radius * Mathf.Cos(currenttheta + 90 * deg2rad_ratio), 0, radius * Mathf.Sin(currenttheta + 90 * deg2rad_ratio));
        baseforce += T * -dangle * 0.12f;   //add some frictions here

        float theta_2st = 1.0f * (Vector3.Dot(T, force) / mass - Vector3.Dot(T, currentTranspose) * dangle) / Vector3.Dot(T, T);

        Vector3 HalfTranspose = currentTranspose - T * dangle * h/2;
        float halfdangle = dangle + theta_2st * h / 2;
        float halftheta = currenttheta + halfdangle * h / 2;

        baseforce = force;
        T = new Vector3(radius * Mathf.Cos(halftheta + 90 * deg2rad_ratio), 0, radius * Mathf.Sin(halftheta + 90 * deg2rad_ratio));
        baseforce += T * -halfdangle* 0.12f;   //add some frictions here

        theta_2st = 1.0f * (Vector3.Dot(T, baseforce) / mass - Vector3.Dot(T, HalfTranspose) * halfdangle) / Vector3.Dot(T, T);

        newTranspose = currentTranspose - T * halfdangle * h;   //update transpose_1st first (utlize chain rules)
        dangle += theta_2st * h;   //Then angle_1st
        newtheta = currenttheta + dangle * h;    //finally angle  
        
        // just for rotation counting, doesn't change the actual angle at all
        newtheta %= 2 * Mathf.PI;
    }

    
}
