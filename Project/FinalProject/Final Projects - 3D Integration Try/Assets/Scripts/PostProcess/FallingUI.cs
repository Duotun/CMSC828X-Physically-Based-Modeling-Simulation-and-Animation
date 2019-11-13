using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FallingUI : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] plastics = new GameObject[2];
    public GameObject[] HallowWood = new GameObject[2];
    public GameObject[] Wood = new GameObject[2];
    public GameObject[] Aluminum = new GameObject[2];

    public Toggle Plastic_T;
    public Toggle HallowWood_T;
    public Toggle Wood_T;
    public Toggle Aluminum_T;
    //public Toggle Mixed_T;

    GameObject[] plasticobjects;
    GameObject[] Aluminumobjects;
    GameObject[] HallowWoodObjects;
    GameObject[] WoodObjects;
    //GameObject[] MixedObjects;
    void Start()
    {
        
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Plastic_Status(bool status)
    {
        if (status)
        {
            plasticobjects = new GameObject[2];
            plasticobjects[0] = Instantiate(plastics[0], this.transform);
            plasticobjects[1] = Instantiate(plastics[1], this.transform);
            plasticobjects[0].transform.localPosition = new Vector3(Random.Range(-5, -1), Random.Range(2, 4), 0);
            plasticobjects[1].transform.localPosition = new Vector3(Random.Range(1, 5), Random.Range(2,4), 0);
            plasticobjects[0].transform.localRotation = Quaternion.Euler(Random.Range(-15, 25), 0, 0);
            plasticobjects[1].transform.localRotation = Quaternion.Euler(Random.Range(-15,25),0,0);
        }
        else
        {
            Destroy(plasticobjects[0]);
            Destroy(plasticobjects[1]);
        }
       

    }

    public void Aluminum_Status(bool status)
    {
        if (status)
        {
            Aluminumobjects = new GameObject[2];
            Aluminumobjects[0] = Instantiate(Aluminum[0], this.transform);
            Aluminumobjects[1] = Instantiate(Aluminum[1], this.transform);
            Aluminumobjects[0].transform.localPosition = new Vector3(Random.Range(-5, -1), Random.Range(2, 4), 0);
            Aluminumobjects[1].transform.localPosition = new Vector3(Random.Range(1, 5), Random.Range(2, 4), 0);
            Aluminumobjects[0].transform.localRotation = Quaternion.Euler(Random.Range(-15, 25), 0, 0);
            Aluminumobjects[1].transform.localRotation = Quaternion.Euler(Random.Range(-15, 25), 0, 0);
        }
        else
        {
            Destroy(Aluminumobjects[0]);
            Destroy(Aluminumobjects[1]);
        }


    }
    public void Wood_Status(bool status)
    {
        if (status)
        {
            WoodObjects = new GameObject[2];
            WoodObjects[0] = Instantiate(Wood[0], this.transform);
            WoodObjects[1] = Instantiate(Wood[1], this.transform);
            WoodObjects[0].transform.localPosition = new Vector3(Random.Range(-5, -1), Random.Range(2, 4), 0);
            WoodObjects[1].transform.localPosition = new Vector3(Random.Range(1, 5), Random.Range(2, 4), 0);
            WoodObjects[0].transform.localRotation = Quaternion.Euler(Random.Range(-15, 25), 0, 0);
            WoodObjects[1].transform.localRotation = Quaternion.Euler(Random.Range(-15, 25), 0, 0);
        }
        else
        {
            Destroy(WoodObjects[0]);
            Destroy(WoodObjects[1]);
        }


    }

    public void HallowWood_Status(bool status)
    {
        if (status)
        {
            HallowWoodObjects = new GameObject[2];
            HallowWoodObjects[0] = Instantiate(HallowWood[0], this.transform);
            HallowWoodObjects[1] = Instantiate(HallowWood[1], this.transform);
            HallowWoodObjects[0].transform.localPosition = new Vector3(Random.Range(-5, -1), Random.Range(2, 4), 0);
            HallowWoodObjects[1].transform.localPosition = new Vector3(Random.Range(1, 5), Random.Range(2, 4), 0);
            HallowWoodObjects[0].transform.localRotation = Quaternion.Euler(Random.Range(-15, 25), 0, 0);
            HallowWoodObjects[1].transform.localRotation = Quaternion.Euler(Random.Range(-15, 25), 0, 0);
        }
        else
        {
            Destroy(HallowWoodObjects[0]);
            Destroy(HallowWoodObjects[1]);
        }


    }



    public void Restart()
    {
        if(Plastic_T.isOn)
        {
            Plastic_Status(false);
            Plastic_Status(true);
        }

        if (Aluminum_T.isOn)
        {
            Aluminum_Status(false);
            Aluminum_Status(true);
        }

        if(Wood_T.isOn)
        {
            Wood_Status(false);
            Wood_Status(true);
        }
        if(HallowWood_T.isOn)
        {
            HallowWood_Status(false);
            HallowWood_Status(true);
        }
    }
 
}
