using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 게임 오브젝트의 이름은 "Map"으로 고정
// 다른 게임 오브젝트의 싱글턴처럼 활용 가능하게끔
public class Map : MonoBehaviour
{
    // 아무 맵 데이터가 없을 때 사용할 테스트 맵 데이터
    [SerializeField]
    private MapDataContainer defultMap;

    // 맵의 기본 데이터
    public static MapData mapData;

    // Layer2D 레이어의 기본 데이터
    public static Dictionary<int, LayerData> layerDatas;

    // 전역으로 참조 가능한 팩토리 클래스
    public static ChunkFactory chunkFactory;
    public static Layer2DFactory layer2DFactory;

    //하나의 맵에 있는 모든 레이어
    public static Layer2D floorLayer, structLayer;

    //각 팩토리 연결
    public ChunkFactory _chunkFactory;
    public Layer2DFactory _layer2DFactory;

    //로딩이 트루면 맵이 아직 다 완성되지 않은것
    //이때는 입력받는 모든 메서드를 실행하지 않음
    private bool isLoading;

    // 카메라 컴포넌트
    public Camera2D camera2D;

    // 전역으로 참조 가능한 블럭리스트
    public static BlockList blockList;
    [SerializeField]
    private BlockList _blockList;

    // 상태이상 관련 전역 리스트
    public static List<EffectData> effectList;
    [SerializeField]
    private EffectList _effectList;

    // 액터 및 아이템, 트리거 리스트
    static List<GameObject> actorList;
    static List<GameObject> itemList;
    static List<GameObject> triggerList;
    [SerializeField]
    private ObjList _actorList;
    [SerializeField]
    private ObjList _itemList;
    [SerializeField]
    private ObjList _triggerList;

    // 현재 생성된 액터 리스트
    public static Dictionary<int, Actor> actors;

    // 현재 생성된 아이템 리스트
    public static Dictionary<int, Item> items;

    // 현재 생성된 트리거 리스트
    public static Dictionary<int, GameObject> triggers;

    // 아이템과 액터 인스턴스 인덱스 관리용
    static int actorsIndex, itemsIndex, triggersIndex;
    static List<int> unusingActorsIndex, unusingItemsIndex, unusingTriggersIndex;

    // 게임 일시정지
    static bool _pause = true;
    public static bool pause
    {
        set
        {
            _pause = value;
        }
    }

    //자기 자신을 전역으로 저장
    static Map map;

    // 맵 생성에 필요한 데이터 리스트
    [SerializeField]
    private StageList stageList;

    private void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 테스트용
    public static bool IsMovable(bool isflying, int x, int z)
    {
        if (mapData == null || layerDatas== null || layerDatas.Count < 2) return false;
        if (x < 0 || z < 0 || x >= mapData.mapSizeX || z >= mapData.mapSizeZ) return false;
        if (isflying) return true;

        bool floorL = false, structL = true;
        int block;
        int layerHeight;

        layerHeight = LayerSettings.GetHeight(ObjectType.StructLayer);
        block = layerDatas[layerHeight].tileset.blockIndex[layerDatas[layerHeight].location[x][z]];
        structL = blockList.blocks[block].isPassable;

        layerHeight = LayerSettings.GetHeight(ObjectType.FloorLayer);
        block = layerDatas[layerHeight].tileset.blockIndex[layerDatas[layerHeight].location[x][z]];
        floorL = blockList.blocks[block].isPassable;

        return floorL && structL;
    }

    // 초기화 메서드
    public void Init()
    {
        // 초기화 및 중복 방지
        var objs = FindObjectsOfType<Map>();
        if (objs.Length != 1)
        {
            Debug.Log("error : you can use only one Map.");
            Destroy(gameObject);
            return;
        }
        map = this;

        _pause = true;
        blockList = _blockList;
        effectList = _effectList.effects;

        // 액터와 아이템 등 초기화
        actorsIndex = 0;
        itemsIndex = 0;
        triggersIndex = 0; 
         unusingActorsIndex = new List<int>();
        unusingItemsIndex = new List<int>();
        unusingTriggersIndex = new List<int>(); 
        actors = new Dictionary<int, Actor>();
        items = new Dictionary<int, Item>();
        triggers = new Dictionary<int, GameObject>();

        // 팩토리 전역변수 등록
        chunkFactory = _chunkFactory;
        layer2DFactory = _layer2DFactory;
    }

