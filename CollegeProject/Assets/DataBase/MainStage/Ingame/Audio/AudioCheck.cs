using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCheck : MonoBehaviour
{
    IEnumerator enumerator;
    Knight player;
    bool playing = false;

    public bool play
    {
        set
        {
            if (value)
            {
                StartCoroutine(enumerator);
            }
            else
            {
                StopCoroutine(enumerator);
                if (playing)
                {
                    FilterManager.Off(12);
                    playing = false;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        enumerator = Aud();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Aud()
    {   
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (player.transform.position.x > 3.8f && player.transform.position.x < 11.2f)
            {
                if (!playing)
                {
                    FilterManager.Play(12);
                    playing = true;
                }
            }
            else
            {
                FilterManager.Off(12);
                playing = false;
            }
        }
    }
}
