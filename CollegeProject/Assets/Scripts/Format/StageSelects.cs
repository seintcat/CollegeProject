using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ���� ������ ����Ʈ
[CreateAssetMenu(fileName = "Stage Select Datas", menuName = "menu/All Stage Select Data", order = 1)]
public class StageSelects : ScriptableObject
{
    [SerializeField]
    List<StageSelectData> _list;
    public List<StageSelectData> list
    {
        get { return _list; }
    }
}