    // 카메라가 보고있는 중심 청크가 변할 때 필요한 이벤트
    // 플레이어가 걸어서 이동하는 것을 가정
    public static void MoveChunkEvent(int x, int z)
    {
        // 플레이어 중심 청크가 이동 청크좌표와 같다면 중단
        if (mapData == null || mapData.centerX == x && mapData.centerZ == z) return;

        // 현재좌표에서 움직인 청크 거리 구함
        int _x = x - mapData.centerX;
        int _z = z - mapData.centerZ;

        // 중심 반영
        mapData.centerX = x;
        mapData.centerZ = z;

        // 걸어서 플레이어가 걸어서 이동한 상황이라면, 대각선 + 직선 두개의 방향으로만 가기 때문에
        Direction2D[] directions = { Direction2D.None, Direction2D.None };
        int[] moveCount = { 0, 0 };

        // 청크의 4방향 직선이동밖에 없을 시
        if(_x == 0)
        {
            if(_z > 0)
            {
                floorLayer.ChunkShift(Direction2D.Up, _z);
                structLayer.ChunkShift(Direction2D.Up, _z);
                return;
            }
            if (_z < 0)
            {
                floorLayer.ChunkShift(Direction2D.Down, _z * -1);
                structLayer.ChunkShift(Direction2D.Down, _z * -1);
                return;
            }
        }
        else if (_z == 0)
        {
            if (_x > 0)
            {
                floorLayer.ChunkShift(Direction2D.Right, _x);
                structLayer.ChunkShift(Direction2D.Right, _x);
                return;
            }
            if (_x < 0)
            {
                floorLayer.ChunkShift(Direction2D.Left, _x * -1);
                structLayer.ChunkShift(Direction2D.Left, _x * -1);
                return;
            }
        }

        // 청크의 대각선 이동시
        if (_x > 0)
        {
            if (_z > 0)
            {
                directions[0] = Direction2D.UpRight;
                if (_x < _z)
                {
                    moveCount[0] = _x;
                    _z -= _x;
                    _x = 0;
                }
                else // (_x > _z)
                {
                    moveCount[0] = _z;
                    _x -= _z;
                    _z = 0;
                }
            }
            else // _z < 0
            {
                directions[0] = Direction2D.DownRight;
                _z *= -1;
                if (_x < _z)
                {
                    moveCount[0] = _x;
                    _z -= _x;
                    _x = 0;
                    _z *= -1;
                }
                else // (_x > _z)
                {
                    moveCount[0] = _z;
                    _x -= _z;
                    _z = 0;
                }
            }
        }
        else // _x < 0
        {
            _x *= -1;
            if (_z > 0)
            {
                directions[0] = Direction2D.UpLeft;
                if (_x < _z)
                {
                    moveCount[0] = _x;
                    _z -= _x;
                    _x = 0;
                }
                else // (_x > _z)
                {
                    moveCount[0] = _z;
                    _x -= _z;
                    _z = 0;
                    _x *= -1;
                }
            }
            else // _z < 0
            {
                _z *= -1;
                directions[0] = Direction2D.DownLeft;
                if (_x < _z)
                {
                    moveCount[0] = _x;
                    _z -= _x;
                    _x = 0;
                    _z *= -1;
                }
                else // (_x > _z)
                {
                    moveCount[0] = _z;
                    _x -= _z;
                    _z = 0;
                    _x *= -1;
                }
            }
        }

        // 이후의 직선이동 정의
        if(_x > 0)
        {
            directions[1] = Direction2D.Right;
            moveCount[1] = _x;
        }
        else if (_x < 0)
        {
            directions[1] = Direction2D.Left;
            moveCount[1] = _x * -1;
        }
        else if (_z > 0)
        {
            directions[1] = Direction2D.Up;
            moveCount[1] = _z;
        }
        else if (_z < 0)
        {
            directions[1] = Direction2D.Down;
            moveCount[1] = _z * -1;
        }

        // 정의된 이동 실행
        floorLayer.ChunkShift(directions[0], moveCount[0]);
        floorLayer.ChunkShift(directions[1], moveCount[1]);
        structLayer.ChunkShift(directions[0], moveCount[0]);
        structLayer.ChunkShift(directions[1], moveCount[1]);
    }

    // 맵을 초기에 불러올 때나, 플레이어가 텔레포트등을 사용할 때 전체 청크를 재구성하는 이벤트
    // Layer2D의 JumpChunkEvent() 사용
    public static void JumpChunkEvent(int x, int z)
    {
        floorLayer.JumpChunkEvent(x, z);
        structLayer.JumpChunkEvent(x, z);
    }

