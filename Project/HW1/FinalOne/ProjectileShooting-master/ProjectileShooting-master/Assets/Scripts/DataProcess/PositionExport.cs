using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;
public class PositionExport : MonoBehaviour
{
    // Start is called before the first frame update


    //for two-ball spring, use rigidbody to get analytic solutions

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    private void OnApplicationQuit()
    {
        //StartCoroutine(CrearArchivoCSV("cool"));
    }

    IEnumerator CrearArchivoCSV(string nombreArchivo)
    {

        string ruta = Application.streamingAssetsPath + "/" + nombreArchivo + ".csv";

        //El archivo existe? lo BORRAMOS
        if (File.Exists(ruta))
        {
            File.Delete(ruta);
        }

        //Crear el archivo
        var sr = File.CreateText(ruta);

        string datosCSV = "valor1,valor2,valor3,valor4" + System.Environment.NewLine;
        datosCSV += "valor1,valor2,valor3,valor4" + System.Environment.NewLine;
        datosCSV += "valor1,valor2,valor3,valor4" + System.Environment.NewLine;
        datosCSV += "valor1,valor2,valor3,valor4" + System.Environment.NewLine;
        datosCSV += "valor1,valor2,valor3,valor4";

        sr.WriteLine(datosCSV);

        
        FileInfo fInfo = new FileInfo(ruta);
        fInfo.IsReadOnly = true;

        //Cerrar
        sr.Close();

        yield return new WaitForSeconds(0.5f);//Esperamos para estar seguros que escriba el archivo

        //Abrimos archivo recien creado
        //Application.OpenURL(ruta);
    }

}
