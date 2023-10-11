using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �̵� �ΰ�����, �ִϸ��̼��� ���� �߻� Ŭ����
public abstract class Actor : MonoBehaviour
{
    // ���� �ڷ�ƾ �����ֱ�
    [HideInInspector]
    public WaitForSeconds actingTime;

    // �����̻� ��� �ڷ�ƾ �����ֱ�
    //readonly WaitForSeconds calculateEffectsTime = new WaitForSeconds(ActorSettings.effectTime);

    // �̵� ������Ʈ
    public Movement movement;

    // ���� �Ͻ����� � ����
    // �Ͻ������� ����ȭ�ʹ� �ٸ���, ��� ���۵� ���� ����
    protected bool _pause = true;
    public virtual bool pause
    {
        set
        {
            movement.pause = value;
            _pause = value;
        }
    }

    // ü��
    protected float hp;

    // ���� �ð�, �ǰ����� ����ȵ�
    float invincibleTime;

    // ������ ���� ���
    // ���Ÿ� ����, Ȥ�� �߰� ��� ��
    public GameObject target;

    // �����ð�
    public float lockdownTime;

    // ���� �⺻ ������
    public ActorData data;

    // �ο��� ȿ�� ���� ������
    Dictionary<EffectType, float> effectsTime = new Dictionary<EffectType, float>();
    Dictionary<EffectType, int> effectsCount = new Dictionary<EffectType, int>();

    // ���Ͱ� ���� ������ ����Ʈ
    public List<Item> itemList;

    // ���Ͱ� ����� ������ ����Ʈ
    public List<Item> equipList;

    // �׼� ���� �ڷ�ƾ
    [HideInInspector]
    public IEnumerator act;

    // �ν��Ͻ��μ� ������ Map.actors�� �ε���
    int _index;
    public int index
    {
        get { return _index; }
    }

    // ���Ͱ� ������ �ൿ�ϴ� �κп� ���� �ڷ�ƾ
    // - ������ ���, ���� ��
    public abstract IEnumerator Acting();

    // ���� �����̻� ���
    public void CalculateEffects()
    {
        // IEnumerator �ڷ�ƾ ��ȯ�� �ʿ�
        //while (true)
        //{
        //    yield return calculateEffectsTime;
        //}

        if (_pause)
            return;

        foreach (EffectType type in effectsTime.Keys)
        {
            if (!effectsCount.ContainsKey(type))
                StatusEffect(type);

            if (effectsTime[type] < 0)
                continue;

            effectsTime[type] -= Time.deltaTime;

            if (effectsTime[type] < 0)
            {
                if (effectsCount.ContainsKey(type))
                {
                    if (effectsCount[type] < 0)
                    {
                        effectsCount[type]++;
                        if (effectsCount[type] >= 0)
                        {
                            RemoveEffect(type, true);
                            continue;
                        }
                    }
                    else
                    {
                        effectsCount[type]--;
                        if (effectsCount[type] <= 0)
                        {
                            RemoveEffect(type, true);
                            continue;
                        }
                    }
                    SideEffect(type, effectsCount[type], true);
                }
                else
                    RemoveEffect(type, true);
            }
        }
    }

    // _Init�� ȣ���ϴ� �ʱ�ȭ 
    public virtual Actor Init(int x, int z, int index, MoveType move = MoveType.None, bool __pause = false)
    {
        if (move == MoveType.None)
            movement.Init(x, z, data.moveTypes[0], data.speeds[0]);
        else
        {
            int moveTypeIndex = data.moveTypes.IndexOf(move);
            movement.Init(x, z, data.moveTypes[moveTypeIndex], data.speeds[moveTypeIndex]);
        }

        _index = index;
        itemList = new List<Item>();
        equipList = new List<Item>();
        hp = data.maxHp;

        pause = __pause;
        act = Acting();
        StartCoroutine(act);

        return this;
    }

    // ������ ���, �����ð� üũ
    // ���밪���� ����ǹǷ� �Ӽ� �� ������ ���뿡�� �޼��带 �������ϰų� ����Ʈ �κ��� ��ȸ�Ͽ� ����Ͽ��� ��
    // ����Ʈ �κ� = SideEffect, StatusEffect
    public virtual void Damage(float damage, float lockdownTime = 0f)
    {
        hp -= damage;
        if (hp < 0 && !data.immortal)
        {
            Death();
            return;
        }

        if (lockdownTime > 0) Lockdown(lockdownTime);
    }

