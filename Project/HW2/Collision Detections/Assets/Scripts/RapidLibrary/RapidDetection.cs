using UnityEngine;
using System.Runtime.InteropServices;
public class RapidDetection : MonoBehaviour
{
    // Start is called before the first frame update
    [DllImport("dll_initial_1")]
    public static extern float TestSort(int []a,int length);
    [DllImport("exampleproject")]
    public static extern float FooPluginFunction();
    void Start()
    {
        int[] a = new int[5];
        for (int i = 0; i < 5; i++) a[i] = 5 - i;
        print(a[0]);
        TestSort(a, 5);
        print(a[0]);
        print(FooPluginFunction());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
