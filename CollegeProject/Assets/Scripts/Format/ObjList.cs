using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ӿ�����Ʈ ����Ʈ
[CreateAssetMenu(fileName = "GameObject List", menuName = "menu/Game Object List", order = 1)]
public class ObjList : ScriptableObject
{
    public List<GameObject> list;
}