    // 블럭의 블렌딩을 위해 계산해야 하는 한 블럭 주위의 다른 블럭들 정보
    public static Vector2[] CalculateBlendBlockData(int blockX, int blockZ, int layerHeight)
    {
        // 겉으로 보이는 블럭 외형
        int blockTexture = layerDatas[layerHeight].location[blockX][blockZ];
        // 블록이 실제로 의미하는 값
        int thisBlock = layerDatas[layerHeight].tileset.blockIndex[blockTexture];
        if (!blockList.blocks[thisBlock].isBlendBlock)
            return layerDatas[layerHeight].tileset.GetUV(blockTexture);

        List<bool> lookingBlocks = new List<bool>();
        List<bool> directions = new List<bool>();
        for (int x = blockX - 1; x <= blockX + 1; x++)
            for (int z = blockZ - 1; z <= blockZ + 1; z++)
            {
                if (x < 0 || x >= mapData.mapSizeX || z < 0 || z >= mapData.mapSizeZ) lookingBlocks.Add(true);
                else if(layerDatas[layerHeight].tileset.blockIndex[layerDatas[layerHeight].location[x][z]] == thisBlock) lookingBlocks.Add(true);
                else lookingBlocks.Add(false);
            }

        directions.Add(lookingBlocks[1]); // down
        directions.Add(lookingBlocks[0]); // down left
        directions.Add(lookingBlocks[3]); // left
        directions.Add(lookingBlocks[6]); // up left
        directions.Add(lookingBlocks[7]); // up
        directions.Add(lookingBlocks[8]); // up right
        directions.Add(lookingBlocks[5]); // right
        directions.Add(lookingBlocks[2]); // down right

        return layerDatas[layerHeight].tileset.GetUV(blockTexture, directions);
    }

    // 맵의 좌표에 해당되는 액터, 아이템, 트리거를 검색
    public static List<Actor> FindActors(int x, int y, int z, bool passableCheck)
    {
        List<Actor> list = new List<Actor>();
        foreach(Actor actor in actors.Values)
        {
            if (actor.IsHere(x, y, z, passableCheck))
                list.Add(actor);
        }

        return list;
    }

    public static List<Item> FindItems(int x, int z)
    {
        List<Item> list = new List<Item>();
        foreach (Item item in items.Values)
        {
            if (item.IsHere(x, z))
                list.Add(item);
        }

        return list;
    }

