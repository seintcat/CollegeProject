using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2to1 : MonoBehaviour
{
    static bool once = true;
    AudioCheck audioCheck;

    public void Portal()
    {
        if (once)
        {

            once = false;
        }

        FilterManager.Off(8);
        FilterManager.Play(7);

        audioCheck.play = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        var obj3 = FindObjectOfType<AudioCheck>();
        if (obj3 != null)
            audioCheck = obj3;

        if (!once)
            once = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
