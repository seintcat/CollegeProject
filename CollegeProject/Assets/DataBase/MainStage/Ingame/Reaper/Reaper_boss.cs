using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_boss : MonoBehaviour
{
    [SerializeField]
    Sprite deathSprite;
    [SerializeField]
    ContextList deathText;
    [SerializeField] 
    Animator animator;
    [SerializeField] 
    Movement movement;
    [SerializeField]
    AudioPlayer audios;

    Knight player;

    public float speed, skillTime;
    bool follow = false;
    float timer;

    IEnumerator enumerator, skill;
    WaitForSeconds act;

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        movement.Init((int)transform.position.x, (int)transform.position.z, MoveType.Fly, speed);
        FilterManager.Play(25);
        act = new WaitForSeconds(0.1f);
        timer = 0f;
        audios.SetDelay(2.5f);

        enumerator = Act();
        skill = Skill();
        StartCoroutine(enumerator);
        StartCoroutine(skill);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (follow)
        {
            Vector3 pos = transform.position;
            pos.x = player.transform.position.x - 1;
            transform.position = pos;
        }
    }

    IEnumerator Act()
    {
        while (true)
        {
            yield return act;

            if(transform.position.x >= player.transform.position.x && player.transform.position.z < transform.position.z + 2 && player.transform.position.z > transform.position.z - 2)
            {
                StopCoroutine(skill);

                follow = true;
                animator.Play("Attack");
                movement.enabled = false;
                yield return new WaitForSeconds(0.5f);
                player.killed = true;
                yield return new WaitForSeconds(0.5f);
                animator.Play("Idle");
                yield return new WaitForSeconds(1f);

                GalleryManager.UnlockData(4, 0);
                InGameUIManager.GameOver(deathSprite, deathText);
                ToolTipManager.Show(3);

                StopCoroutine(enumerator);
                enabled = false;
            }
            else
                movement.Move((int)transform.position.x + 1, (int)transform.position.z);
        }
    }

    IEnumerator Skill()
    {
        TriggerSpawn spawn = new TriggerSpawn();
        spawn.code = 22;
        int rand;
        while (true)
        {
            yield return act;
            if(timer > skillTime)
            {
                audios.PlaySound(26);
                timer = 0f;
                animator.Play("Skill2");
                yield return new WaitForSeconds(0.8f);

                spawn.x = (int)player.transform.position.x + 5;
                rand = Random.Range(0, 3);
                if (rand != 0)
                {
                    spawn.z = (int)transform.position.z - 1;
                    Map.SpawnTrigger(spawn);
                }
                if (rand != 1)
                {
                    spawn.z = (int)transform.position.z;
                    Map.SpawnTrigger(spawn);
                }
                if (rand != 2)
                {
                    spawn.z = (int)transform.position.z + 1;
                    Map.SpawnTrigger(spawn);
                }

                yield return new WaitForSeconds(0.8f);
                animator.Play("Idle");
            }
        }
    }
}
