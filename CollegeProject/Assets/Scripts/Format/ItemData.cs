using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ϳ��� �����ۿ� ���� ������
[CreateAssetMenu(fileName = "Name(Data)", menuName = "menu/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    //��ȣ�ۿ��� ��������
    public bool isUsable;

    //������ ��������
    public bool isEquippable;

    //���Ϳ��� �ش� �������� ������ ��������
    public bool controlDisable;

    //�������� ���� �� �ִ� �ִ� ����
    public int maxCount;
}
