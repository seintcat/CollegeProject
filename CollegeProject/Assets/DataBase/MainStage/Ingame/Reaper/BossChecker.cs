using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChecker : MonoBehaviour
{
    Knight player;
    WaitForSeconds wait;
    IEnumerator enumerator;
    public TriggerSpawn trigger;

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        wait = new WaitForSeconds(0.1f);
        enumerator = BossCheck();
        StartCoroutine(enumerator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BossCheck()
    {
        Vector3 position;
        while (true)
        {
            yield return wait;
            position = player.transform.position;
            if ((int)position.x >= (int)transform.position.x && (int)position.z > (int)(transform.position.z - 1) && (int)position.z < (int)(transform.position.z + 1))
            {
                StopCoroutine(enumerator);
                ToolTipManager.Show(7);
                FilterManager.Off(10);
                player.pause = true;
                enumerator = Trigger();
                StartCoroutine(enumerator);
            }
        }
    }

    IEnumerator Trigger()
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

        Map.SpawnTrigger(trigger);
        StopCoroutine(enumerator);
        Destroy(gameObject);
    }
}
