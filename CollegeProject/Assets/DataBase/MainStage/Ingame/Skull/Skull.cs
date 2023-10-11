using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    IEnumerator enumerator;
    Knight player;
    public AudioPlayer _audio;
    public GameObject skull;
    public ItemSpawn itemSpawn;

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        enumerator = GiveItem();
        StartCoroutine(enumerator);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GiveItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if ((int)player.transform.position.x == (int)transform.position.x && (int)player.transform.position.z == (int)transform.position.z)
            {
                ToolTipManager.Show(12);
                Item itemgive = Map.SpawnItem(itemSpawn);
                GalleryManager.UnlockData(1, 1);
                player.PickupItem(itemgive);
                Destroy(skull);
                StopCoroutine(enumerator);
            }
        }
    }
}
