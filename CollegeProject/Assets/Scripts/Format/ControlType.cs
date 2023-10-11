using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 조작타입 설정
[CreateAssetMenu(fileName = "Control Type", menuName = "menu/Control Type", order = 1)]
public class ControlType : ScriptableObject
{
    // 조작타입 목록
    [SerializeField]
    List<Control> _list;
    public List<Control> list
    {
        get { return _list; }
    }
}
