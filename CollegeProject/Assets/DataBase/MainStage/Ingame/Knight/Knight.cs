using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Actor
{
    [HideInInspector]
    public Vector2 teleport;
    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool killed = false;

    Camera2D cam;

    public SpriteRenderer spriteRenderer;
    public GameObject uiObj;
    KnightUI ui;

    public Animator animator;

    WaitForSeconds damage;

    bool down = false;

    // 실제 동작부
    public override IEnumerator Acting()
    {
        bool checkBone = false, checkPotion = false, checkBall = false;

        while (true)
        {
            yield return actingTime;

            List<Actor> actors = Map.FindActors((int)target.transform.position.x, (int)transform.position.y, (int)target.transform.position.z, false);
            if ((actors == null || actors.Count < 1) && !_pause)
                movement.Move((int)target.transform.position.x, (int)target.transform.position.z);

            if (target.transform.position.x > transform.position.x)
                spriteRenderer.flipX = false;
            else if(target.transform.position.x < transform.position.x)
                spriteRenderer.flipX = true;

            if (teleport.x > 0f && teleport.y > 0f)
            {
                movement.Move((int)transform.position.x, (int)transform.position.z);
                movement.pause = true;
                transform.position = new Vector3(teleport.x, transform.position.y, teleport.y);
                teleport = new Vector2(-1f, -1f);
                EssentialManager.Loading(2f);
                movement.pause = false;
                yield return new WaitForSeconds(2);
            }

            checkBone = false;
            checkPotion = false;
            checkBall = false;
            foreach (Item item in itemList)
            {
                if (item.itemCode == 0)
                {
                    checkPotion = true;
                    ui.potion = item.count;
                }
                else if (item.itemCode == 1)
                    checkBone = true;
                else if (item.itemCode == 2)
                    checkBall = true;
                else if (item.itemCode == 3)
                    ui.minotaur = item.count;
            }
            ui.bone = checkBone;
            ui.cannonBall = checkBall;
            if (!checkPotion)
                ui.potion = 0;

            if (killed)
            {
                canMove = false;
                movement.enabled = false;
                target.GetComponent<PlayerTarget>().enabled = false;
                Destroy(ui.gameObject);
                animator.Play("Down");
                StopCoroutine(act);
                enabled = false;
            }

            if(lockdownTime > 0)
            {
                movement.pause = true;
                target.SetActive(false);
                damage = new WaitForSeconds(lockdownTime);
                lockdownTime = -1f;
                animator.Play("Damage");
                yield return damage;
                movement.pause = false;
                target.SetActive(true);
            }

            if (down)
            {
                movement.pause = true;
                target.SetActive(false);
                animator.Play("Down");
                yield return new WaitForSeconds(0.7f);
                animator.Play("Backup");
                yield return new WaitForSeconds(0.7f);
                movement.pause = false;
                target.SetActive(true);
                down = false;
            }
        }
    }

    public override Actor Init(int x, int z, int index, MoveType move = MoveType.None, bool __pause = false)
    {
        base.Init(x, z, index, move, __pause);
        actingTime = new WaitForSeconds(data.waitTime);
        movement.pause = false;
        cam = FindObjectsOfType<Camera2D>()[0]; 
        cam.SetTarget(transform);
        GalleryManager.UnlockData(2, 0);
        GalleryManager.UnlockData(6, 0);
        ToolTipManager.Show(11);
        GameObject obj = Instantiate(uiObj);
        ui = obj.GetComponent<KnightUI>();
        ui.maxHp = data.maxHp;
        ui.hp = data.maxHp;
        hp = data.maxHp;
        _pause = false;
        return this;
    }

    public override void SideEffect(EffectType type, int count, bool isRemoving)
    {

    }

    public override void StatusEffect(EffectType type)
    {

    }

    public override void Damage(float damage, float lockdownTime = 0)
    {
        if (down) return;

        hp -= damage;
        ui.hp = hp;
        if (hp <= 0 && !data.immortal)
        {
            killed = true;
            return;
        }

        if (lockdownTime > 0) Lockdown(lockdownTime);

        if (damage > 0.8f)
            down = true;
    }

    private void Start()
    {
        FilterManager.Play(7);
    }

    public void Heal(float value)
    {
        hp += value;
        if (hp > data.maxHp)
            hp = data.maxHp;

        ui.hp = hp;
    }
}
