using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ϳ��� ���� ������
[CreateAssetMenu(fileName = "Name(Data)", menuName = "menu/Actor Data", order = 1)]
public class ActorData : ScriptableObject
{
    // �ִ�ü��
    public float maxHp;

    // �⺻ ���� ����, �ǰ����� ����
    public bool immortal;

    // ���۾Ƹ� ����
    public bool superArmor;

    // �ڷ�ƾ �����ֱ�
    public float waitTime;

    // �⺻ �̵� �ӵ�
    public List<float> speeds;

    // ��� ������ MoveType ����Ʈ
    public List<MoveType> moveTypes;

    // ���� MoveType���� ����� ��������
    public bool isPassable;

    // ȿ���� ������� �ʴ� �����̻�
    public List<EffectType> immune;

    // �κ��丮 ĭ ��
    public int maxInventory;
}
