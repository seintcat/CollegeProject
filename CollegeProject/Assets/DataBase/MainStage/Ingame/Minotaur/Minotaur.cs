using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : Actor
{
    public Animator animator;
    Knight player;
    public SpriteRenderer spriteRenderer;
    WaitForSeconds walk, charge, lockDown;
    public float walkTime, chargeTime;
    IEnumerator attack, damage;

    [SerializeField]
    Sprite deathSprite;
    [SerializeField]
    ContextList deathText;
    [SerializeField]
    ItemSpawn itemSpawn;
    [SerializeField]
    AudioPlayer audios;

    // 실제 동작부
    public override IEnumerator Acting()
    {
        movement.pause = false;

        if (player != null)
            while (player.transform.position.x + 5 < transform.position.x)
            {
                yield return actingTime;
                spriteRenderer.flipX = true;
                animator.Play("Attack");
                movement.Move(Direction2D.Left);
                yield return new WaitForSeconds(0.3f);

                animator.Play("Idle");
                yield return walk;
            }

        audios.SetDelay(2);
        audios.PlaySound(17);
        animator.Play("Alert");
        yield return new WaitForSeconds(2f);

        StartCoroutine(attack);
        StopCoroutine(act);
    }

    public IEnumerator Attack()
    {
        int x, z;
        while (true)
        {
            yield return actingTime;

            x = (int)player.transform.position.x - (int)transform.position.x;
            z = (int)player.transform.position.z - (int)transform.position.z;

            movement.speed = data.speeds[1];
            if (x == 0 && z == 0) { }
            else if(x == 0)
            {
                if (z < 0) z = -1;
                else z = 1;

                audios.SetDelay(3);
                audios.PlaySound(19);
                animator.Play("Charge");
                yield return charge;
                audios.Off();

                audios.SetDelay(99);
                audios.PlaySound(18);
                animator.Play("Attack");
                while (Map.IsMovable(false, (int)transform.position.x, (int)transform.position.z + z) && !player.IsHere((int)transform.position.x, (int)player.transform.position.y, (int)transform.position.z + z, false))
                {
                    yield return actingTime;
                    if (!player.IsHere((int)transform.position.x, (int)player.transform.position.y, (int)transform.position.z + z, false))
                        movement.Move((int)transform.position.x, (int)transform.position.z + z);
                }
                audios.Off();

                if (player.killed)
                {
                    StopCoroutine(act);
                    StopCoroutine(attack);
                    enabled = false;
                    Map.RemoveActor(index);
                    yield return actingTime;
                }

                x = (int)player.transform.position.x - (int)transform.position.x;
                z = (int)player.transform.position.z - (int)transform.position.z;
                if((x <= 1 && x >= -1) && (z <= 1 && z >= -1) && !player.killed)
                {
                    player.Damage(1.5f);
                    if (player.killed)
                    {
                        yield return walk;
                        InGameUIManager.GameOver(deathSprite, deathText);
                        GalleryManager.UnlockData(4, 3);
                        ToolTipManager.Show(20);
                        StopCoroutine(act);
                        StopCoroutine(attack);
                        enabled = false;
                        Map.RemoveActor(index);
                        yield return actingTime;
                    }
                }

                animator.Play("Idle");
                yield return walk;
            }
            else if (z == 0)
            {
                if (x < 0)
                {
                    spriteRenderer.flipX = true;
                    x = -1;
                }
                else
                {
                    spriteRenderer.flipX = false;
                    x = 1;
                }

                audios.SetDelay(3);
                audios.PlaySound(19);
                animator.Play("Charge");
                yield return charge;
                audios.Off();

                audios.SetDelay(99);
                audios.PlaySound(18);
                animator.Play("Attack");
                while (Map.IsMovable(false, (int)transform.position.x + x, (int)transform.position.z) && !player.IsHere((int)transform.position.x + x, (int)player.transform.position.y, (int)transform.position.z, false))
                {
                    yield return actingTime;
                    if (!player.IsHere((int)transform.position.x + x, (int)player.transform.position.y, (int)transform.position.z, false))
                        movement.Move((int)transform.position.x + x, (int)transform.position.z);
                }
                audios.Off();

                if (player.killed)
                {
                    StopCoroutine(act);
                    StopCoroutine(attack);
                    enabled = false;
                    Map.RemoveActor(index);
                    yield return actingTime;
                }

                x = (int)player.transform.position.x - (int)transform.position.x;
                z = (int)player.transform.position.z - (int)transform.position.z;
                if ((x <= 1 && x >= -1) && (z <= 1 && z >= -1) && !player.killed)
                {
                    player.Damage(1.5f);
                    if (player.killed)
                    {
                        yield return walk;
                        InGameUIManager.GameOver(deathSprite, deathText);
                        GalleryManager.UnlockData(4, 3);
                        ToolTipManager.Show(20);
                        StopCoroutine(act);
                        StopCoroutine(attack);
                        enabled = false;
                        Map.RemoveActor(index);
                        yield return actingTime;
                    }
                }

                animator.Play("Idle");
                yield return walk;
            }
            else if (z == x || z == -x)
            {
                if (x < 0)
                {
                    spriteRenderer.flipX = true;
                    x = -1;
                }
                else
                {
                    spriteRenderer.flipX = false;
                    x = 1;
                }
                if (z < 0) z = -1;
                else z = 1;

                audios.SetDelay(3);
                audios.PlaySound(19);
                animator.Play("Charge");
                yield return charge;
                audios.Off();

                audios.SetDelay(99);
                audios.PlaySound(18);
                animator.Play("Attack");
                while (Map.IsMovable(false, (int)(transform.position.x + 0.3f) + x, (int)transform.position.z + z) && !player.IsHere((int)transform.position.x + x, (int)player.transform.position.y, (int)transform.position.z + z, false))
                {
                    yield return actingTime;
                    if (!player.IsHere((int)transform.position.x + x, (int)player.transform.position.y, (int)transform.position.z + z, false))
                        movement.Move((int)transform.position.x + x, (int)transform.position.z + z);
                }
                audios.Off();

                if (player.killed)
                {
                    StopCoroutine(act);
                    StopCoroutine(attack);
                    enabled = false;
                    Map.RemoveActor(index);
                    yield return actingTime;
                }

                x = (int)player.transform.position.x - (int)transform.position.x;
                z = (int)player.transform.position.z - (int)transform.position.z;
                if ((x <= 1 && x >= -1) && (z <= 1 && z >= -1) && !player.killed)
                {
                    player.Damage(1.5f);
                    if (player.killed)
                    {
                        yield return walk;
                        InGameUIManager.GameOver(deathSprite, deathText);
                        GalleryManager.UnlockData(4, 3);
                        ToolTipManager.Show(20);
                        StopCoroutine(act);
                        StopCoroutine(attack);
                        enabled = false;
                        Map.RemoveActor(index);
                        yield return actingTime;
                    }
                }

                animator.Play("Idle");
                yield return walk;
            }
            movement.speed = data.speeds[0];

            Moveto(new Vector2((int)player.transform.position.x, (int)player.transform.position.z));
            yield return new WaitForSeconds(0.3f);
            animator.Play("Idle");
            yield return walk;
        }
    }


    public IEnumerator DamageCheck()
    {
        while (true)
        {
            yield return actingTime;


            if (hp <= 0f)
            {
                audios.Off();
                audios.SetDelay(2);
                audios.PlaySound(17);
                movement.enabled = false;
                animator.Play("Death");
                StopCoroutine(act);
                StopCoroutine(attack);

                yield return new WaitForSeconds(2f);
                FilterManager.Play(21);
                itemSpawn.x = (int)transform.position.x;
                itemSpawn.z = (int)transform.position.z;
                Map.SpawnItem(itemSpawn);
                enabled = false;
                Map.RemoveActor(index);
            }

            if (lockdownTime > 0)
            {
                movement.pause = true;
                StopCoroutine(attack);
                lockDown = new WaitForSeconds(lockdownTime);
                lockdownTime = -1f;
                animator.Play("Damage");
                yield return lockDown;
                StartCoroutine(attack);
                movement.pause = false;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override Actor Init(int x, int z, int index, MoveType move = MoveType.None, bool __pause = false)
    {
        var obj = FindObjectsOfType<Knight>();
        if (obj != null)
            player = obj[0];
        walk = new WaitForSeconds(walkTime);
        charge = new WaitForSeconds(chargeTime);

        base.Init(x, z, index, move, __pause);

        damage = DamageCheck();
        attack = Attack();
        StartCoroutine(damage);

        return this;
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
                if (!player.IsHere((int)transform.position.x + 1, (int)player.transform.position.y, (int)transform.position.z + 1, false))
                    movement.Move(Direction2D.UpRight);
                spriteRenderer.flipX = false;
            }
            else if (tx == x)
            {
                if (!player.IsHere((int)transform.position.x, (int)player.transform.position.y, (int)transform.position.z + 1, false))
                    movement.Move(Direction2D.Up);
            }
            else
            {
                if (!player.IsHere((int)transform.position.x - 1, (int)player.transform.position.y, (int)transform.position.z + 1, false))
                    movement.Move(Direction2D.UpLeft);
                spriteRenderer.flipX = true;
            }
        }
        else if (ty == y)
        {
            if (tx < x)
            {
                if (!player.IsHere((int)transform.position.x + 1, (int)player.transform.position.y, (int)transform.position.z, false))
                    movement.Move(Direction2D.Right);
                spriteRenderer.flipX = false;
            }
            else if (tx == x)
                return true;
            else
            {
                if (!player.IsHere((int)transform.position.x - 1, (int)player.transform.position.y, (int)transform.position.z, false))
                    movement.Move(Direction2D.Left);
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            if (tx < x)
            {
                if (!player.IsHere((int)transform.position.x + 1, (int)player.transform.position.y, (int)transform.position.z - 1, false))
                    movement.Move(Direction2D.DownRight);
                spriteRenderer.flipX = false;
            }
            else if (tx == x)
            {
                if (!player.IsHere((int)transform.position.x, (int)player.transform.position.y, (int)transform.position.z - 1, false))
                    movement.Move(Direction2D.Down);
            }
            else
                {
                    if (!player.IsHere((int)transform.position.x - 1, (int)player.transform.position.y, (int)transform.position.z - 1, false))
                        movement.Move(Direction2D.DownLeft);
                spriteRenderer.flipX = true;
            }
        }
        animator.Play("Attack");
        return false;
    }

    public override void Damage(float damage, float lockdownTime = 0)
    {
        hp -= damage;

        if (lockdownTime > 0) Lockdown(lockdownTime);
    }

    public void Resume()
    {
        StartCoroutine(damage);
        StartCoroutine(attack);
    }
}
