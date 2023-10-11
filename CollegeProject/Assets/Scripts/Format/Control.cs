using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//한개 조작타입
[CreateAssetMenu(fileName = "Control(Name)", menuName = "menu/One Control Type", order = 1)]
public class Control : ScriptableObject
{
    // 조작타입 이미지
    public Sprite image;

    // 조작타입 이름
    public string _name;
}
