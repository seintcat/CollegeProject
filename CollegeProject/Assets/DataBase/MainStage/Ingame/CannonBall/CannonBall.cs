using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Item
{
    Knight player;
    IEnumerator enumerator;
    [SerializeField]
    GameObject image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override Item Init(int x, int z, int code, int _count, bool __pause, int index)
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        enumerator = GiveItem();
        StartCoroutine(enumerator);

        return base.Init(x, z, code, _count, __pause, index);
    }

    public override void OnEvent(Actor actor)
    {

    }

    public override void UnEquipEvent(Actor actor)
    {

    }

    public override void UseEvent(Actor actor)
    {

    }

    IEnumerator GiveItem()
    {
        bool have = false;
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if ((int)player.transform.position.x == (int)transform.position.x && (int)player.transform.position.z == (int)transform.position.z)
            {
                have = false;
                foreach (Item item in player.itemList)
                    if (item.itemCode == 2)
                        have = true;

                if (!have)
                {
                    player.PickupItem(this);
                    GalleryManager.UnlockData(1, 3);
                    Destroy(image);
                    StopCoroutine(enumerator);
                    FilterManager.Play(23);
                }
            }
        }
    }
}
