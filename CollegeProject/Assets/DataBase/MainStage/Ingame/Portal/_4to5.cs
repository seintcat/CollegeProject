using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _4to5 : MonoBehaviour
{
    static bool once = true;

    public void Portal()
    {
        if (once)
        {
            var obj = FindObjectOfType<Reaper_boss>();
            if (obj != null)
                Destroy(obj.gameObject);

            once = false;
        }

        FilterManager.Off(10);
        FilterManager.Off(25);
        FilterManager.Play(11);
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
