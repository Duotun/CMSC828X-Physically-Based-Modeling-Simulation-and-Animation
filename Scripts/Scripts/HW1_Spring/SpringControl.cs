using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringControl : MonoBehaviour
{

    // Start is called before the first frame update

    [SerializeField]
    public GameObject lineGen;


    //anchorpositions for integratins (restlength)
    public static  Vector3 anchorposition;
    Vector3 currentvelocity;
    public bool isMoving;
    const int MOUSE = 0;
    public Vector3 targetPos;

    private Vector3 mOffset;
    private float mZCoord;

    public float mass = 5.0f;  // 5kg
    public float k = 15.0f;  // 15kg/s
    public bool simulationflag;
    static float stepsize = 0f;  //step size

    void Start()
    {
        anchorposition = transform.position;
        targetPos = transform.position;
        currentvelocity = Vector3.zero;
        simulationflag = false;
        stepsize = Time.fixedDeltaTime * 1f;
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
       if(!RestpositionCheck()&&!simulationflag) // if not on the resposition
        {
            //begin simulation
            if(!Input.GetMouseButtonDown(0))  //not mouse down
            {
                //print("here");
                simulationflag = true;  //only begin this
                Integration_Prepare();
            }
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

        //Vector3 springforce = -k * (transform.position - anchorposition);
        //Vector3 forceinitial = Physics.gravity+springforce;
        StartCoroutine(Integration_begin(mass,transform.position,currentvelocity,newPosition,newVelocity,k));
    }


    IEnumerator Integration_begin(float massnow, Vector3 currentPosition, Vector3 currentVelocity, Vector3 newPosition, Vector3 newVelocity,float k)
    {

        while(true)
        {
            yield return new WaitForSeconds(stepsize);
            //print("simulation: "+ simulationflag);
            //print("Anchor_Position: " + anchorposition);
            print("Current_Position: " + currentPosition);
            //out meanse it is the output value and muse be initilized or updated.
            IntegrationMethods_Spring.CurrentIntegrationMethod(stepsize, currentPosition, currentVelocity, out newPosition, out newVelocity, mass, k);
            currentPosition = newPosition;
            currentVelocity = newVelocity;
            this.GetComponent<Rigidbody>().MovePosition(currentPosition);

            //print("here");
            //line 
            LineRenderer IRend = lineGen.GetComponent<LineRenderer>();
            IRend.SetPosition(1, transform.position);
            //Bullet.transform.position = currentPosition;


            float distance = Mathf.Round((anchorposition - currentPosition).sqrMagnitude);
            if (Mathf.Round(currentVelocity.magnitude) == 0&&distance == 0) break;

            if (Input.GetMouseButton(0)) break;

        }

        simulationflag = false;
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
