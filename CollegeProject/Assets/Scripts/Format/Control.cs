using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ѱ� ����Ÿ��
[CreateAssetMenu(fileName = "Control(Name)", menuName = "menu/One Control Type", order = 1)]
public class Control : ScriptableObject
{
    // ����Ÿ�� �̹���
    public Sprite image;

    // ����Ÿ�� �̸�
    public string _name;
}
