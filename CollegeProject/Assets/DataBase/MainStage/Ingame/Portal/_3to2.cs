using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3to2 : MonoBehaviour
{
    static bool once = true;
    AudioCheck audioCheck;
    static List<Minotaur> minotaurs;

    public void Portal()
    {
        if (once)
        {
            minotaurs = new List<Minotaur>();

            var obj2 = FindObjectsOfType<Minotaur>();
            if (obj2 != null)
                foreach (Minotaur minotaur in obj2)
                    minotaurs.Add(minotaur);

            once = false;
        }

        if (minotaurs != null && minotaurs.Count > 0)
            foreach (Minotaur minotaur in minotaurs)
                if (minotaur.enabled)
                    minotaur.gameObject.SetActive(false);

        FilterManager.Off(9);
        FilterManager.Play(8);

        audioCheck.play = true;
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
