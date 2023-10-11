using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사운드매니저의 볼륨 설정
[CreateAssetMenu(fileName = "Volume Setting", menuName = "menu/Volume Data", order = 1)]
public class Volume : ScriptableObject
{
    //효과음, 배경음 볼륨
    [SerializeField]
    [Range(0.0f, 2.0f)]
    public float bgm;
    [SerializeField]
    [Range(0.0f, 2.0f)]
    public float sfx;
}
