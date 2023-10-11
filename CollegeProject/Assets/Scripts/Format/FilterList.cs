using System.Collections.Generic;
using UnityEngine;

// 화면 필터 데이터 전체를 저장
[CreateAssetMenu(fileName = "Filter List", menuName = "menu/Filter List")]
public class FilterList : ScriptableObject
{
    // 화면 필터 모음
    [SerializeField]
    public List<FilterSet> filter; 
}
//https://wergia.tistory.com/189
