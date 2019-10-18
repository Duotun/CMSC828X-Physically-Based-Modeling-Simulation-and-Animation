using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LOD_Control : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] Bunnies=new GameObject[4];
    public GameObject[] Bananas = new GameObject[4];

    public int size = 1;

    //two toggles here
    bool bunnyflag = true;
    bool bannaflag = true;

    GameObject[] Produced_Bunnies = new GameObject[12];
    GameObject[] Produced_Bananas = new GameObject[12];


    float mass_bunny = 1.0f;
    float mass_banana = 1.0f;

    float velocity = 3f;
    int LOD = 0;

    public Transform anchortransform;
    public Transform anchortransform_Bunny;
    public Text lodtext;

    int collision;
    void Start()
    {
        if(bannaflag)
        StartCoroutine(Produce_Banana());
        if(bunnyflag)
        StartCoroutine(Produce_Bunny());

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(input_judge());
        LODSet();
    }

    
    void LODSet()
    {
        lodtext.text = "Level of Details (LOD): " + LOD;
        
    }

    public void setCollisionMode(bool flag)
    {

        if(bunnyflag)
        {
            for(int i=0;i<Produced_Bunnies.Length;i++)
            {
                if (flag)
                    Produced_Bunnies[i].transform.GetChild(0).GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                else
                    Produced_Bunnies[i].transform.GetChild(0).GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;

            }
        }
        if(bannaflag)
        {
            for (int i = 0; i < Produced_Bananas.Length; i++)
            {
                if (flag)
                    Produced_Bananas[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                else
                    Produced_Bananas[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
        }
    }

    IEnumerator input_judge()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LOD = 0;
            if(bunnyflag)
            {
                yield return Destroy_Bunny();
                yield return Produce_Bunny();
               
            }
            if(bannaflag)
            {
                yield return Destroy_Banana();
                yield return Produce_Banana();

            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LOD = 1;
            if (bunnyflag)
            {
                yield return Destroy_Bunny();
                yield return Produce_Bunny();

            }
            if (bannaflag)
            {
                yield return Destroy_Banana();
                yield return Produce_Banana();

            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LOD = 2;
            if (bunnyflag)
            {
                yield return Destroy_Bunny();
                yield return Produce_Bunny();

            }
            if (bannaflag)
            {
                yield return Destroy_Banana();
                yield return Produce_Banana();

            }

        }
        yield return null;
    }

    IEnumerator Produce_Bunny()
    {
        
        for (int i = 0; i < Produced_Bunnies.Length; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Produced_Bunnies[i] = Instantiate(Bunnies[LOD], anchortransform_Bunny);
            Produced_Bunnies[i].transform.GetChild(0).GetComponent<Rigidbody>().velocity = velocity * new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            Produced_Bunnies[i].transform.GetChild(0).GetComponent<Rigidbody>().mass = mass_bunny;
        }
    }

    IEnumerator Destroy_Bunny()
    {
        for (int i = 0; i < Produced_Bunnies.Length; i++)
        {
            Destroy(Produced_Bunnies[i]);
        }
        yield return null;
    }

    IEnumerator Produce_Banana()
    {
        //print(Produced_Bananas.Length);
        for(int i=0;i<Produced_Bananas.Length;i++)
        {
            yield return new WaitForSeconds(0.05f);
            Produced_Bananas[i] = Instantiate(Bananas[LOD], anchortransform);
            Produced_Bananas[i].GetComponent<Rigidbody>().velocity = velocity*new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            Produced_Bananas[i].GetComponent<Rigidbody>().mass = mass_banana;
        }
    }

    IEnumerator Destroy_Banana()
    {
        for (int i = 0; i < Produced_Bananas.Length; i++)
        {
             Destroy(Produced_Bananas[i]);
        }
        yield return null;
    }

    public void setmass_1(string massnew)
    {
        mass_bunny = float.Parse(massnew);
        if (bunnyflag)
        {
            for (int i = 0; i < Produced_Bunnies.Length; i++)
            {

                Produced_Bunnies[i].transform.GetChild(0).GetComponent<Rigidbody>().mass = mass_bunny;

            }
        }
       

    }

    public void setmass_2(string massnew)
    {
        mass_banana = float.Parse(massnew);
        if (bannaflag)
        {
            for (int i = 0; i < Produced_Bananas.Length; i++)
            {
                Produced_Bananas[i].GetComponent<Rigidbody>().mass = mass_banana;
            }
        }


    }

   


    public void setvelocity(string velocitynew)
    {
        velocity = float.Parse(velocitynew);
        if (bunnyflag)
        {
            for (int i = 0; i < Produced_Bunnies.Length; i++)
            {

                Produced_Bunnies[i].transform.GetChild(0).GetComponent<Rigidbody>().velocity = velocity*new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));

            }
        }
        if (bannaflag)
        {
            for (int i = 0; i < Produced_Bananas.Length; i++)
            {
                Produced_Bananas[i].GetComponent<Rigidbody>().velocity = velocity* new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            }
        }
    }

    public void setsize_1(string sizenew)
    {
        size = int.Parse(sizenew);
        if (bunnyflag)
        {
            for (int i = 0; i < Produced_Bananas.Length; i++)
            {
                Produced_Bunnies[i].GetComponent<Transform>().localScale = new Vector3(size, size, size);
            }
        }
    }

    public void setsize_2(string sizenew)
    {

        size = int.Parse(sizenew);
        if (bannaflag)
        {
            for (int i = 0; i < Produced_Bananas.Length; i++)
            {
                Produced_Bananas[i].GetComponent<Transform>().localScale = new Vector3(size, size, size);
            }
        }
    }
    public void BananaProduced(bool flag)
    {
       
        bannaflag = flag;
        if(bannaflag)
        {
            StartCoroutine(Produce_Banana());
        }
        else
        {
            StartCoroutine(Destroy_Banana());
        }
    }

    public void BunnyProduced(bool flag)
    {
        
        bunnyflag = flag;
        if (bunnyflag)
        {
            StartCoroutine(Produce_Bunny());
        }
        else
        {
            StartCoroutine(Destroy_Bunny());
        }
    }

}
