using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 내부에서 쓰이는 모든 종류의 오브젝트에 대한 열거형
public enum ObjectType 
{
    None,
    BackgroundLayer,
    FloorLayer,
    ItemObject,
    ActorLow,
    StructLayer,
    ActorHigh,
    FogOfWar,
    EffectObject,
    PlayerObject,
    FillterLayer,
}
