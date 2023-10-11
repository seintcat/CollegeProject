using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    Knight player;
    [SerializeField]
    SpriteRenderer image;
    IEnumerator enumerator, fire;
    Color color;
    public float shotTime;
    float shot;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Sprite deathSprite;
    [SerializeField]
    ContextList deathText;
    [SerializeField]
    AudioPlayer audios;

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        color = new Color(255, 255, 255, 0);
        image.color = color;

        shot = shotTime + 1f;

        enumerator = Shot();
        fire = Fire();
        StartCoroutine(enumerator);
    }

    // Update is called once per frame
    void Update()
    {
        if(shot < shotTime)
        {
            shot += Time.deltaTime;
            if(shot > shotTime)
            {
                color.a = 0;
                image.color = color;
                StartCoroutine(fire);
                return;
            }

            color.a = (shot / shotTime);
            image.color = color;
        }
    }

    IEnumerator Shot()
    {
        Item ball;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if ((int)player.transform.position.x == (int)transform.position.x && (int)player.transform.position.z == (int)transform.position.z && shot > shotTime)
            {
                ball = null;
                foreach (Item item in player.itemList)
                    if (item.itemCode == 2)
                        ball = item;

                if (ball != null)
                {
                    player.UseItem(ball);
                    shot = 0f;
                    color.a = 0;
                    image.color = color;
                    audios.SetDelay(2);
                    audios.PlaySound(22);
                }
            }
        }
    }

    IEnumerator Fire()
    {
        bool exp;
        var obj2 = FindObjectsOfType<Minotaur>();

        while (true)
        {
            exp = false;
            if (obj2 != null)
                foreach (Minotaur minotaur in obj2)
                    minotaur.Damage(1f, 0.5f);

            audios.SetDelay(3);
            audios.PlaySound(20);
            animator.Play("Attack");
            float x = player.transform.position.x - transform.position.x, z = player.transform.position.z - transform.position.z;
            if (x < 1.3f && x > -1.3f && z < 1.3f && z > -1.3f && !player.killed)
            {
                exp = true;
                player.killed = true;
            }

            yield return new WaitForSeconds(1f);
            animator.Play("Idle");
            if (exp)
            {
                GalleryManager.UnlockData(4, 4);
                InGameUIManager.GameOver(deathSprite, deathText);
                ToolTipManager.Show(30);
            }

            yield return new WaitForSeconds(0.1f);
            StopCoroutine(fire);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
