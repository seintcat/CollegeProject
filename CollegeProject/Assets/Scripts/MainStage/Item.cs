using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ���� �߻� Ŭ����
public abstract class Item : MonoBehaviour
{
    // ������ �⺻ ������
    public ItemData data;

    // ������ ����
    public int count;

    // ���� �Ͻ�����
    bool _pause;
    public virtual bool pause
    {
        set { _pause = value; }
    }

    // InstanceList������ ������ �ε���
    int _itemCode;
    public int itemCode
    {
        get { return _itemCode; }
    }

    // �ν��Ͻ��μ� ������ Map.items�� �ε���
    int _index;
    public int index
    {
        get { return _index; }
    }

    // �ʱ�ȭ �޼���
    public virtual Item Init(int x, int z, int code, int _count, bool __pause, int index)
    {
        _itemCode = code;
        count = _count;
        pause = __pause;
        _index = index;
        transform.position = new Vector3(x, LayerSettings.GetHeight(ObjectType.ItemObject), z);

        return this;
    }

    // ���Ͱ� �ش� �������� ����� �� ����
    public abstract void OnEvent(Actor actor);

    // ���Ͱ� �ش� �������� �ֿ��� �� ����
    public virtual void PickupEvent(Actor actor)
    {
        transform.SetParent(actor.transform);
        gameObject.SetActive(false);
    }

    // ���Ͱ� �ش� �������� ��� Ȥ�� �����Ͽ��� �� ����
    public abstract void UseEvent(Actor actor);

    // ���Ͱ� �ش� ������ ������ �����Ͽ��� �� ����
    public abstract void UnEquipEvent(Actor actor);

    //���Ͱ� �ش� �������� ���� �� ����
    public virtual void DropEvent(Actor actor)
    {
        gameObject.SetActive(true);
        transform.SetParent(null);
        transform.position = new Vector3((int)actor.transform.position.x, LayerSettings.GetHeight(ObjectType.ItemObject), (int)actor.transform.position.z);
    }

    // x, z��ǥ���� �� ������Ʈ�� �����ϴ��� ����
    public virtual bool IsHere(int x, int z)
    {
        return (x == (int)transform.position.x) && (z == (int)transform.position.z);
    }

    // �ڱ��ڽ��� �����ϴ� �޼���
    public virtual void Remove()
    {
        Map.RemoveItem(index);
    }
}
