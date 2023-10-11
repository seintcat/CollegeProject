using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2to3 : MonoBehaviour
{
    static bool once = true;
    AudioCheck audioCheck;
    WaitForSeconds wait;
    Reaper_follow reaper;
    public ActorSpawn mino1, mino2;
    static List<Minotaur> minotaurs;
    Knight player;

    public void Portal()
    {
        if (once)
        {
            once = false;
            minotaurs = new List<Minotaur>();

            var obj = FindObjectOfType<Reaper_follow>();
            if (obj != null)
            {
                reaper = obj;
                reaper.gameObject.SetActive(false);
            }
            GalleryManager.UnlockData(3, 3);
            GalleryManager.UnlockData(0, 1);
            ToolTipManager.Show(4);
            player.pause = true;
            StartCoroutine(Check());
        }

        if(minotaurs != null && minotaurs.Count > 0)
            foreach (Minotaur minotaur in minotaurs)
                if (minotaur.enabled)
                {
                    minotaur.gameObject.SetActive(true);
                    minotaur.Resume();
                }

        once = false;
        FilterManager.Off(8);
        FilterManager.Play(9);

        audioCheck.play = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        var obj3 = FindObjectOfType<AudioCheck>();
        if (obj3 != null)
            audioCheck = obj3;
        wait = new WaitForSeconds(0.1f);

        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        if (!once)
            once = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Check()
    {
        yield return wait;
        while (true)
        {
            yield return wait;

            var obj2 = FindObjectOfType<ToolTip>();
            if (obj2 == null)
                break;
        }
        player.pause = false;
        reaper.gameObject.SetActive(true);
        reaper.Play();

        Map.SpawnActor(mino1);
        Map.SpawnActor(mino2);

        var obj = FindObjectsOfType<Minotaur>();
        if (obj != null)
            foreach (Minotaur minotaur in obj)
                minotaurs.Add(minotaur);
    }
}