    // ����ó��, �����ð� ��ø����, ���۾Ƹ� ������� �������� ����
    public virtual void Lockdown(float time, bool timeAddible = false, bool forceLockdown = false)
    {
        if (data.superArmor && !forceLockdown) 
            return;

        if (time < 0)
        {
            Debug.Log("Wrong lockdown time");
            return;
        }

        if (timeAddible) lockdownTime += time;
        else if (time > lockdownTime) lockdownTime = time;
    }

    // ����Ʈ ����
    public virtual void SetEffect(EffectType type)
    {
        int typeInt = EffectList.GetTypeIndex(type);

        EffectData effect = Map.effectList[typeInt];
        float time = effect.effectTime;
        int count = effect.effectCount;
        if (count == 0)
        {
            // effectsCount �������, effectTime = ȿ�� ���� �ð�
            // effectsTime��(<0)�� ȿ�� ��������
            if (effectsTime.ContainsKey(type))
            {
                if (effect.isAddible)
                    effectsTime[type] += time;
                else
                    effectsTime[type] = time;
            }
            else 
                effectsTime.Add(type, time);
        }
        else if(count > 0)
        {
            // effectsCount = ȿ�� ���� Ƚ��, effectTime = �ߵ� ��Ÿ��
            if (time < 0)
            {
                Debug.Log("Wrong effect time");
                return;
            }

            if (effectsCount.ContainsKey(type))
            {
                if (effect.isAddible)
                    effectsCount[type] += count;
                else
                    effectsCount[type] = count;
            }
            else
                effectsCount.Add(type, count);

            if (effectsTime.ContainsKey(type))
                effectsTime[type] = time;
            else
                effectsTime.Add(type, time);
        }
        else
        {
            // effectsCount = ���밪��ŭ ���� ���� �ߵ�, effectsTime = ���� �Ҹ�ð�
            // effectsTime��(<0)�� ���� �Ҹ����� ����
            if (effectsCount.ContainsKey(type))
            {
                effectsCount[type]++;
                if (effectsCount[type] >= 0)
                    StatusEffect(type);
            }
            else
                effectsCount.Add(type, count);

            if (effectsTime.ContainsKey(type))
            {
                if (effect.isAddible)
                    effectsTime[type] += time;
                else
                    effectsTime[type] = time;
            }
            else
                effectsTime.Add(type, time);
        }
    }

    // ����Ʈ ����, ���� ���� ���� ����
    public virtual void RemoveEffect(EffectType type, bool removeAll = false)
    {
        int typeInt = EffectList.GetTypeIndex(type);
        EffectData effect = Map.effectList[typeInt];

        if (removeAll)
        {
            if (effectsTime.ContainsKey(type)) 
                effectsTime.Remove(type);
            if (effectsCount.ContainsKey(type)) 
                effectsCount.Remove(type);
            return;
        }

        float time = effect.effectTime;
        int count = effect.effectCount;
        if (count == 0)
        {
            // effectsCount �������, effectTime = ȿ�� ���� �ð�
            // effectsTime��(<0)�� ȿ�� ��������
            effectsTime[type] -= time;
            if (effectsTime[type] < 0)
            {
                effectsTime.Remove(type);
                return;
            }
        }
        else if (count > 0)
        {
            // effectsCount = ȿ�� ���� Ƚ��, effectTime = �ߵ� ��Ÿ��
            effectsCount[type] -= count;
            if(effectsCount[type] <= 0)
            {
                SideEffect(type, count, true);
                RemoveEffect(type, true);
                return;
            }

            effectsTime[type] = time;
        }
        else
        {
            // effectsCount = ���밪��ŭ ���� ���� �ߵ�, effectsTime = ���� �Ҹ�ð�
            // effectsTime��(<0)�� ���� �Ҹ����� ����
            effectsCount[type] --;
            if (effectsCount[type] < count)
            {
                SideEffect(type, count, true);
                RemoveEffect(type, true);
                return;
            }

            effectsTime[type] = time;
        }
    }

    // �����Ʈ�� ���� �޼���, passableCheck���� ���� ������ ������ üũ
    public virtual bool IsHere(int x, int y, int z, bool passableCheck)
    {
        if (passableCheck || data.isPassable)
            return false;

        return movement.IsHere(x, y, z);
    }

