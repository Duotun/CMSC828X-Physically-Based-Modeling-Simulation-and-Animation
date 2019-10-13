using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CannonInterface : MonoBehaviour 
{
    [SerializeField]
    Cursor targetCursor;

    [SerializeField]
    CannonController cannon;

    [SerializeField]
    Text timeOfFlightText;

    [SerializeField]
    float defaultFireSpeed = 35;

    [SerializeField]
    float defaultFireAngle = 45;

    [SerializeField]
    float defaultMassofBall = 1000f;

    [SerializeField]
    float defaultMassofPowder = 1f;

    //[SerializeField]
    //Control simulation;

    private float initialFireAngle;
    private float initialFireSpeed;
    public static float timestepsize;
    private bool useLowAngle;

    private bool useInitialAngle;

    public static float MassofBall;
    public static float MassofPowder;
    private bool airdragflagindex;
    void Awake()
    {
        useLowAngle = true;

        initialFireAngle = defaultFireAngle;
        initialFireSpeed = defaultFireSpeed;

        airdragflagindex = true;
        MassofBall = defaultMassofBall;
        MassofPowder = defaultMassofPowder;
        timestepsize = Time.fixedDeltaTime;   //0.02f
        useInitialAngle = true;
    }

    void Update()
    {
        if (useInitialAngle)
            cannon.SetTargetWithAngle(targetCursor.transform.position, initialFireAngle);
        else
            cannon.SetTargetWithSpeed(targetCursor.transform.position, initialFireSpeed, useLowAngle);

        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
           
            cannon.Fire();
        }

        if (Input.GetButtonDown("Fire2") && !EventSystem.current.IsPointerOverGameObject())
        {
            cannon.Fire_Simulation(airdragflagindex);
        }

            timeOfFlightText.text = Mathf.Clamp(cannon.lastShotTimeOfFlight - (Time.time - cannon.lastShotTime), 0, float.MaxValue).ToString("F3");
    }

    public void SetInitialFireAngle(string angle)
    {
        initialFireAngle = Convert.ToSingle(angle);
    }

    public void SetInitialFireSpeed(string speed)
    {
        initialFireSpeed = Convert.ToSingle(speed);
    }

    public void SetLowAngle(bool useLowAngle)
    {
        this.useLowAngle = useLowAngle;
    }

    public void UseInitialAngle(bool value)
    {
        useInitialAngle = value;
    }

    public void SetTimeStepSize(string timestep)
    {
        timestepsize = float.Parse(timestep);
    }

    public void SetMassofBall(string mass1)
    {
        MassofBall = float.Parse(mass1);
        
    }

    public void SetMassofPowder(string mass2)
    {
       
         MassofPowder = float.Parse(mass2);

    }

    public void setairdrag(bool airdrag)
    {
        airdragflagindex = airdrag;
    }
}
