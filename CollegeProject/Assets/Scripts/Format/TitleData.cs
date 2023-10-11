using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ȭ���� �����ϱ� ���� ������
[CreateAssetMenu(fileName = "Title(Name)", menuName = "menu/One Title preset data", order = 1)]
public class TitleData : ScriptableObject
{
    //�� ȭ���� ǥ���� �ε���
    public int titleIndex, optionIndex, galleryIndex;

    //ȭ�� �������� �̸�
    public string presetName;

    //���ȭ��, �����
    public int backgroundIndex;

    //���� �Ͻ�����ȭ��
    public int pauseIndex;

    //���� Ŭ���� UI
    public int clearIndex;

    //���� ���� UI
    public int overIndex;
}
