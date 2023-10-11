using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Actor
{
    Knight player;
    [SerializeField]
    Animator animator;
    [SerializeField]
    AudioPlayer audios;
    Vector2 start;

    [SerializeField]
    Sprite deathSprite;
    [SerializeField]
    ContextList deathText;

    // 실제 동작부
    public override IEnumerator Acting()
    {
        Vector2 playerPos;
        bool haveBone = false;
        while (true)
        {
            yield return actingTime;
            if(player != null)
            {
                playerPos = new Vector2((int)player.transform.position.x, (int)player.transform.position.z);
                if (playerPos.x < 32 && playerPos.x > 20 && playerPos.y < 23 && playerPos.y > 15)
                {
                    GalleryManager.UnlockData(3, 2);
                    while (true)
                    {
                        playerPos = new Vector2((int)player.transform.position.x, (int)player.transform.position.z);

                        if (Moveto(playerPos))
                            break;

                        yield return actingTime;
                    }

                    foreach (Item item in player.itemList)
                        if (item.itemCode == 1)
                        {
                            haveBone = true;
                            player.DropItem(item, true);
                            break;
                        }

                    animator.Play("Howl");
                    audios.SetDelay(3);
                    audios.PlaySound(15);
                    if (haveBone)
                    {
                        yield return new WaitForSeconds(2.5f);
                        while (true)
                        {
                            if (Moveto(start))
                                break;

                            yield return actingTime;
                        }

                        animator.Play("Sleep");
                        StopCoroutine(act);
                        enabled = false;
                    }
                    else if (!player.killed)
                    {
                        player.killed = true;
                        yield return new WaitForSeconds(2.5f);
                        animator.Play("Idle");

                        yield return new WaitForSeconds(1f);

                        GalleryManager.UnlockData(4, 2);
                        InGameUIManager.GameOver(deathSprite, deathText);
                        ToolTipManager.Show(19);

                        StopCoroutine(act);
                        enabled = false;
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];
        start = new Vector2((int)transform.position.x, (int)transform.position.z);
        movement.pause = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SideEffect(EffectType type, int count, bool isRemoving)
    {

    }

    public override void StatusEffect(EffectType type)
    {

    }

    bool Moveto(Vector2 vector)
    {
        int x = (int)vector.x, y = (int)vector.y, tx = (int)transform.position.x, ty = (int)transform.position.z;

        if (ty < y)
        {
            if (tx < x)
            {
                movement.Move(Direction2D.UpRight);
                animator.Play("Up");
            }
            else if (tx == x)
            {
                movement.Move(Direction2D.Up);
                animator.Play("Up");
            }
            else
            {
                movement.Move(Direction2D.UpLeft);
                animator.Play("Up");
            }
        }
        else if (ty == y)
        {
            if (tx < x)
            {
                movement.Move(Direction2D.Right);
                animator.Play("Right");
            }
            else if (tx == x)
                return true;
            else
            {
                movement.Move(Direction2D.Left);
                animator.Play("Left");
            }
        }
        else
        {
            if (tx < x)
            {
                movement.Move(Direction2D.DownRight);
                animator.Play("Right");
            }
            else if (tx == x)
            {
                movement.Move(Direction2D.Down);
                animator.Play("Down");
            }
            else
            {
                movement.Move(Direction2D.DownLeft);
                animator.Play("Left");
            }
        }
        return false;
    }
}
