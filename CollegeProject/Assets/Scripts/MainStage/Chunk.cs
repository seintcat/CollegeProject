using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// VoxelData를 사용하여 블록을 표시하는 클래스 = 하나의 청크
public class Chunk : MonoBehaviour
{
    //블록을 보여주기 위해 위의 두 컴포넌트가 필요
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    //청크로서의 위치
    private int locationX;
    private int locationY;
    private int locationZ;

    // 청크 내의 한 블록에 관한 데이터
    private List<List<Block>> blockList;

    // locationX, locationZ의 외부 접근 형식
    public int x
    {
        get { return locationX; }
    }
    public int z
    {
        get { return locationZ; }
    }

    public Material material
    {
        set { gameObject.GetComponent<MeshRenderer>().material = value; }
    }

    // 청크 내의 블록 수는 ChunkSettings의 blockCountXZ로 저장
    // 청크 오브젝트는 생성시 이름이 Chunk(x, y, z)로 실시간 반영

    // Map에서 레이어 기준 x, z좌표의 블록 외형 정보를 수정한 이벤트
    public void BlockChangeEvent(int _x, int _z)
    {
        if (_x >= (int)(transform.position.x - 1) && _z >= (int)(transform.position.z - 1) && _x <= (int)(transform.position.x + ChunkSettings.blockCountX) && _z <= (int)(transform.position.z + ChunkSettings.blockCountZ))
            BlockRend();
    }

    //수정된 전체 블록의 정보를 실제로 인게임 내에 표시
    public void BlockRend()
    {
        // Vector3 정점 리스트
        List<Vector3> vertices = new List<Vector3>();
        // 삼각형 폴리곤 리스트
        List<int> triangles = new List<int>();
        // 텍스쳐 매핑 리스트
        List<Vector2> uvs = new List<Vector2>();

        // 각 블록 정보를 하나로 통합
        foreach (List<Block> _list in blockList)
            foreach (Block block in _list)
            {
                vertices.AddRange(block.vertices);
                triangles.AddRange(block.triangles);
                uvs.AddRange(block.uvs);
            }

        // 새 메쉬데이터 생성 후 정점, 삼각면, uv텍스쳐 정보 삽입
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        // 정보가 삽입된 메쉬 재계산
        mesh.RecalculateNormals();
        // 메쉬필터에 메쉬 적용
        meshFilter.mesh = mesh;

        // 청크 보이기
        RenderingSwitch(true);
    }

    //청크를 청크좌표 x, z로 움직인다
    //움직이면서, 맵의 경계선 밖으로 나갈 시 RenderingSwitch()로 조정한다
    public void MoveChunk(int x, int z)
    {
        // 삼각형 정점입력에 필요한 인덱스
        int verticesIndex = 0;

        // 청크 좌표 대입
        locationX = x;
        locationZ = z;
        // 청크 숨기기
        RenderingSwitch(false);
        // 청크 위치 이동
        transform.position = new Vector3((ChunkSettings.blockCountX * locationX), locationY, (ChunkSettings.blockCountZ * locationZ));
        // chunk(x, y, z)로 이름 변경
        gameObject.name = "chunk(" + locationX + ", " + locationY + ", " + locationZ + ")";
        // 청크가 조금이라도 맵 안에 보인다면
        if (((locationZ + 1) * ChunkSettings.blockCountZ) - 1 > 0 ||
            ((locationX + 1) * ChunkSettings.blockCountX) - 1 > 0 ||
            locationZ * ChunkSettings.blockCountZ <= Map.mapData.mapSizeZ ||
            locationX * ChunkSettings.blockCountX <= Map.mapData.mapSizeX)
        {
            // 블록 정보 초기화
            blockList = new List<List<Block>>();

            // 하나의 청크에 해당하는 blockCountX * blockCountZ개의 블록 정보 계산
            for (int _x = 0; _x < ChunkSettings.blockCountX; _x++)
            {
                List<Block> list = new List<Block>();

                for (int _z = 0; _z < ChunkSettings.blockCountZ; _z++)
                {
                    Block block = new Block();

                    if (transform.position.x + _x >= 0 && transform.position.x + _x < Map.mapData.mapSizeX && transform.position.z + _z >= 0 && transform.position.z + _z < Map.mapData.mapSizeZ)
                    {
                        verticesIndex = block.MakeBlock(_x, _z, verticesIndex, Map.CalculateBlendBlockData((ChunkSettings.blockCountX * locationX) + _x, (ChunkSettings.blockCountZ * locationZ) + _z, locationY));
                    }
                    else block.MakeBlock();

                    list.Add(block);
                }

                blockList.Add(list);
            }
        }
    }

    //청크를 초기화한다
    public Chunk Init(int _locationY, Material _material)
    {
        // 청크 좌표 초기화
        locationX = -1;
        locationZ = -1;
        // 청크 높이 초기화
        locationY = _locationY;
        // 청크 외형 비활성화
        RenderingSwitch(false);
        // 청크 이름 제거하기
        gameObject.name = "chunk(null)";
        // 블록정보 리스트
        blockList = new List<List<Block>>();
        // 청크 머티리얼
        material = _material;

        return this;
    }

    //bool값에 따라 메쉬렌더러를 활성화 / 비활성화한다
    private void RenderingSwitch(bool flag)
    {
        meshRenderer.enabled = flag;
    }

    // 청크 불필요 메모리 리셋
    public void ResetChunk()
    {
        // 청크 좌표 초기화
        locationX = -1;
        locationZ = -1;
        // 청크 외형 비활성화
        RenderingSwitch(false);
        // 청크 이름 제거하기
        gameObject.name = "chunk(null)";
    }
}
