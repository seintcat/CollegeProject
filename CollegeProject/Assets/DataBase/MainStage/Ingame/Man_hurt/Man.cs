using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : MonoBehaviour
{
    public GameObject icon;
    IEnumerator enumerator;
    Knight player;
    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField]
    Sprite offs;

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        enumerator = Scene();
        StartCoroutine(enumerator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Scene()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if(Mathf.Abs(player.transform.position.x - transform.position.x) < 1.3f && Mathf.Abs(player.transform.position.z - transform.position.z) < 1.3f)
            {
                Destroy(icon);
                icon = null;
                GalleryManager.UnlockData(2, 1);

                ToolTipManager.Show(13);
                StopCoroutine(enumerator);
            }
        }
    }

    public void Off()
    {
        StopCoroutine(enumerator);
        sprite.sprite = offs;
        if(icon != null)
        {
            Destroy(icon);
            icon = null;
        }
    }
}
