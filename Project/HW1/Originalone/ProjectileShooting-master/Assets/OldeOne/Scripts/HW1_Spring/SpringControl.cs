using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SpringControl : MonoBehaviour
{

    // Start is called before the first frame update

    [SerializeField]
    public GameObject lineGen;


    //anchorpositions for integratins (restlength)
    public static  Vector3 anchorposition; 
    public static  Vector3 fixedPosition; // fixed point (white point)
    Vector3 currentvelocity;
    public bool isMoving;
    const int MOUSE = 0;

    public GameObject fixedGameobject;

    private Vector3 mOffset;
    private float mZCoord;

    public float mass = 5.0f;  // 5kg
    public float k = 15.0f;  // 15kg/s
    public bool simulationflag;
    static float stepsize = 0f;  //step size

    public TMP_Dropdown integrationmethodsUI;
    public TMP_InputField input_k;
    public TMP_InputField input_mass;

    int simulationstart = 0;
    void Start()
    {
        anchorposition = transform.position;
        //print("Anchor: " + anchorposition);
        fixedPosition = fixedGameobject.transform.position;
        currentvelocity = Vector3.zero;
        simulationflag = false;
        stepsize = Time.fixedDeltaTime * 1f;

        simulationstart = 0;
        simulationflag = true; //first begin simulation
    }

    private void Awake()
    {
        lineGenerator();  //only two points to represent lines
    }
    // Update is called once per frame
    void Update()
    {
        

    }
  
    //drag and then simulate
    private void FixedUpdate()
    {
       if(simulationflag) // if not on the resposition
        {
            //simulationflag = true;  //only begin this
            if (simulationstart == 0)
            {
                Integration_Prepare();
                simulationstart = 1;
            }
        }

        information_submit();

    }


    void information_submit()
    {
        if (input_k.isFocused && input_k.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            k = int.Parse(input_k.text);
            print("a" + k);
        }

        if (input_mass.isFocused && input_mass.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            mass = int.Parse(input_mass.text);
        }
    }

    void Integration_Prepare()
    {
        Vector3 newPosition = Vector3.zero;  //begin from current
        Vector3 newVelocity = Vector3.zero;

        //may probably use the startcoroutine 

        //Add velocity to the bullet with a rigidbody, Unity internal physics engine
        //newBullet.GetComponent<Rigidbody>().velocity = KgofPowderper / MassofBall * output.transform.up;
        this.GetComponent<Rigidbody>().isKinematic = true;

        Vector3 springforce = -k * (transform.position - anchorposition);
        //print("Springforce: " + springforce);
        //Vector3 forceinitial = Physics.gravity+springforce;
        int methods_chosen = integrationmethodsUI.value;
        StartCoroutine(Integration_begin(mass,transform.position,currentvelocity,newPosition,newVelocity,k,methods_chosen));
    }



    IEnumerator Integration_begin(float massnow, Vector3 currentPosition, Vector3 currentVelocity, Vector3 newPosition, Vector3 newVelocity,float k, int methodsindex)
    {

        while(true)
        {
            yield return new WaitForSeconds(stepsize);
            //print("simulation: "+ simulationflag);
            //print("Anchor_Position: " + anchorposition);
            //print("Current_Position: " + currentPosition);
            //out meanse it is the output value and muse be initilized or updated.
            IntegrationMethods_Spring.CurrentIntegrationMethod(stepsize, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k,methodsindex);
            currentPosition = newPosition;
            currentVelocity = newVelocity;
            this.GetComponent<Rigidbody>().MovePosition(currentPosition);

            //print("here");
            //line 
            LineRenderer IRend = lineGen.GetComponent<LineRenderer>();
            IRend.SetPosition(1, transform.position);
            //Bullet.transform.position = currentPosition;


             float Compositingforce = Mathf.Round(((fixedPosition - currentPosition) * k + Physics.gravity).magnitude);

            //print("Spring force: "+Mathf.Round(((fixedPosition - currentPosition) * k).magnitude));
            //print("Gravity force: " + Mathf.Round((Physics.gravity * mass).magnitude));
            //if (Mathf.Round(currentVelocity.magnitude) == 0&& Compositingforce == 0) break;
            if (simulationflag == false) break;

        }

        //simulationflag = false;
        //yield return new WaitForSeconds(0.8f);

    }

    void OnMouseDown()  //based on the chosen object
    {
        // need to control stoping simulation
            simulationflag = false;

            mZCoord = Camera.main.WorldToScreenPoint(
            gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
       
    }


    private void OnMouseUp()
    {
        simulationflag = true;
        simulationstart = 0;
    }
    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    void OnMouseDrag()
    {
        //transform.LookAt(targetPos);
        transform.position = GetMouseAsWorldPoint() + mOffset;
        
        LineRenderer IRend = lineGen.GetComponent<LineRenderer>();
        IRend.SetPosition(1, transform.position);
    }

    /*
    void MoveObject()
    {
        transform.LookAt(targetPos);
        GetComponent<Rigidbody>().MovePosition(targetPos * Time.fixedDeltaTime);
        if (transform.position == targetPos)
            isMoving = false;

        LineRenderer IRend = lineGen.GetComponent<LineRenderer>();
        IRend.SetPosition(1, transform.position);
    }
    */
    void lineGenerator()
    {
        if (lineGen == null) lineGen = GameObject.Find("Line");

        LineRenderer IRend = lineGen.GetComponent<LineRenderer>();
        IRend.positionCount = 2;
        IRend.SetPosition(0, new Vector3(0, 5.3f, 0));
        IRend.SetPosition(1, new Vector3(0, 2.52f, 0));
        
    }

    bool RestpositionCheck()
    {
        float distance = Mathf.Round((anchorposition - transform.position).sqrMagnitude);
        float currentvelocitymag = Mathf.Round((currentvelocity.sqrMagnitude));
        if (distance == 0 && currentvelocitymag == 0)   // 0 && velocity ==0
        {
            return true;
        }

        return false;
    }


}
