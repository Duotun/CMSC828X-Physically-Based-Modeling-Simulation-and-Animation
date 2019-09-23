using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using System;
public class CannonController : MonoBehaviour 
{
    [SerializeField]
    Transform cannonBase;

    [SerializeField]
    Transform turret;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    Transform smokePuffPoint;

    [SerializeField]
    Animation anim;

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    GameObject cannonFirePrefab;

    [SerializeField]
    GameObject cannonFirePrefabforSimulation;

    [SerializeField]
    TMP_Dropdown integrationmethodsindex;

    [SerializeField]
    ProjectileArc projectileArc;

    [SerializeField]
    float cooldown = 1;

    //[SerializeField]
    //Transform Terraintransform;

    [SerializeField]
    Toggle Trackfollow;

    private float currentSpeed;
    private float currentAngle;
    private float currentTimeOfFlight;
    private float ForcePowderper = 10000f;   //N
    public float lastShotTime { get; private set; }
    public float lastShotTimeOfFlight { get; private set; }

    public void SetTargetWithAngle(Vector3 point, float angle)
    {
        currentAngle = angle;

        Vector3 direction = point - firePoint.position;
        float yOffset = -direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;

        currentSpeed = ProjectileMath.LaunchSpeed(distance, yOffset, Physics.gravity.magnitude, angle * Mathf.Deg2Rad);

        projectileArc.UpdateArc(currentSpeed, distance, Physics.gravity.magnitude, currentAngle * Mathf.Deg2Rad, direction, true);

        SetTurret(direction, currentAngle);

        currentTimeOfFlight = ProjectileMath.TimeOfFlight(currentSpeed, currentAngle * Mathf.Deg2Rad, yOffset, Physics.gravity.magnitude);
    }

    public void SetTargetWithSpeed(Vector3 point, float speed, bool useLowAngle)
    {
        currentSpeed = speed;

        Vector3 direction = point - firePoint.position;
        float yOffset = direction.y;
        direction = Math3d.ProjectVectorOnPlane(Vector3.up, direction);
        float distance = direction.magnitude;     

        float angle0, angle1;
        bool targetInRange = ProjectileMath.LaunchAngle(speed, distance, yOffset, Physics.gravity.magnitude, out angle0, out angle1);

        if (targetInRange)
            currentAngle = useLowAngle ? angle1 : angle0;                     

        projectileArc.UpdateArc(speed, distance, Physics.gravity.magnitude, currentAngle, direction, targetInRange);
        SetTurret(direction, currentAngle * Mathf.Rad2Deg);

        currentTimeOfFlight = ProjectileMath.TimeOfFlight(currentSpeed, currentAngle, -yOffset, Physics.gravity.magnitude);
    }

    public void Fire()
    {
        if (Time.time > lastShotTime + cooldown)
        {
            GameObject p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            p.GetComponent<Rigidbody>().velocity = turret.up * currentSpeed;  //rigibody
          
            //SMOKE
            Instantiate(cannonFirePrefab, smokePuffPoint.position, Quaternion.LookRotation(turret.up));

           // print(p.transform.position);
            lastShotTime = Time.time;
            lastShotTimeOfFlight = currentTimeOfFlight;

            anim.Rewind();
            anim.Play();
        }
    }


    public void Fire_Simulation(bool airdrag)
    {
       
        GameObject newBullet = Instantiate(cannonFirePrefabforSimulation, firePoint.position, Quaternion.identity) as GameObject;

        //Parent it to get a less messy workspace
        Vector3 newPosition = Vector3.zero;
        Vector3 newVelocity = Vector3.zero;

        //Instantiate(cannonFirePrefab, smokePuffPoint.position, Quaternion.LookRotation(turret.up));
        //may probably use the startcoroutine 

        //Add velocity to the bullet with a rigidbody, Unity internal physics engine
        //newBullet.GetComponent<Rigidbody>().velocity = KgofPowderper / MassofBall * output.transform.up;
        newBullet.GetComponent<Rigidbody>().useGravity = false;
        newBullet.GetComponent<Rigidbody>().isKinematic = false;
        Vector3 acceleratingfactor = Physics.gravity;
        float DeltatimeforAcceleration = 2e-3f;   //self-defined acceleration for initial velocities
                                                  //acceleratingfactor += winddrag;
                                                  //new thread to control
        int methodsindex = integrationmethodsindex.value;
        //turret.up * currentSpeed
        //print(DeltatimeforAcceleration * CannonInterface.MassofPowder * ForcePowderper*turret.up);
        //print(ForcePowderper * CannonInterface.MassofPowder * DeltatimeforAcceleration / CannonInterface.MassofBall*turret.up);
        //print(currentSpeed);

        
        if(Trackfollow.isOn)   //following the track or not
            StartCoroutine(IntegrationMethods_Prepare(newBullet, CannonInterface.MassofBall, firePoint.position, turret.up * currentSpeed, newPosition, newVelocity, acceleratingfactor, methodsindex,airdrag));
        else
            StartCoroutine(IntegrationMethods_Prepare(newBullet, CannonInterface.MassofBall, firePoint.position, ForcePowderper * CannonInterface.MassofPowder * DeltatimeforAcceleration / CannonInterface.MassofBall * turret.up, newPosition, newVelocity, acceleratingfactor, methodsindex, airdrag));
        //anim.Rewind();
        //anim.Play();

    }



