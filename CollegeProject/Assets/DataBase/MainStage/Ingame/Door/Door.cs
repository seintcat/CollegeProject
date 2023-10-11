using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject counter, spawnObj;
    public float count;
    [HideInInspector]
    public float time;
    Bar bar;
    public Animator animator;
    public AudioPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject barObj =  Instantiate(counter);
        bar = barObj.GetComponent<Bar>();
        time = count;
    }

    // Update is called once per frame
    void Update()
    {
        if(bar != null)
        {
            time -= Time.deltaTime;

            bar.value = time / count;

            if (time < 0f)
            {
                Destroy(bar.gameObject);
                bar = null;
                StartCoroutine(Spawn());
            }
        }
    }

    IEnumerator Spawn()
    {
        GalleryManager.UnlockData(3, 0);
        animator.Play("Open");
        player.SetDelay(3);
        player.PlaySound(3);
        GameObject obj = Instantiate(spawnObj);
        obj.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z - 1);
        yield return new WaitForSeconds(1f);
        animator.Play("Close");
    }
}
