using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.Data.Text;
public class Click_Aluminum : MonoBehaviour
{
    // Start is called before the first frame update
    Sound Sound;
    float forceamp=550;
    float forceoffset=10f;   //directly controlled in the scripts
    Vector3[] displacedVertices;

    Matrix<float> G;
    Matrix<float> D;
    Matrix<float> M;
    Vector<float> gain;

    float Beta = 0.000000000023f;
    float Alpha = 0.000080f;  //rayleigh damping, metal

    // object name
    public string objname="";
    void Start()
    {
        Control.UseNativeMKL();
        G = DelimitedReader.Read<float>("Gmatrix"+objname+".csv", false, ",", false);
        D = DelimitedReader.Read<float>("Dmatrix"+objname+".csv", false, ",", false);
        //print("Gmatrix" + objname + ".csv");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    //change to arbitrary force impulse ()  
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            Sound = hit.collider.GetComponent<Sound>();
            Vector3 point = hit.point;
            if (Sound)
            {
                displacedVertices = hit.collider.GetComponentInParent<MeshFilter>().sharedMesh.vertices;
                for (int i = 0; i < displacedVertices.Length; i++)
                {
                    displacedVertices[i] = transform.TransformPoint(displacedVertices[i]);
                }
                point += hit.normal * forceoffset;
              
                AddForce(point, forceamp);
            }

        }
    }

    public void AddForce(Vector3 point, float force)
    {
        var F = Vector<float>.Build;
        Vector<float> tmpForce = F.Dense(displacedVertices.Length);

        for (int i = 0; i < displacedVertices.Length; i++)
        {
            AddForceVertex(i, point, force, ref tmpForce);

        }

        Matrix<float> ff = Matrix<float>.Build.Dense(displacedVertices.Length / 3, 3);
        for (int i = 0; i < displacedVertices.Length / 3; i++)
        {
            ff[i, 0] = point.x;
            ff[i, 1] = point.y;
            ff[i, 2] = point.z;
        }
        // Debug.DrawLine(Camera.main.transform.position, point);
        //print(tmpForce);
        gain = G.Transpose().Multiply(tmpForce);
      
        //gain_process();
        SoundSimulator(gain);
    }

    void AddForceVertex(int i, Vector3 point, float force, ref Vector<float> tmpForce)
    {
        Vector3 pointToVertex = point - displacedVertices[i];
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        //var f = pointToVertex.normalized * attenuatedForce;
        tmpForce[i] = attenuatedForce;
    }

    void SoundSimulator(Vector<float> gain)
    {
        float[] samples = new float[44100];
        //simulate 1 seconds
        int sampleFreq = 44100;
        //print(D.RowCount);

        for (int i = 0; i < D.RowCount; i++)
        {

            float d = 0.5f * (Alpha + Beta * D[i, i]);
            if ((D[i, i] - d * d) < 0 || D[i, i] <= 0) continue;
            float omega = Mathf.Sqrt(D[i, i] - d * d);
            //print(d);      
            //omega = Mathf.PI * 2 * 2670.117188f;
            for (int j = 0; j < samples.Length; j++)
            {

                samples[j] += gain[i] * Mathf.Exp(-d * j) * Mathf.Sin(j * omega / sampleFreq);
            }

        }

        Sound.soundplay(samples);

    }

    void gain_process()
    {
        float maxnum = 0.0f;
        maxnum = gain.AbsoluteMaximum();
        for (int i = 0; i < gain.Count; i++)
        {
            gain[i] /= maxnum;
        }
    }
}
