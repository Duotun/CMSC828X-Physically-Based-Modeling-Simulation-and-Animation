using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Diagnostics;
public class SpringContralInterface : MonoBehaviour
{
    // Start is called before the first frame update


    public static Vector3 fixedPosition; // fixed point (white point)

    [SerializeField]
    GameObject lineGen;

    [SerializeField]
    float defaultTime = 35;

    [SerializeField]
    float defaultHook = 45;

    [SerializeField]
    float defaultMassofBall = 1000f;

    [SerializeField]
    float defaultDamping= 1f;

    [SerializeField]
    TMP_Dropdown integrationmethodsindex;

    float timestepsize;
    float massofball;
    float dampingcoeff;
    float hookcoeff;

    bool simulationflag;  //for mouse input
    int simulationstart;  //control only one simulation

    private Vector3 mOffset;
    private float mZCoord;

    [SerializeField]
    GameObject firstball;

    [SerializeField]
    GameObject secondball;

    [SerializeField]
    GameObject fixedGameobject;

    Vector3[] ballposition;
    Vector3[] ballvelocity;

    int timeaccounting;  //for efficiency
    float lastrecordtime;
    static double timeconsumed;
    private void Awake()
    {
        timestepsize = defaultTime;
        massofball = defaultMassofBall;
        dampingcoeff = defaultDamping;
        hookcoeff = defaultHook;
        ballposition = new Vector3[2];
        ballvelocity = new Vector3[2];
        ballposition[0] = firstball.transform.position;
        ballposition[1] = secondball.transform.position;
        fixedPosition = fixedGameobject.transform.position;
        simulationflag = true;
        simulationstart = 0;
        lineGenerator();


        //data process
        timeaccounting = 0;
        lastrecordtime = 0f;
        timeconsumed = 0.0;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButton("Fire1") || Input.GetButtonDown("Fire1")) && !EventSystem.current.IsPointerOverGameObject())  //ui
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform.gameObject.tag == "ball")
                {
                    simulationflag = false;  //stop when click on balls
                   
                }
                
            }
        }
        else
        {
            simulationflag = true;
        }

        lineupdate();
        if(simulationflag)
        {
            if(simulationstart == 0)
            {
                
                Integration_Prepare();
                simulationstart = 1;

            }
        }
        
    }

    void Integration_Prepare()
    {

        firstball.GetComponent<Rigidbody>().isKinematic = true;
        secondball.GetComponent<Rigidbody>().isKinematic = true;

        //Vector3 springforce = -k * (transform.position - anchorposition);
        //print("Springforce: " + springforce);
        //Vector3 forceinitial = Physics.gravity+springforce;
        int methods_chosen = integrationmethodsindex.value;
        StartCoroutine(Integration_begin(methods_chosen));
    }


    IEnumerator Integration_begin(int methods_chosen)
    {
        float currentmass = massofball;
        float currentK = hookcoeff;
        float currentdamp = dampingcoeff;
        float stepsize = timestepsize;


        Vector3 [] newPosition = new Vector3[2];
        Vector3 [] newVelocity = new Vector3[2];


        while (true)
        {
            if (simulationflag == false) break;
            int methodsindex = integrationmethodsindex.value;   //methods

            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //yield return new WaitForSeconds(stepsize);
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            IntegrationMethods_twoball.CurrentIntegrationMethod(stepsize, ballposition, ballvelocity, out newPosition, out newVelocity, currentmass, currentK, currentdamp, methods_chosen);
            watch.Stop();
            //print("One-step: " + watch.Elapsed.TotalMilliseconds);
            timeconsumed += watch.Elapsed.TotalMilliseconds;
            ballposition[0] = newPosition[0]; ballposition[1] = newPosition[1];
            ballvelocity[0] = newVelocity[0]; ballvelocity[1] = newVelocity[1];
            
            firstball.GetComponent<Rigidbody>().MovePosition(ballposition[0]);
            secondball.GetComponent<Rigidbody>().MovePosition(ballposition[1]);

            //coefficients updates
            currentmass = massofball;
            currentK = hookcoeff;
            currentdamp = dampingcoeff;
            stepsize = timestepsize;

           

            lineupdate();
            timeaccounting += 1;
            if(timeaccounting >=500)
            {
                time_counting();
                timeaccounting = 0;
            }
            yield return null;

        }

        simulationstart = 0;
       // yield return new WaitForSeconds(0.2f);
    }



    //for efficiency
    void time_counting()
    {
        //float timeconsumed = Time.time - lastrecordtime;
        //lastrecordtime = Time.time;
        switch(integrationmethodsindex.value)
        {
            case 0: print("Time consumed through Mid-Point Methods: " + timeconsumed); break;
            case 1: print("Time consumed through Back Euler Methods: " + timeconsumed); break;
            case 2: print("Time consumed through 4nd Runge Kutta Methods: " + timeconsumed); break;
        }
        timeconsumed = 0f;
    }



    void lineupdate()
    {
        if (lineGen == null) lineGen = GameObject.Find("Line");

        LineRenderer IRend = lineGen.GetComponent<LineRenderer>();

        ballposition[0] = firstball.transform.position;
        ballposition[1] = secondball.transform.position;
        IRend.SetPosition(1, ballposition[0]);
        IRend.SetPosition(2, ballposition[1]);
    }

    void lineGenerator()
    {
        if (lineGen == null) lineGen = GameObject.Find("Line");

        LineRenderer IRend = lineGen.GetComponent<LineRenderer>();
        IRend.positionCount = 3;
        IRend.SetPosition(0, new Vector3(0, 5.3f, 0));
        IRend.SetPosition(1, new Vector3(0, 2.52f, 0));
        IRend.SetPosition(2, new Vector3(0, 0, -2f));

    }

    public void SetMassofBall(string mass1)
    {
        massofball = float.Parse(mass1);

    }

    public void SetHook(string k)
    {
        hookcoeff = float.Parse(k);
    }

    public void SetTimeStep(string timestep)
    {
        timestepsize = float.Parse(timestep);
    }

    public void SetDamping(string damp)
    {
        dampingcoeff = float.Parse(damp);
    }

    public void dampset(bool drageflag)
    {
        if(!drageflag)
        {
            dampingcoeff = 0f;
        }
        else
        {
            dampingcoeff = defaultDamping;
        }

    }

    public void Methodchange()  //re-count time
    {

        timeaccounting = 0;
        lastrecordtime = Time.time;
    }
}
