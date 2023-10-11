using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하나의 아이템에 대한 데이터
[CreateAssetMenu(fileName = "Name(Data)", menuName = "menu/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    //상호작용이 가능한지
    public bool isUsable;

    //장착이 가능한지
    public bool isEquippable;

    //액터에게 해당 아이템의 통제가 가능한지
    public bool controlDisable;

    //아이템을 쌓을 수 있는 최대 갯수
    public int maxCount;
}
