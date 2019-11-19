using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator toggles;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleControl(bool toggleclick)
    {
        if(this.gameObject!=null)
        {
            if(toggles!=null)
            {
               
                toggles.SetBool("Click", toggleclick);
            }
        }
    }
}
