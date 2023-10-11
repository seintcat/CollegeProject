using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ÿ�� ����
[CreateAssetMenu(fileName = "Control Type", menuName = "menu/Control Type", order = 1)]
public class ControlType : ScriptableObject
{
    // ����Ÿ�� ���
    [SerializeField]
    List<Control> _list;
    public List<Control> list
    {
        get { return _list; }
    }
}
