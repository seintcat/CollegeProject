using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ ����Ʈ
[CreateAssetMenu(fileName = "ToolTip Data List", menuName = "menu/ToolTip Data List", order = 1)]
public class ToolTipDataList : ScriptableObject
{
    public List<ToolTipData> list;
}
