using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이동 인공지능, 애니메이션을 위한 추상 클래스
public abstract class Actor : MonoBehaviour
{
    // 액터 코루틴 동작주기
    [HideInInspector]
    public WaitForSeconds actingTime;

    // 상태이상 계산 코루틴 동작주기
    //readonly WaitForSeconds calculateEffectsTime = new WaitForSeconds(ActorSettings.effectTime);

    // 이동 컴포넌트
    public Movement movement;

    // 게임 일시정지 등에 사용됨
    // 일시정지는 무력화와는 다르며, 어떠한 동작도 하지 않음
    protected bool _pause = true;
    public virtual bool pause
    {
        set
        {
            movement.pause = value;
            _pause = value;
        }
    }

    // 체력
    protected float hp;

    // 무적 시간, 피격판정 적용안됨
    float invincibleTime;

    // 관심을 갖는 대상
    // 원거리 공격, 혹은 추격 대상 등
    public GameObject target;

    // 경직시간
    public float lockdownTime;

    // 액터 기본 데이터
    public ActorData data;

    // 부여된 효과 관련 데이터
    Dictionary<EffectType, float> effectsTime = new Dictionary<EffectType, float>();
    Dictionary<EffectType, int> effectsCount = new Dictionary<EffectType, int>();

    // 액터가 가진 아이템 리스트
    public List<Item> itemList;

    // 액터가 장비한 아이템 리스트
    public List<Item> equipList;

    // 액션 실행 코루틴
    [HideInInspector]
    public IEnumerator act;

    // 인스턴스로서 생성된 Map.actors의 인덱스
    int _index;
    public int index
    {
        get { return _index; }
    }

    // 액터가 실제로 행동하는 부분에 대한 코루틴
    // - 데미지 계산, 경직 등
    public abstract IEnumerator Acting();

    // 각종 상태이상 계산
    public void CalculateEffects()
    {
        // IEnumerator 코루틴 변환시 필요
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

    // _Init을 호출하는 초기화 
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

    // 데미지 계산, 경직시간 체크
    // 절대값으로 적용되므로 속성 등 가변적 적용에는 메서드를 재정의하거나 이펙트 부분을 우회하여 사용하여야 함
    // 이펙트 부분 = SideEffect, StatusEffect
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

    // 경직처리, 경직시간 중첩여부, 슈퍼아머 상관없이 강제적용 여부
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

    // 이펙트 설정
    public virtual void SetEffect(EffectType type)
    {
        int typeInt = EffectList.GetTypeIndex(type);

        EffectData effect = Map.effectList[typeInt];
        float time = effect.effectTime;
        int count = effect.effectCount;
        if (count == 0)
        {
            // effectsCount 적용없음, effectTime = 효과 지속 시간
            // effectsTime도(<0)시 효과 무한지속
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
            // effectsCount = 효과 적용 횟수, effectTime = 발동 쿨타임
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
            // effectsCount = 절대값만큼 스택 이후 발동, effectsTime = 스택 소멸시간
            // effectsTime도(<0)시 스택 소멸하지 않음
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

    // 이펙트 제거, 스택 포함 제거 여부
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
            // effectsCount 적용없음, effectTime = 효과 지속 시간
            // effectsTime도(<0)시 효과 무한지속
            effectsTime[type] -= time;
            if (effectsTime[type] < 0)
            {
                effectsTime.Remove(type);
                return;
            }
        }
        else if (count > 0)
        {
            // effectsCount = 효과 적용 횟수, effectTime = 발동 쿨타임
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
            // effectsCount = 절대값만큼 스택 이후 발동, effectsTime = 스택 소멸시간
            // effectsTime도(<0)시 스택 소멸하지 않음
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

    // 무브먼트의 같은 메서드, passableCheck따라 현재 액터의 투과성 체크
    public virtual bool IsHere(int x, int y, int z, bool passableCheck)
    {
        if (passableCheck || data.isPassable)
            return false;

        return movement.IsHere(x, y, z);
    }

    // 사망 처리 혹은 라이프 깎임 등
    public virtual void Death()
    {
        Remove();
    }

    // 상태이상 효과 발동
    public abstract void StatusEffect(EffectType type);

    // 상태이상 카운트, 스택 증감시 효과
    public abstract void SideEffect(EffectType type, int count, bool isRemoving);

    // 액터가 해당 아이템 위에 위치해 있음
    public virtual void OnItem(Item item)
    {
        item.OnEvent(this);
    }

    // 액터가 해당 아이템을 획득
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

    // 액터가 해당 아이템을 사용 혹은 장착
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

    // 액터가 해당 아이템 장착을 해제
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

    // 액터가 해당 아이템을 버림
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

    // 자기자신을 삭제하는 메서드
    public virtual void Remove()
    {
        Map.RemoveActor(index);
    }
}
