using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Actor
{
    bool down = false;
    public float attackTime, moveTime;
    WaitForSeconds attack, moveDelay;
    Knight player;

    [SerializeField]
    Sprite deathSprite;
    [SerializeField]
    ContextList deathText;
    [SerializeField]
    AudioPlayer audios;

    // 실제 동작부
    public override IEnumerator Acting()
    {
        Vector2 move = new Vector2(), look = new Vector2();
        move.x = (int)transform.position.x;
        look.x = (int)transform.position.x;
        while (true)
        {
            yield return actingTime;
            if (down)
            {
                move.y = (int)transform.position.z - 1;
                look.y = (int)transform.position.z - 1;
            }
            else
            {
                move.y = (int)transform.position.z + 1;
                look.y = (int)transform.position.z + 2;
            }

            List<Actor> actors = Map.FindActors((int)move.x, (int)transform.position.y, (int)move.y, false);
            if (actors == null || actors.Count < 1)
            {
                movement.Move((int)move.x, (int)move.y);
                yield return moveDelay;
            }
            else if (actors[0].GetType() == typeof(Knight))
            {
                yield return attack;
                audios.SetDelay(1);
                audios.PlaySound(24);

                if ((int)player.transform.position.x == (int)move.x && (int)player.transform.position.z == (int)move.y && !player.killed)
                {
                    player.Damage(0.5f, 0.5f);

                    if (player.killed)
                    {
                        yield return new WaitForSeconds(1f);

                        GalleryManager.UnlockData(4, 1);
                        InGameUIManager.GameOver(deathSprite, deathText);
                        ToolTipManager.Show(18);

                        StopCoroutine(act);
                        enabled = false;
                    }
                }
            }

            if (!Map.IsMovable(false, (int)look.x, (int)look.y))
                down = !down;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override Actor Init(int x, int z, int index, MoveType move = MoveType.None, bool __pause = false)
    {
        base.Init(x, z, index, move, __pause);
        movement.pause = false;
        attack = new WaitForSeconds(attackTime);
        moveDelay = new WaitForSeconds(moveTime);
        return this;
    }

    public override void SideEffect(EffectType type, int count, bool isRemoving)
    {

    }

    public override void StatusEffect(EffectType type)
    {

    }

}
