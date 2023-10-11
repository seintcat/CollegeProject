using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3to4 : MonoBehaviour
{
    static bool once = true;
    List<Minotaur> minotaurs;

    public void Portal()
    {
        if (once)
        {
            var obj3 = FindObjectsOfType<_4to3>();
            if (obj3 != null)
                foreach (_4to3 find in obj3)
                    find.Find();

            var obj = FindObjectOfType<Reaper_follow>();
            if (obj != null)
                Destroy(obj.gameObject);

            once = false;
        }

        minotaurs = new List<Minotaur>();

        var obj2 = FindObjectsOfType<Minotaur>();
        if (obj2 != null)
            foreach (Minotaur minotaur in obj2)
                minotaurs.Add(minotaur);

        if (minotaurs != null && minotaurs.Count > 0)
            foreach (Minotaur minotaur in minotaurs)
                if (minotaur.enabled)
                    minotaur.gameObject.SetActive(false);

        FilterManager.Off(9);
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
