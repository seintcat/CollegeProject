using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ϳ��� ȿ�� ������
[System.Serializable]
public class EffectData
{
    // �����ð�, (< 0)�̸� ����ȵ�
    public float lockdown;

    // ���ط�, (< 0)�̸� ȸ����
    public float damage;

    // �̵��ӵ� ���� ��, �⺻�� ����
    public float speedValue;

    // �̵��ӵ��� ���ϴ��� ����(����� ������ �ȵ�)
    public bool isSpeedMult;

    // ȿ�� ����Ʈ�� �����ִ� ������
    public GameObject visual;

    // ȿ�� ������
    // ===========================================
    // effectCount(==0)
    // - effectCount �������, effectTime = ȿ�� ���� �ð�
    // - effectTime��(<0)�� ȿ�� ��������
    // 
    // effectCount(>0)
    // - effectCount = ȿ�� ���� Ƚ��, effectTime = �ߵ� ��Ÿ��
    // 
    // effectCount(<0)
    // - effectCount = ���밪��ŭ ���� ���� �ߵ�, effectTime = ���� �Ҹ�ð�
    // - effectTime��(<0)�� ���� �Ҹ����� ����
    // ===========================================
    public float effectTime;
    public int effectCount;

    // ���� ȿ�� ��ø ���� ����
    public bool isAddible;
}
