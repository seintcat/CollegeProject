using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shine : MonoBehaviour
{
    [SerializeField]
    GameObject anim;
    [SerializeField]
    ContextList text, hint;
    [SerializeField]
    AudioPlayer audios;

    bool isHere = false;
    IEnumerator enumerator;
    Knight player;

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
        WaitForSeconds act = new WaitForSeconds(0.1f);
        ContextList _hint = null;

        while (true)
        {
            yield return act;

            if ((int)player.transform.position.x == (int)transform.position.x && (int)player.transform.position.z == (int)transform.position.z && !isHere)
            {
                isHere = true;
                anim.SetActive(false);

                foreach (Item item in player.itemList)
                    if (item.itemCode == 3 && item.count == 2)
                    {
                        GalleryManager.UnlockData(5, 0);
                        player.pause = true;
                        player.enabled = false;
                        player.target = gameObject;
                        audios.PlaySound(29);
                        ToolTipManager.Show(10);
                        yield return new WaitForSeconds(2f);

                        FilterManager.Play(27);
                        ToolTipManager.Show(21);
                        yield return new WaitForSeconds(6f);

                        foreach (Item item2 in player.itemList)
                            if (item2.itemCode == 2)
                            {
                                Save.UnlockPreset(0);
                                _hint = text;
                            }

                        InGameUIManager.StageClear(_hint);
                        StopCoroutine(enumerator);
                        gameObject.SetActive(false);
                        enabled = false;
                        yield return null;
                    }

                ToolTipManager.Show(9);
            }
            else if((int)player.transform.position.x != (int)transform.position.x || (int)player.transform.position.z != (int)transform.position.z)
            {
                isHere = false;
                anim.SetActive(true);
            }
        }
    }
}
