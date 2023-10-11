using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템을 위한 추상 클래스
public abstract class Item : MonoBehaviour
{
    // 아이템 기본 데이터
    public ItemData data;

    // 아이템 수량
    public int count;

    // 게임 일시정지
    bool _pause;
    public virtual bool pause
    {
        set { _pause = value; }
    }

    // InstanceList에서의 아이템 인덱스
    int _itemCode;
    public int itemCode
    {
        get { return _itemCode; }
    }

    // 인스턴스로서 생성된 Map.items의 인덱스
    int _index;
    public int index
    {
        get { return _index; }
    }

    // 초기화 메서드
    public virtual Item Init(int x, int z, int code, int _count, bool __pause, int index)
    {
        _itemCode = code;
        count = _count;
        pause = __pause;
        _index = index;
        transform.position = new Vector3(x, LayerSettings.GetHeight(ObjectType.ItemObject), z);

        return this;
    }

    // 액터가 해당 아이템을 밟았을 때 반응
    public abstract void OnEvent(Actor actor);

    // 액터가 해당 아이템을 주웠을 때 반응
    public virtual void PickupEvent(Actor actor)
    {
        transform.SetParent(actor.transform);
        gameObject.SetActive(false);
    }

    // 액터가 해당 아이템을 사용 혹은 장착하였을 때 반응
    public abstract void UseEvent(Actor actor);

    // 액터가 해당 아이템 장착을 해제하였을 때 반응
    public abstract void UnEquipEvent(Actor actor);

    //액터가 해당 아이템을 버릴 때 반응
    public virtual void DropEvent(Actor actor)
    {
        gameObject.SetActive(true);
        transform.SetParent(null);
        transform.position = new Vector3((int)actor.transform.position.x, LayerSettings.GetHeight(ObjectType.ItemObject), (int)actor.transform.position.z);
    }

    // x, z좌표에서 이 오브젝트가 존재하는지 여부
    public virtual bool IsHere(int x, int z)
    {
        return (x == (int)transform.position.x) && (z == (int)transform.position.z);
    }

    // 자기자신을 삭제하는 메서드
    public virtual void Remove()
    {
        Map.RemoveItem(index);
    }
}
