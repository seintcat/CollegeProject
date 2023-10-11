using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지문 및 텍스트, 폰트 사용 정보
[System.Serializable]
public class Context
{
    // 지문
    [TextArea]
    public string text;

    // 사용할 폰트
    public int fontIndex;

    // 폰트 사이즈
    public float fontSize;
}
