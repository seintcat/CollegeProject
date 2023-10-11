using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _4to3 : MonoBehaviour
{
    static bool once = true;
    List<Minotaur> minotaurs;

    public void Portal()
    {
        if (once)
        {
            once = false;
        }

        if (minotaurs != null && minotaurs.Count > 0)
            foreach (Minotaur minotaur in minotaurs)
                if (minotaur.enabled)
                {
                    minotaur.gameObject.SetActive(true);
                    minotaur.Resume();
                }

        FilterManager.Off(10);
        FilterManager.Play(9);
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

    public void Find()
    {
        if (minotaurs != null && minotaurs.Count > 0)
            return;

        minotaurs = new List<Minotaur>();

        var obj2 = FindObjectsOfType<Minotaur>();
        if (obj2 != null)
            foreach (Minotaur minotaur in obj2)
                minotaurs.Add(minotaur);
    }
}
