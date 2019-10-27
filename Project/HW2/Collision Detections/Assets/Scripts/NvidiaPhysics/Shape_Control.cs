using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Control : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Shapes = new GameObject[4];

    GameObject[] Produced_Cube = new GameObject[8];
    GameObject[] Produced_Cone = new GameObject[8];
    GameObject[] Produced_Capsule = new GameObject[8];
    GameObject[] Produced_Cylinder = new GameObject[8];

    //four toggles here
    bool cubeflag = true;
    bool coneflag = true;
    bool capsuleflag = true;
    bool cylinderflag = true;


    public Transform Anchortransform;
    float velocity;

    float mass_cube = 1.0f;
    float mass_cone = 1.0f;
    float mass_capsule = 1.0f;
    float mass_cylinder = 1.0f;

    float size_now = 1.0f;
    void Start()
    {
        if (cubeflag)
            StartCoroutine(Produce_Cube());
        if (coneflag)
            StartCoroutine(Produce_Cone());
        if (capsuleflag)
            StartCoroutine(Produce_Capsule());
        if (cylinderflag)
            StartCoroutine(Produce_Cylinder());
    }

    IEnumerator Produce_Cube()
    {

        for (int i = 0; i < Produced_Cube.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Produced_Cube[i] = Instantiate(Shapes[0], Anchortransform);
            Random.InitState(System.DateTime.Now.Millisecond);
   
            Produced_Cube[i].GetComponent<Rigidbody>().velocity = velocity * new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f));
            Produced_Cube[i].GetComponent<Rigidbody>().mass = mass_cube;
        }
        yield return null;
    }

    IEnumerator Produce_Cone()
    {
        for (int i = 0; i < Produced_Cone.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Produced_Cone[i] = Instantiate(Shapes[1], Anchortransform);
            Random.InitState(System.DateTime.Now.Millisecond);
            Produced_Cone[i].GetComponent<Rigidbody>().velocity = velocity * new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f));
            Produced_Cone[i].GetComponent<Rigidbody>().mass = mass_cone;
        }
        yield return null;
    }

    IEnumerator Produce_Capsule()
    {
        for (int i = 0; i < Produced_Capsule.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Produced_Capsule[i] = Instantiate(Shapes[2], Anchortransform);
            Random.InitState(System.DateTime.Now.Millisecond);
            Produced_Capsule[i].GetComponent<Rigidbody>().velocity = velocity * new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f));
            Produced_Capsule[i].GetComponent<Rigidbody>().mass = mass_capsule;

        }
        yield return null;
    }

    IEnumerator  Produce_Cylinder()
    {
        for (int i = 0; i < Produced_Cylinder.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Produced_Cylinder[i] = Instantiate(Shapes[3], Anchortransform);
            Random.InitState(System.DateTime.Now.Millisecond);
            Produced_Cylinder[i].GetComponent<Rigidbody>().velocity = velocity * new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            Produced_Cylinder[i].GetComponent<Rigidbody>().mass = mass_cylinder;

        }
        yield return null;
    }

    public void setvelocity(string velocitynew)
    {
        velocity = float.Parse(velocitynew);
        if (cubeflag)
        {
            for (int i = 0; i < Produced_Cube.Length; i++)
            {

                Produced_Cube[i].GetComponent<Rigidbody>().velocity = velocity * new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));

            }
        }
        if (coneflag)
        {
            for (int i = 0; i < Produced_Cone.Length; i++)
            {
                Produced_Cone[i].GetComponent<Rigidbody>().velocity = velocity * new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            }
        }

        if (cylinderflag)
        {
            for (int i = 0; i < Produced_Cone.Length; i++)
            {
                Produced_Cone[i].GetComponent<Rigidbody>().velocity = velocity * new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            }
        }

        if (capsuleflag)
        {
            for (int i = 0; i < Produced_Capsule.Length; i++)
            {
                Produced_Capsule[i].GetComponent<Rigidbody>().velocity = velocity * new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            }
        }
    }

        IEnumerator  Destroy_Cube()
    {
        for (int i = 0; i < Produced_Cube.Length; i++)
        {
            Destroy(Produced_Cube[i]);
        }

        yield return null;
    }

    IEnumerator  Destroy_Cone()
    {
        for (int i = 0; i < Produced_Cone.Length; i++)
        {
            Destroy(Produced_Cone[i]);
        }

        yield return null;
    }

    IEnumerator Destroy_Capsule()
    {
        for (int i = 0; i < Produced_Capsule.Length; i++)
        {
            Destroy(Produced_Capsule[i]);
        }

        yield return null;
    }

    IEnumerator Destroy_Cylinder()
    {
        for (int i = 0; i < Produced_Cylinder.Length; i++)
        {
            Destroy(Produced_Cylinder[i]);
        }

        yield return null;

    }


    public void setCollisionMode(bool flag)
    {

        if (cubeflag)
        {
            for (int i = 0; i < Produced_Cube.Length; i++)
            {
                if (flag)
                    Produced_Cube[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                else
                    Produced_Cube[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;

            }
        }
        if (coneflag)
        {
            for (int i = 0; i < Produced_Cone.Length; i++)
            {
                if (flag)
                    Produced_Cone[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                else
                    Produced_Cone[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
        }
        if(capsuleflag)
        {
            for(int i = 0; i < Produced_Capsule.Length; i++)
            {
                if (flag)
                    Produced_Capsule[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                else
                    Produced_Capsule[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
        }
        if(cylinderflag)
        {
            for(int i = 0; i < Produced_Cylinder.Length; i++)
            {
                if (flag)
                    Produced_Cylinder[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                else
                    Produced_Cylinder[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
        }
    }


    public void CubeProduced(bool flag)
    {

        cubeflag = flag;
        if (cubeflag)
        {
            StartCoroutine(Produce_Cube());
        }
        else
        {
            StartCoroutine(Destroy_Cube());
        }
    }

    public void ConeProduced(bool flag)
    {

        coneflag = flag;
        if (coneflag)
        {
            StartCoroutine(Produce_Cone());
        }
        else
        {
            StartCoroutine(Destroy_Cone());
        }
    }

    public void CapsuleProduced(bool flag)
    {

        capsuleflag = flag;
        if (capsuleflag)
        {
            StartCoroutine(Produce_Capsule());
        }
        else
        {
            StartCoroutine(Destroy_Capsule());
        }
    }

    public void CylinderProduced(bool flag)
    {

        cylinderflag = flag;
        if (cylinderflag)
        {
            StartCoroutine(Produce_Cylinder());
        }
        else
        {
            StartCoroutine(Destroy_Cylinder());
        }
    }


    public void setobject_size(string massnew)
    {
        size_now= float.Parse(massnew);
    }


    public void setmass_1(string massnew)
    {
        mass_cube = float.Parse(massnew);
        if (cubeflag)
        {
            for (int i = 0; i < Produced_Cube.Length; i++)
            {

                Produced_Cube[i].GetComponent<Rigidbody>().mass = mass_cone;

            }
        }
    }

        public void setmass_2(string massnew)
        {
            mass_cone = float.Parse(massnew);
            if (coneflag)
            {
                for (int i = 0; i < Produced_Cone.Length; i++)
                {

                    Produced_Cone[i].GetComponent<Rigidbody>().mass = mass_cone;

                }
            }
        }

    public void setmass_3(string massnew)
    {
        mass_capsule = float.Parse(massnew);
        if (capsuleflag)
        {
            for (int i = 0; i < Produced_Capsule.Length; i++)
            {
                Produced_Capsule[i].GetComponent<Rigidbody>().mass = mass_capsule;
            }
        }


    }

    public void setmass_4(string massnew)
    {
        mass_cylinder = float.Parse(massnew);
        if (cylinderflag)
        {
            for (int i = 0; i < Produced_Cylinder.Length; i++)
            {
                Produced_Cylinder[i].GetComponent<Rigidbody>().mass = mass_cylinder;
            }
        }


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
