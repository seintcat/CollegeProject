using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 레이어를 생성하는 데 필요한 불변하는 데이터
public static class LayerSettings
{
    // chunkCountX, Z를 / 2의 몫을 정수로 반환
    public static readonly int halfCountX = 2;
    public static readonly int halfCountZ = 2;

    // 하나의 레이어에 청크가 몇개 있는지 정의
    // 홀수 라인이어야 계산 좋으니까 참고
    public static readonly int chunkCountX = halfCountX * 2 + 1;
    public static readonly int chunkCountZ = halfCountZ * 2 + 1;

    // 각 레이어에 맞추어 레이어 이름을 다시 지정
    public static string LayerName(int index)
    {
        switch (index)
        {
            case 0:
                return "Layer_BackGround";
            case 1:
                return "Layer_Floor";
            case 4:
                return "Layer_Structure";
            default:
                return "Layer_None";
        }
    }

    // 각 레이어 높이에 맞는 높이값 반환
    public static int GetHeight(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.BackgroundLayer:
                return 0;
            case ObjectType.FloorLayer:
                return 1;
            case ObjectType.ItemObject:
                return 2;
            case ObjectType.StructLayer:
                return 3;
            case ObjectType.ActorLow:
                return 4;
            case ObjectType.ActorHigh:
                return 5;
            case ObjectType.FogOfWar:
                return 6;
            case ObjectType.EffectObject:
                return 7;
            case ObjectType.PlayerObject:
                return 8;
            case ObjectType.FillterLayer:
                return 9;
            default:
                break;
        }
        return 0;
    }
}
