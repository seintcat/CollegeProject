using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 화면 설정값
[System.Serializable]
public class Ratio
{
    // 목록이름
    public string optionName { get { return value.x + " : " + value.y; } }

    // 비율 값 예시) 16:9
    [SerializeField]
    private Vector2Int _value;
    public Vector2Int value { get { return _value; } }

    //화면 화소
    [SerializeField]
    private Vector2Int _pixel;
    public Vector2Int pixel { get { return _pixel; } }
}
