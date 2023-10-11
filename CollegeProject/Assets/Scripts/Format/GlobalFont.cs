using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 언어별 지원이 되는 폰트 클래스
[System.Serializable]
public class GlobalFont
{
    // 폰트 데이터
    [SerializeField]
    TMP_FontAsset _fontAsset;
    public TMP_FontAsset fontAsset
    {
        get { return _fontAsset; }
    }

    // 폰트가 지원하는 언어 목록
    [SerializeField]
    List<bool> _usableLanguage;
    public List<bool> usableLanguage
    {
        get { return _usableLanguage; }
    }
}
