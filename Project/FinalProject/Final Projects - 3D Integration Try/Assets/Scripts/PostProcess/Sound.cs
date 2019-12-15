using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.Data.Text;

public class Sound : MonoBehaviour
{
    AudioSource thisaudio;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    //compute force (impulse) first
    void OnCollisionEnter(Collision other)
    {
        
       
       
    }
    IEnumerator soundplay_simulator()
    {
        int sampleFreq = 44000;
        yield return null;
    }

    public void soundplay(float []samples)   //add a values here to pass buffer
    {
        int sampleFreq = 44000;
        float frequency = 440;

        /*float[] samples1 = new float[44000];
        for (int i = 0; i < samples1.Length; i++)
        {
            
            samples1[i] = 1.000000f * Mathf.Sin(Mathf.PI * 2 * i * 2670.117188f / sampleFreq);
        }
        */
        AudioClip ac = AudioClip.Create("Test", samples.Length, 1, sampleFreq, false);
        ac.SetData(samples, 0);
        thisaudio = this.gameObject.GetComponent<AudioSource>();
        thisaudio.clip = ac;
        thisaudio.Play();
        
    }





}
