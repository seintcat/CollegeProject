using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ŵ����� ���� ����
[CreateAssetMenu(fileName = "Volume Setting", menuName = "menu/Volume Data", order = 1)]
public class Volume : ScriptableObject
{
    //ȿ����, ����� ����
    [SerializeField]
    [Range(0.0f, 2.0f)]
    public float bgm;
    [SerializeField]
    [Range(0.0f, 2.0f)]
    public float sfx;
}
