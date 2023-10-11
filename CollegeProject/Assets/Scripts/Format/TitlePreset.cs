using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 화면을 구성하기 위한 데이터
[CreateAssetMenu(fileName = "Title Presets", menuName = "menu/All Title preset list", order = 1)]
public class TitlePreset : ScriptableObject
{
    //프리셋 리스트
    [SerializeField]
    List<TitleData> _list;
    public List<TitleData> list { get { return _list; } }
}
