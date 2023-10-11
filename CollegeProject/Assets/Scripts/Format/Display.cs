using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 화면 설정값 모음
[CreateAssetMenu(fileName = "Display", menuName ="menu/Display Option", order = 1)]
public class Display : ScriptableObject
{
    // 화면 해상도 리스트
    [SerializeField]
    List<Ratio> _ratio;
    public List<Ratio> ratio { get { return _ratio; } }

    // 여백값
    public float padding;
}
