using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.Data.Text;
public class EVD : MonoBehaviour
{
    // Start is called before the first frame update

    public MeshFilter Object;
    Vector3[] Vertices;
    int[] Triangles;

    float youngs;
    float Poission;
    float rho;
    float m;
    float thickness;  //constant here
    Matrix<float> Mass;
    Matrix<float> Stiff;

    void Start()
    {
        InitialMeshData();
        ConstructStiffMatrix();
        ConstructMassMatrix();   //let matlab to compute a more precise evd
        //extmat();

    }


    void InitialMeshData()
    {
        Vertices = Object.sharedMesh.vertices;
        /*for(int i=0;i<Vertices.Length;i++)
        {
            print("x: " + Vertices[i].x + "y: " + Vertices[i].y + "z: " + Vertices[i].z);
        }
        */
        Triangles = Object.sharedMesh.triangles;
        youngs = 20e10f;    //aluminum
        rho = 2.4e3f;
        /*
        youngs = 9e10f;    //wo
        rho = 0.4e3f;

         youngs = 9e9f;    //aluminum
        rho = 2.7e3f;
         */
        m = rho * VolumeOfMesh(Object.sharedMesh);
        thickness = 0.1f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void getMeshdata()
    {

    }

    void ConstructMassMatrix()
    {
        Mass= Matrix<float>.Build.Sparse(Vertices.Length, Vertices.Length,0f);
        float vertexmass = m / Vertices.Length;
        
        for(int i=0;i<Vertices.Length;i++)
        {

            Mass[i, i] = vertexmass;
        }
        savematrix(ref Mass, 1);
    }
    
    void ConstructStiffMatrix()
    {
        Stiff = Matrix<float>.Build.Sparse(Vertices.Length, Vertices.Length, 0f);  //temporarily dense matrix
        float k = youngs * thickness;  //here assumed constant
        for(int i=0;i<Triangles.Length;i+=3)
        {
            //symmetric
            Stiff[Triangles[i], Triangles[i+1]] = -k;
            Stiff[Triangles[i+1], Triangles[i]] = -k;
            Stiff[Triangles[i+1], Triangles[i + 2]] = -k;
            Stiff[Triangles[i+2], Triangles[i + 1]] = -k;
            Stiff[Triangles[i+2], Triangles[i]] = -k;
            Stiff[Triangles[i], Triangles[i+2]] = -k;

        }

        savematrix(ref Stiff,0);
    }


    public static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    public static float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
     {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }

    public static float areaofmesh()
    {
        return 0f;
    }

    public void extmat()  //triangular mesh now
    {
        //because here my mass matrix is a diagonal matrix
        var c = Mass.Inverse().Multiply(Stiff);
        MathNet.Numerics.LinearAlgebra.Factorization.Evd<float> eigen = c.Evd();
        
        Matrix<float> G = eigen.EigenVectors;
        Matrix<float> D = eigen.EigenValues.Real().ToColumnMatrix().Map(x => (float)x);
        
        //print(eigen.D);
        //print(G.Transpose()*eigen.D * G);
        savematrix(ref G, 2);
        savematrix(ref D, 3);
        
    }

    public void savematrix(ref Matrix<float> stored, int casenum)
    {
        switch (casenum)
        {
            case 0: DelimitedWriter.Write("Kmatrix.csv", stored, ",");break;
            case 1: DelimitedWriter.Write("Mmatrix.csv", stored, ","); break;
            case 2: DelimitedWriter.Write("Gmatrix.csv", stored, ","); break;
            case 3: DelimitedWriter.Write("Dmatrix.csv", stored, ",");break;

        }
        
    }


}
