using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _1to2 : MonoBehaviour
{
    static bool once = true;
    AudioCheck audioCheck;

    public void Portal()
    {
        if (once)
        {
            var obj = FindObjectOfType<Man>();
            if (obj != null)
                obj.Off();

            var obj2 = FindObjectOfType<Door>();
            if (obj2 != null)
                obj2.time = 0f;

            GalleryManager.UnlockData(3, 1);

            once = false;
        }

        FilterManager.Off(7);
        FilterManager.Play(8);

        audioCheck.play = true;
    }

    private void Start()
    {
        var obj3 = FindObjectOfType<AudioCheck>();
        if (obj3 != null)
            audioCheck = obj3;

        if (!once)
            once = true;
    }
}
