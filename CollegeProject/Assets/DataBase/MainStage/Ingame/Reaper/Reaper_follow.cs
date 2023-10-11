using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_follow : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float followTime, attackDelay, speed;
    public int teleportChance;
    readonly float killDelay = 0.5f, teleportDelay = 0.6f, deathDelay = 1f;

    WaitForSeconds follow, attack, teleport, kill, death;
    Knight player;
    public Movement movement;
    IEnumerator enumerator;

    [SerializeField]
    Sprite deathSprite;
    [SerializeField]
    ContextList deathText;

    // Start is called before the first frame update
    void Start()
    {
        follow = new WaitForSeconds(followTime);
        attack = new WaitForSeconds(attackDelay);
        teleport = new WaitForSeconds(teleportDelay);
        kill = new WaitForSeconds(killDelay);
        death = new WaitForSeconds(deathDelay);

        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];

        movement.Init((int)transform.position.x, (int)transform.position.z, MoveType.Fly, speed);

        enumerator = Follow();
        StartCoroutine(enumerator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Follow()
    {
        Vector2 spot;
        while (true)
        {
            yield return follow;

            var obj = FindObjectsOfType<ToolTip>();
            if (obj != null && obj.Length > 0)
                continue;

            spot = new Vector2(player.transform.position.x, player.transform.position.z);
            animator.Play("Idle");
            yield return attack;
            if ((int)spot.x == (int)transform.position.x && (int)spot.y == (int)transform.position.z)
            {
                animator.Play("Attack");
                yield return kill;

                spot = new Vector2(player.transform.position.x, player.transform.position.z);
                if ((int)spot.x == (int)transform.position.x && (int)spot.y == (int)transform.position.z && !player.killed)
                {
                    player.killed = true;

                    yield return kill;
                    animator.Play("Idle");

                    yield return death;
                    GalleryManager.UnlockData(4, 0);
                    InGameUIManager.GameOver(deathSprite, deathText);
                    ToolTipManager.Show(3);

                    StopCoroutine(enumerator);
                    enabled = false;
                }
                else
                {
                    yield return kill;
                    animator.Play("Idle");
                }
            }
            else
            {

                if (teleportChance > Random.Range(0, 100))
                {
                    animator.Play("Skill1");
                    yield return teleport;
                    spot = new Vector2((int)player.transform.position.x, (int)player.transform.position.z);
                    transform.position = new Vector3(spot.x, transform.position.y, spot.y);
                    yield return teleport;

                    animator.Play("Idle");
                    yield return attack;
                    spot = new Vector2((int)player.transform.position.x, (int)player.transform.position.z);
                    if ((int)spot.x == (int)transform.position.x && (int)spot.y == (int)transform.position.z)
                    {
                        animator.Play("Attack");
                        yield return kill;

                        spot = new Vector2(player.transform.position.x, player.transform.position.z);
                        if ((int)spot.x == (int)transform.position.x && (int)spot.y == (int)transform.position.z && !player.killed)
                        {
                            player.killed = true;

                            yield return kill;
                            animator.Play("Idle");

                            yield return death;
                            GalleryManager.UnlockData(4, 0);
                            InGameUIManager.GameOver(deathSprite, deathText);
                            ToolTipManager.Show(3);

                            StopCoroutine(enumerator);
                            enabled = false;
                        }
                        else
                        {
                            yield return kill;
                            animator.Play("Idle");
                        }
                    }
                }
                else
                {
                    if (transform.position.x > spot.x)
                        spriteRenderer.flipX = true;
                    else if (transform.position.x < spot.x)
                        spriteRenderer.flipX = false;

                    movement.Move((int)spot.x, (int)spot.y);
                }
            }
        }
    }

    public void Play()
    {
        StartCoroutine(enumerator);
    }
}