    public static List<GameObject> FindTriggers(int x, int z)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject obj in triggers.Values)
        {
            if ((int)obj.transform.position.x == x && (int)obj.transform.position.z == z)
                list.Add(obj);
        }

        return list;
    }

    // 액터, 아이템, 트리거의 스폰, 초기화, 관리
    public static Actor SpawnActor(int code, int x, int z, MoveType moveType)
    {
        if (unusingActorsIndex.Count > 0)
        {
            actors.Add(unusingActorsIndex[0], Instantiate(actorList[code]).GetComponent<Actor>().Init(x, z, unusingActorsIndex[0], moveType, _pause));
            unusingActorsIndex.RemoveAt(0);
        }
        else
        {
            actors.Add(actorsIndex, Instantiate(actorList[code]).GetComponent<Actor>().Init(x, z, actorsIndex, moveType, _pause));
            actorsIndex++;
        }

        return actors[actors.Count - 1];
    }
    public static Actor SpawnActor(ActorSpawn spawn)
    {
        return SpawnActor(spawn.index, spawn.x, spawn.z, spawn.moveType);
    }

    public static Item SpawnItem(int code, int x, int z, int count = 1)
    {
        int returnIndex;
        if (items.Count > 0)
            foreach (Item item in items.Values)
                if (item.itemCode == code && item.IsHere(x, z) && (item.count + count) <= item.data.maxCount)
                {
                    item.count += count;
                    return item;
                }

        if (unusingItemsIndex.Count > 0)
        {
            items.Add(unusingItemsIndex[0], Instantiate(itemList[code]).GetComponent<Item>().Init(x, z, code, count, _pause, unusingItemsIndex[0]));
            returnIndex = unusingItemsIndex[0];
            unusingItemsIndex.RemoveAt(0);
        }
        else
        {
            returnIndex = itemsIndex;
            items.Add(itemsIndex, Instantiate(itemList[code]).GetComponent<Item>().Init(x, z, code, count, _pause, itemsIndex));
            itemsIndex++;
        }

        return items[returnIndex];
    }
    public static Item SpawnItem(ItemSpawn spawn)
    {
        return SpawnItem(spawn.code, spawn.x, spawn.z, spawn.count);
    }

    public static GameObject SpawnTrigger(int code, int x, int z)
    {
        GameObject obj = Instantiate(triggerList[code]);
        obj.transform.position = new Vector3(x, obj.transform.position.y, z);
        if (unusingTriggersIndex.Count > 0)
        {
            triggers.Add(unusingTriggersIndex[0], obj);
            unusingTriggersIndex.RemoveAt(0);
        }
        else
        {
            triggers.Add(triggersIndex, obj);
            triggersIndex++;
        }

        return triggers[triggers.Count - 1];
    }
    public static GameObject SpawnTrigger(TriggerSpawn spawn)
    {
        return SpawnTrigger(spawn.code, spawn.x, spawn.z);
    }

    // 인덱스에 해당하는 액터, 아이템, 트리거 제거
    public static void RemoveItem(int index)
    {
        if (items.ContainsKey(index))
        {
            Destroy(items[index].gameObject);
            items.Remove(index);
            unusingItemsIndex.Add(index);
        }
    }
    public static void RemoveActor(int index)
    {
        if (actors.ContainsKey(index))
        {
            Destroy(actors[index].gameObject);
            actors.Remove(index);
            unusingActorsIndex.Add(index);
        }
        else
            Debug.Log("Wrong actor index");
    }
    public static void RemoveTrigger(int index)
    {
        if (triggers.ContainsKey(index))
        {
            GameObject obj = triggers[index];
            triggers.Remove(index);
            Destroy(obj);
            unusingTriggersIndex.Add(index);
        }
        else
            Debug.Log("Wrong trigger index");
    }

    // 블럭 하나를 바꾸는 메서드
    public void BlockChange(int x, int z, ObjectType layerType, int blockValue)
    {
        if (x < 0 || x >= mapData.mapSizeX || z < 0 || z >= mapData.mapSizeZ)
            return;

        if (layerType != ObjectType.FloorLayer && layerType != ObjectType.StructLayer)
        {
            Debug.Log("Wrong layer type");
            return;
        }

        layerDatas[LayerSettings.GetHeight(layerType)].location[x][z] = blockValue;

        switch (layerType)
        {
            case ObjectType.FloorLayer:
                floorLayer.BlockChangeEvent(x, z);
                break;
            case ObjectType.StructLayer:
                structLayer.BlockChangeEvent(x, z);
                break;
            default:
                break;
        }
    }

    // 맵을 만드는 메서드
    public static void MakeMap()
    {
        map.MakeMap_();
    }
    public void MakeMap_()
    {
        int index = Save.GetSave().stageIndex;
        MapDataContainer load;
        if (index > -1 && stageList.list.Count > index)
            load = defultMap;
        else
            load = stageList.list[index];

        // 맵 데이터 정보를 넣음
        mapData = load.GetMapData();

        // 레이어 정보 등록
        layerDatas = load.GetLayerData();
        // 레이어별 타일셋 초기화
        foreach (LayerData data in layerDatas.Values)
            data.tileset.CalculateBlockSize();

        // 각 레이어 초기화
        floorLayer = layer2DFactory.GetLayer(ObjectType.FloorLayer);
        structLayer = layer2DFactory.GetLayer(ObjectType.StructLayer);

        // 액터와 아이템 등 스폰, 카메라 연결
        // 액터
        camera2D = FindObjectsOfType<Camera2D>()[0];
        if (_actorList != null && _actorList.list.Count > 0)
        {
            actorList = _actorList.list;
            
            if (load.actorSpawn != null && load.actorSpawn.Count > 0)
            {
                foreach (ActorSpawn spawn in load.actorSpawn)
                    SpawnActor(spawn);
                camera2D.SetTarget(actors[load.actorSpawn[0].index].transform);
            }
            else
                camera2D.Teleport(load.centerX, load.centerZ);
        }
        else
            camera2D.Teleport(load.centerX, load.centerZ);

        // 아이템
        if (_itemList != null && _itemList.list.Count > 0)
        {
            itemList = _itemList.list;

            if (load.itemSpawn != null && load.itemSpawn.Count > 0)
                foreach (ItemSpawn spawn in load.itemSpawn)
                    SpawnItem(spawn);
        }

        // 트리거
        if (_triggerList != null && _triggerList.list.Count > 0)
        {
            triggerList = _triggerList.list;

            if (load.triggerSpawn != null && load.triggerSpawn.Count > 0)
                foreach (TriggerSpawn spawn in load.triggerSpawn)
                    SpawnTrigger(spawn);
        }

        // 배경 재생
        BackgroundLayer.Play(mapData.background);
    }
}
