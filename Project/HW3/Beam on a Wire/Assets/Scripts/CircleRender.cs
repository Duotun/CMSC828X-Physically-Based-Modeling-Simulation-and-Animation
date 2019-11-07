using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRender : MonoBehaviour
{
    public int vertexCount = 40; //more vertices, smoother will be
    public float linewidth = 0.1f;
    float radius;  //updates with balls
    // Start is called before the first frame update

    private LineRenderer mylineRenderer;
    public bool circleFillscreen;
    private void Awake()
    {
        radius = SimulationPrep.R;
        mylineRenderer = GetComponent<LineRenderer>();
        SetupCircle();
    }
    
    private void SetupCircle()
    {
        mylineRenderer.widthMultiplier = linewidth;
        if(circleFillscreen)
        {
            radius = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0,0f)), -Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelRect.xMax, 0, 0f)))*0.28f - linewidth;
        }
        //print(Camera.main.pixelRect.width);
        //print(Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelRect.xMin, 0f)));
        //print(Camera.main.pixelRect.yMin);
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0.0f;
        mylineRenderer.positionCount = vertexCount;
        for(int i =0;i<mylineRenderer.positionCount;i++)
        {
            Vector3 pos = new Vector4(radius * Mathf.Cos(theta), 0f, radius * Mathf.Sin(theta));
            mylineRenderer.SetPosition(i, pos);
            theta += deltaTheta;

        }

    }
    void Start()
    {

    }

    //preview in unityeditor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float deltathetheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;
        Vector3 oldPos = new Vector3(radius,0,0);
        for(int i=0;i<vertexCount+1;i++)
        {
            Vector3 pos = new Vector4(radius * Mathf.Cos(theta), 0f, radius * Mathf.Sin(theta));
            Gizmos.DrawLine(oldPos, transform.position + pos);
            oldPos = transform.position + pos;
            theta += deltathetheta;
        }
    }
#endif
    // Update is called once per frame
    void Update()
    {
        
        if(radius!=SimulationUI.R)
        {
            //update radius to fit change
            
            radius = SimulationUI.R;
            SetupCircle();
           
        }

        
        
    }


}
