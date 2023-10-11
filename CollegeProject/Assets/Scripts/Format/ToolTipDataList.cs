using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 툴팁 데이터 리스트
[CreateAssetMenu(fileName = "ToolTip Data List", menuName = "menu/ToolTip Data List", order = 1)]
public class ToolTipDataList : ScriptableObject
{
    public List<ToolTipData> list;
}