    private IEnumerator IntegrationMethods_Prepare(GameObject Bullet, float mass, Vector3 currentPosition, Vector3 currentVelocity, Vector3 newPosition, Vector3 newVelocity, Vector3 acceleratingFactor, int methodsindex, bool airdrag)
    {
        Vector3 BulletPosition = Bullet.transform.position;

        //plusing wind here is not very proper
        //currentVelocity += new Vector3(50, 0f, 0f);   
        while (true)  //still in the flying
        {
            /*   //use internel physics to resume
            if (Bullet.transform.position.y <= 0.3f)
            {
                Bullet.GetComponent<Rigidbody>().velocity = currentVelocity;
                Bullet.GetComponent<Rigidbody>().isKinematic = false;
                break;
            }
            */
            float stepsize = CannonInterface.timestepsize;
            //simulation methods
           if (Bullet.GetComponent<BallCollision>().collisionflag!=0)
            {
               
                float theta = 0.8f;

                if (Bullet.GetComponent<BallCollision>().collisionflag == 1)
                    currentVelocity = new Vector3(newVelocity.x, -theta * newVelocity.y, newVelocity.z);
                else if (Bullet.GetComponent<BallCollision>().collisionflag == 2)
                    currentVelocity = Bullet.GetComponent<Rigidbody>().velocity;   //if collision is from the ball

                Bullet.GetComponent<BallCollision>().collisionflag = 0;
                if (Mathf.Round(currentVelocity.x) == 0f && Mathf.Round(currentVelocity.z) == 0f) break;

                /*sr.WriteLine(datosCSV);
                datosCSV = "";

                FileInfo fInfo = new FileInfo(ruta);
                fInfo.IsReadOnly = true;
                sr.Close();
                */

            }
            //print("s " + currentVelocity);
            if(currentPosition.y < -3f) break;  //some bad collision compensation



            yield return new WaitForFixedUpdate();

            //datosCSV += currentPosition.x+","+ currentPosition.y +","+ currentPosition.z+System.Environment.NewLine;
            //out meanse it is the output value and muse be initilized or updated.
            //mass = CannonInterface.MassofBall;  // need to update according to the ui input
            var watch = System.Diagnostics.Stopwatch.StartNew();
            IntegrationMethods.CurrentIntegrationMethod(stepsize, currentPosition, currentVelocity, out newPosition, out newVelocity, ref acceleratingFactor, mass, methodsindex, airdrag);
            watch.Stop();
            //print("One-step: " + watch.Elapsed.Ticks);
            currentPosition = newPosition;
            currentVelocity = newVelocity;
            acceleratingFactor = Physics.gravity;
            //print("Curren Position: " + currentVelocity);
            //Bullet.transform.position = currentPosition;
            Bullet.GetComponent<Rigidbody>().MovePosition(currentPosition);
            //Bullet.transform.position = currentPosition;
            BulletPosition = currentPosition;

           
        }

        yield return new WaitForSeconds(0.3f);
        Destroy(Bullet);

    }

    private void SetTurret(Vector3 planarDirection, float turretAngle)
    {
        cannonBase.rotation =  Quaternion.LookRotation(planarDirection) * Quaternion.Euler(-90, -90, 0);
        turret.localRotation = Quaternion.Euler(90, 90, 0) * Quaternion.AngleAxis(turretAngle, Vector3.forward);        
    }
}
