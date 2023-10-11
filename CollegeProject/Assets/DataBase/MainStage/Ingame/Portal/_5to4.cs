using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _5to4 : MonoBehaviour
{
    static bool once = true;

    public void Portal()
    {
        if (once)
        {

            once = false;
        }

        FilterManager.Off(11);
        FilterManager.Play(10);
    }

    // Start is called before the first frame update
    void Start()
    {


        if (!once)
            once = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
