using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 여러 맵들의 정보를 한데 모아놓은 데이터
[CreateAssetMenu(fileName = "Stage List", menuName = "menu/All Stage List", order = 1)]
public class StageList : ScriptableObject
{
    // 맵 리스트
    [SerializeField]
    List<MapDataContainer> _list;
    public List<MapDataContainer> list
    {
        get { return _list; }
    }
}
