using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임오브젝트 리스트
[CreateAssetMenu(fileName = "GameObject List", menuName = "menu/Game Object List", order = 1)]
public class ObjList : ScriptableObject
{
    public List<GameObject> list;
}
