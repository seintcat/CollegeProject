using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 언어설정
[CreateAssetMenu(fileName = "Language", menuName = "menu/Language Data", order = 1)]
public class Language : ScriptableObject
{
    // 언어별 아이콘
    [SerializeField]
    List<Sprite> _icons;
    public List<Sprite> icons
    {
        get { return _icons; }
    }

    // 등록된 폰트 목록
    [SerializeField]
    public List<GlobalFont> _fonts;
    public List<GlobalFont> fonts
    {
        get { return _fonts; }
    }
}
