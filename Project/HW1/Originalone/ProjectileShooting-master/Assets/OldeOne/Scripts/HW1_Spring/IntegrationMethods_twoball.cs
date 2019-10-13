using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntegrationMethods_twoball
{
    public static void CurrentIntegrationMethod(float h,
   Vector3[] currentPosition,
   Vector3[] currentVelocity,
   out Vector3[] newPosition,
   out Vector3[] newVelocity,
   float mass,
   float k,
   float damp,
   float methodsindex)
    {
        switch (methodsindex)
        {
            case 0:
                MidPointMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k, damp);
                break;
            case 1:
                BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k,damp);
                break;
            case 2:
                ForthOrderRungeKuttaMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k,damp);
                break;
            default:
                MidPointMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k,damp);
                break;


        }


    }


    public static void BackwardEuler(float h,
      Vector3 []currentPosition,
      Vector3 []currentVelocity,
      out Vector3 []newPosition,
      out Vector3 []newVelocity,
      float mass,
      float k,
      float damp)
    {

        //out parameters must be assigned value in the called method
        //BackwardEuler
        Vector3 [] acceleratingFactor = new Vector3[2];
        acceleratingFactor[0] = CalculateForce_Firstball(currentPosition, currentVelocity, k, mass, damp) / mass;
        acceleratingFactor[1] = CalculateForce_Secondball(currentPosition, currentVelocity, k, mass, damp) / mass;

        newVelocity = new Vector3[2];
        newVelocity[0] = currentVelocity[0] + h * acceleratingFactor[0];
        newVelocity[1] = currentVelocity[1] + h * acceleratingFactor[1];
        newPosition = new Vector3[2];
        newPosition[0] = currentPosition[0] + h * newVelocity[0];
        newPosition[1] = currentPosition[1] + h * newVelocity[1];

    }

    public static void ForthOrderRungeKuttaMethod(float h,
       Vector3 []currentPosition,
       Vector3 []currentVelocity,
       out Vector3 []newPosition,
       out Vector3 []newVelocity,
       float mass,
       float k,
       float damp)
    {
        Vector3[][] position = new Vector3[5][];
        Vector3[][] velocity = new Vector3[5][];

        newPosition = new Vector3[2];
        newVelocity = new Vector3[2];

        Vector3 []acceleratingFactor = new Vector3[2];
        acceleratingFactor[0] = CalculateForce_Firstball(currentPosition, currentVelocity, k, mass,damp) / mass;
        acceleratingFactor[1] = CalculateForce_Secondball(currentPosition, currentVelocity, k, mass, damp) / mass;


        ///need to improve
        position[0] = new Vector3[2]; position[1] = new Vector3[2]; position[2] = new Vector3[2]; position[3] = new Vector3[2]; position[4] = new Vector3[2];
        velocity[0] = new Vector3[2]; velocity[1] = new Vector3[2]; velocity[2] = new Vector3[2]; velocity[3] = new Vector3[2]; velocity[4] = new Vector3[2];

        position[0][0] = currentPosition[0];
        velocity[0][0] = currentVelocity[0];

        position[0][1] = currentPosition[1];
        velocity[0][1] = currentVelocity[1];

        position[1][1] = position[0][1];
        velocity[1][1] = velocity[0][1];

        position[1][0] = position[1][0];
        velocity[1][0] = velocity[1][0];

        position[2][0] = position[0][0] + h / 2.0f * velocity[1][0];
        position[2][1] = position[0][1] + h / 2.0f * velocity[1][1];
        acceleratingFactor[0] = CalculateForce_Firstball(position[1], velocity[1], k, mass, damp) / mass;   //accounting the velocity
        acceleratingFactor[1] = CalculateForce_Secondball(position[1], velocity[1], k, mass, damp) / mass;   //accounting the velocity
        velocity[2][0] = velocity[0][0] + h / 2.0f * acceleratingFactor[0];
        velocity[2][1] = velocity[0][1] + h / 2.0f * acceleratingFactor[1];

        position[3][0] = position[0][0] + h / 2.0f * velocity[2][0];
        position[3][1] = position[0][1] + h / 2.0f * velocity[2][1];
        acceleratingFactor[0] = CalculateForce_Firstball(position[2], velocity[2], k, mass, damp) / mass;   //accounting the velocity
        acceleratingFactor[1] = CalculateForce_Secondball(position[2], velocity[2], k, mass, damp) / mass;   //accounting the velocity
        velocity[3][0] = velocity[0][0] + h / 2.0f * acceleratingFactor[0];
        velocity[3][1] = velocity[0][1] + h / 2.0f * acceleratingFactor[1];

        position[4][0] = position[0][0] + h * velocity[3][0];
        position[4][1] = position[0][1] + h * velocity[3][1];
        acceleratingFactor[0] = CalculateForce_Firstball(position[3], velocity[3], k, mass, damp) / mass;   //accounting the velocity
        acceleratingFactor[1] = CalculateForce_Secondball(position[3], velocity[3], k, mass, damp) / mass;   //accounting the velocity
        velocity[4][0] = velocity[0][0] + h * acceleratingFactor[0];
        velocity[4][1] = velocity[0][1] + h * acceleratingFactor[1];

        //combined derivates
        newPosition[0] = position[0][0] + h / 6.0f * (velocity[1][0] + 2 * velocity[2][0] + 2 * velocity[3][0] + velocity[4][0]);
        newVelocity[0] = velocity[0][0] + h * acceleratingFactor[0];   //const a
        newPosition[1] = position[0][1] + h / 6.0f * (velocity[1][1] + 2 * velocity[2][1] + 2 * velocity[3][1] + velocity[4][1]);
        newVelocity[1] = velocity[0][1] + h * acceleratingFactor[1];   //const a

    }

    public static void MidPointMethod(float h,
        Vector3 []currentPosition,
        Vector3 []currentVelocity,
        out Vector3 []newPosition,
        out Vector3 []newVelocity,
        float mass,
        float k,
        float damp)
    {
      ;
        Vector3[] acceleratingFactor = new Vector3[2];

        acceleratingFactor[0] = CalculateForce_Firstball(currentPosition, currentVelocity, k, mass, damp) / mass;
        acceleratingFactor[1] = CalculateForce_Secondball(currentPosition, currentVelocity, k, mass, damp) / mass;

        Vector3[] HalfnewVelocity = new Vector3[2];

        HalfnewVelocity[0] = currentVelocity[0] + acceleratingFactor[0] * h / 2.0f;
        HalfnewVelocity[1] =  currentVelocity[1] + acceleratingFactor[1] * h / 2.0f;
        Vector3[] HalfnewPosition = new Vector3[2];
        HalfnewPosition[0] = currentPosition[0] + HalfnewVelocity[0] * h / 2.0f;
        HalfnewPosition[1] = currentPosition[1] + HalfnewVelocity[1] * h / 2.0f;

        acceleratingFactor[0] = CalculateForce_Firstball(HalfnewPosition, HalfnewVelocity, k, mass,damp) / mass;   //accounting the velocity
        acceleratingFactor[1] = CalculateForce_Secondball(HalfnewPosition, HalfnewVelocity, k, mass, damp) / mass;

        newVelocity = new Vector3[2];
        newVelocity[0] = currentVelocity[0] + h * acceleratingFactor[0];
        newVelocity[1] = currentVelocity[1] + h * acceleratingFactor[1];
        newPosition = new Vector3[2];
        newPosition[0] = currentPosition[0] + h * HalfnewVelocity[0];
        newPosition[1] = currentPosition[1] + h * HalfnewVelocity[1];

        //MonoBehaviour.print("Resulting Force:" + newVelocity[0]);
    }


    public static Vector3 CalculateForce_Firstball(Vector3 []currentPosition, Vector3 []currentVelocity, float k, float mass, float damp)
    {
        Vector3 forcevector = Vector3.zero;
        Vector3 springforce_1 = -k * (currentPosition[0] - SpringContralInterface.fixedPosition);
        Vector3 springforce_2 = -k * (currentPosition[1] - currentPosition[0]);
        Vector3 dampingforce = -damp * currentVelocity[0];

        forcevector = Physics.gravity * mass + springforce_1 - springforce_2 + dampingforce;
        //MonoBehaviour.print("Resulting Force:" );
        return forcevector;
    }

    public static Vector3 CalculateForce_Secondball(Vector3 [] currentPosition, Vector3 [] currentVelocity, float k, float mass, float damp)
    {
        Vector3 forcevector = Vector3.zero;
        Vector3 springforce_2 = -k * (currentPosition[1] - currentPosition[0]);
        Vector3 dampingforce = -damp * currentVelocity[1];

        forcevector = Physics.gravity * mass + springforce_2  + dampingforce;
        //MonoBehaviour.print("Resulting Force:" + forcevector);
        return forcevector;
    }

}
