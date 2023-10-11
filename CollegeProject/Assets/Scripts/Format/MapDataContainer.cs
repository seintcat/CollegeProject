using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 한개 맵의 정보에 대한 데이터
[CreateAssetMenu(fileName = "Map_name", menuName = "menu/One Map Data", order = 1)]
public class MapDataContainer : ScriptableObject
{
    // 맵의 이름에 관한 데이터
    public string mapName;

    // 맵을 구현하는 데 필요한 맵 타일셋
    public Tileset floorTileset;
    public Tileset structTileset;

    // 맵의 크기 관련 데이터
    public short mapSizeX;
    public short mapSizeZ;

    // 카메라가 보는 중앙 청크 좌표기준 데이터
    public int centerX;
    public int centerZ;

    // 맵 백그라운드 기본 컬러
    public Color backgroundColor;

    // 맵 백그라운드 기본 배경 인덱스
    public int background;

    // 맵 레이어별 자료 배열이 저장된 파일 이름조각
    public static readonly string layerFloor = "_Floor";
    public static readonly string layerStruct = "_Struct";
    // 맵 자료 배열을 저장하는 자료형
    public static readonly string fileType = ".json";
    // 맵 배열 자료가 저장된 경로 (스트리밍 에셋 하위가 기본)
    public static readonly string mapLocation = "/Maps/";

    // 트리거, 액터, 아이템을 생성하기 위한 데이터
    public List<ActorSpawn> actorSpawn;
    public List<ItemSpawn> itemSpawn;
    public List<TriggerSpawn> triggerSpawn;

    public MapData GetMapData()
    {
        MapData map = new MapData();

        // 디폴트맵 기준 100, 100
        map.mapSizeX = mapSizeX;
        map.mapSizeZ = mapSizeZ;

        // 디폴트맵 기준 2, 2
        map.centerX = centerX;
        map.centerZ = centerZ;

        // 디폴트맵 기준 검은색
        map.backgroundColor = backgroundColor;

        // 디폴트맵 기준 -1(필터 없음)
        map.background = background;

        return map;
    }

    LayerData GetLayer(Tileset _tileset, ObjectType objType)
    {
        LayerData layer = new LayerData();

        // 디폴트맵 기준 테스트 타일셋 
        //Tileset tileset = new Tileset();
        //tileset.material = _tileset.material;
        //tileset.tilesetSize = _tileset.tilesetSize;
        //layer.tileset = tileset;
        layer.tileset = _tileset;

        // 레이어별 데이터 불러오기
        int[] data = { 0, 0 };
        switch (objType)
        {
            case ObjectType.FloorLayer:
                data = JsonHelper.FromJson<int>(File.ReadAllText(Application.streamingAssetsPath + mapLocation + mapName + layerFloor + fileType));
                break;
            case ObjectType.StructLayer:
                data = JsonHelper.FromJson<int>(File.ReadAllText(Application.streamingAssetsPath + mapLocation + mapName + layerStruct + fileType));
                break;
            default:
                break;
        }

        List<List<int>> layerBlocks = new List<List<int>>();
        for (int x = 0; x < mapSizeX; x++)
        {
            List<int> list = new List<int>();
            for (int z = mapSizeZ - 1; z > -1; z--)
                list.Add(data[(z * mapSizeZ) + x]);
            layerBlocks.Add(list);
        }

        layer.location = layerBlocks;

        return layer;
    }

    public Dictionary<int, LayerData> GetLayerData()
    {
        Dictionary<int, LayerData> pairs = new Dictionary<int, LayerData>();
        pairs.Add(LayerSettings.GetHeight(ObjectType.FloorLayer), GetLayer(floorTileset, ObjectType.FloorLayer));
        pairs.Add(LayerSettings.GetHeight(ObjectType.StructLayer), GetLayer(structTileset, ObjectType.StructLayer));
        return pairs;
    }
}
