using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helm : MonoBehaviour
{
    Knight player;
    IEnumerator enumerator;
    bool isHere = false;
    [SerializeField]
    AudioPlayer audios;

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        audios.SetDelay(1f);

        enumerator = Act();
        StartCoroutine(enumerator);
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Act()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            if ((int)player.transform.position.x == (int)transform.position.x && (int)player.transform.position.z == (int)transform.position.z && !isHere)
            {
                isHere = true;
                audios.PlaySound(28);
                GalleryManager.UnlockData(0, 0);
                ToolTipManager.Show(25);
            }
            else if ((int)player.transform.position.x != (int)transform.position.x || (int)player.transform.position.z != (int)transform.position.z)
                isHere = false;
        }
    }
}
