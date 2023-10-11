using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    IEnumerator enumerator;
    Knight player;
    public Animator animator;
    public AudioPlayer _audio;
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
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 1.3f && Mathf.Abs(player.transform.position.z - transform.position.z) < 1.3f)
            {
                animator.Play("Open");
                _audio.SetDelay(1);
                _audio.PlaySound(5);
                GalleryManager.UnlockData(1, 0);
                Item itemgive = Map.SpawnItem(itemSpawn);   
                player.PickupItem(itemgive);
                StopCoroutine(enumerator);
            }
        }
    }
}
