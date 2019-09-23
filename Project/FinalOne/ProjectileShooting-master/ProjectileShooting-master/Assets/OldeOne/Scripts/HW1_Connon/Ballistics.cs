using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballistics : MonoBehaviour
{
    //Drags
    public Transform targetObj;
    public Transform gunObj;

    //The bullet's initial speed in m/s
    //Sniper rifle
    //public static float bulletSpeed = 850f;
    //Test
    public static float bulletSpeed = 10f;

    //The step size
    static float h;

    //For debugging
    private LineRenderer lineRenderer;

    void Awake()
    {
        //Can use a less precise h to speed up calculations
        //Or a more precise to get a more accurate result
        //But lower is not always better because of rounding errors
        h = Time.fixedDeltaTime * 1f;

        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        RotateGun();

        //DrawTrajectoryPath();
    }


    

    void RotateGun()
    {
        //Get the 2 angles
        float? highAngle = 0f;
        float? lowAngle = 0f;

        CalculateAngleToHitTarget(out highAngle, out lowAngle);

        //Artillery
        float angle = (float)highAngle;
        //Regular gun
        //float angle = (float)lowAngle;

        //If we are within range
        //if (angle != null)
        //{
            //Rotate the gun
            //The equation we use assumes that if we are rotating the gun up from the
            //pointing "forward" position, the angle increase from 0, but our gun's angles
            //decreases from 360 degress when we are rotating up
            gunObj.localEulerAngles = new Vector3(360f - angle, 0f, 0f);

            //Rotate the turret towards the target
            transform.LookAt(targetObj);
            transform.eulerAngles = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
        //}

    }

    //Which angle do we need to hit the target?
    //Returns 0, 1, or 2 angles depending on if we are within range
    void CalculateAngleToHitTarget(out float? theta1, out float? theta2)
    {
        //Initial speed
        float v = bulletSpeed;

        Vector3 targetVec = targetObj.position - gunObj.position;

        //Vertical distance
        float y = targetVec.y;

        //Reset y so we can get the horizontal distance x
        targetVec.y = 0f;

        //Horizontal distance
        float x = targetVec.magnitude;

        //Gravity
        float g = 9.81f;


        //Calculate the angles

        float vSqr = v * v;

        float underTheRoot = (vSqr * vSqr) - g * (g * x * x + 2 * y * vSqr);

        //Check if we are within range
        if (underTheRoot >= 0f)
        {
            float rightSide = Mathf.Sqrt(underTheRoot);

            float top1 = vSqr + rightSide;
            float top2 = vSqr - rightSide;

            float bottom = g * x;

            theta1 = Mathf.Atan2(top1, bottom) * Mathf.Rad2Deg;
            theta2 = Mathf.Atan2(top2, bottom) * Mathf.Rad2Deg;
        }
        else
        {
            theta1 = null;
            theta2 = null;
        }
    }
}