    // ��� ó�� Ȥ�� ������ ���� ��
    public virtual void Death()
    {
        Remove();
    }

    // �����̻� ȿ�� �ߵ�
    public abstract void StatusEffect(EffectType type);

    // �����̻� ī��Ʈ, ���� ������ ȿ��
    public abstract void SideEffect(EffectType type, int count, bool isRemoving);

    // ���Ͱ� �ش� ������ ���� ��ġ�� ����
    public virtual void OnItem(Item item)
    {
        item.OnEvent(this);
    }

    // ���Ͱ� �ش� �������� ȹ��
    public virtual void PickupItem(Item item)
    {
        if (item.data.controlDisable) 
            return;

        bool pickuped = false;

        foreach (Item _item in itemList)
            if (item.itemCode == _item.itemCode)
            {
                if ((_item.count + item.count) <= item.data.maxCount)
                {
                    _item.count += item.count;
                    if (!pickuped)
                    {
                        pickuped = true;
                        _item.PickupEvent(this);
                    }
                    item.Remove();
                    return;
                }

                int value = item.data.maxCount - _item.count;
                _item.count = item.data.maxCount;
                item.count -= value;
                if (!pickuped)
                {
                    pickuped = true;
                    item.PickupEvent(this);
                }
            }

        if (itemList.Count < data.maxInventory)
        {
            itemList.Add(item);
            if (!pickuped)
            {
                pickuped = true;
                item.PickupEvent(this);
            }
        }
    }

    // ���Ͱ� �ش� �������� ��� Ȥ�� ����
    public virtual void UseItem(Item item)
    {
        if (item.data.controlDisable)
            return;

        int index = itemList.IndexOf(item);
        Item tmp = itemList[index];

        tmp.count--;
        if (item.data.isEquippable)
        {
            foreach (Item _item in equipList)
                if (tmp.itemCode == _item.itemCode)
                {
                    tmp.count++;
                    return;
                }

            if (tmp.count <= 0)
            {
                tmp.count = 1;
                tmp.UseEvent(this);
                itemList.RemoveAt(index);
                equipList.Add(item);

                return;
            }

            equipList.Add(Map.SpawnItem(item.itemCode, (int)transform.position.x, (int)transform.position.z, item.count));
            equipList[equipList.Count - 1].UseEvent(this);
        }
        else if (tmp.count <= 0)
        {
            tmp.UseEvent(this);
            itemList.RemoveAt(index);
            tmp.Remove();

            return;
        }
        else
            tmp.UseEvent(this);

        for(int i = 0; i < itemList.Count; i++)
        {
            if (index == i) 
                continue;

            if (tmp.itemCode == itemList[i].itemCode)
            {
                if ((itemList[i].count + tmp.count) <= tmp.data.maxCount)
                {
                    itemList[i].count += tmp.count;
                    tmp.Remove();
                    return;
                }

                int value = tmp.data.maxCount - itemList[i].count;
                itemList[i].count = tmp.data.maxCount;
                tmp.count -= value;
            }
        }
    }

    // ���Ͱ� �ش� ������ ������ ����
    public virtual void UnEquipItem(Item item)
    {
        if (item.data.controlDisable)
            return;

        int index = equipList.IndexOf(item);
        Item tmp = equipList[index];

        foreach (Item _item in itemList)
            if (tmp.itemCode == _item.itemCode && (_item.count + 1) <= item.data.maxCount)
            {
                _item.count++;
                _item.UnEquipEvent(this);
                tmp.Remove();
                return;
            }

        if(itemList.Count < data.maxInventory)
            itemList.Add(tmp);
        else
            tmp.DropEvent(this);

        equipList.RemoveAt(index);
    }

    // ���Ͱ� �ش� �������� ����
    public virtual void DropItem(Item item, bool dropAll)
    {
        if (item.data.controlDisable)
            return;

        int index = itemList.IndexOf(item);
        if (dropAll || itemList[index].count == 1)
        {
            itemList.RemoveAt(index);
            item.DropEvent(this);
        }
        else
        {
            itemList[index].count--;
            Item _item = Map.SpawnItem(item.itemCode, (int)transform.position.x, (int)transform.position.z, item.count);
            _item.DropEvent(this);
        }
    }

    // �ڱ��ڽ��� �����ϴ� �޼���
    public virtual void Remove()
    {
        Map.RemoveActor(index);
    }
}
