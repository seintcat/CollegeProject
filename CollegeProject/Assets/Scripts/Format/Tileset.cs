using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵 타일셋 클래스
[CreateAssetMenu(fileName = "Tileset_name", menuName = "menu/Tile Set", order = 1)]
public class Tileset : ScriptableObject
{
    // 타일셋 이미지 머티리얼
    public Material material;

    // 타일셋 내부의 가로세로 블럭 개수
    public int tilesetSize;

    // 일반 블록이 타일맵에서 차지하는 칸 수
    public static readonly int nomalBlockSize = 1;
    // 블랜딩을 지원하는 블록이 타일맵에서 차지하는 칸 수
    public static readonly int blendBlockSize = 5;

    // 텍스쳐의 좌하단부터 배치된 블록들의 데이터 인덱스를 가리킴
    // 해당 인덱스는 BlockList에 대입됨
    public List<int> blockIndex;

    // 텍스쳐 각 블록 인덱스마다 필요한 사이즈 인덱스
    // 처음 쓸때 계산하고, 한번 계산되었으면 있던거 계속 쓰기
    // 스크립터블 오브젝트 파일로 저장되면 곤란하니까 퍼블릭으로 일단 해놓기
    List<int> blockSize = null;

    public void CalculateBlockSize()
    {
        if (!(blockSize == null || blockSize.Count != blockIndex.Count))
            return;

        blockSize = new List<int>();
        int size = 0;

        for (int index = 0; index < blockIndex.Count; index++)
        {
            blockSize.Add(size);

            if (Map.blockList.blocks[blockIndex[index]].isBlendBlock)
                size += blendBlockSize;
            else
                size += nomalBlockSize;
        }
    }

    // 타일셋에서 좌표값을 얻어오는 메서드
    public Vector2[] GetUV(int blockData, List<bool> directions = null)
    {
        int index = blockSize[blockData];
        float oneBlock = 1f / tilesetSize;

        List<Vector2> uvs = new List<Vector2>();
        if (directions == null) // 블럭이 블렌딩을 지원하지 않을 때
        {
            int x = index % tilesetSize;
            int z = index / tilesetSize;

            uvs.Add( new Vector2(x * oneBlock, z * oneBlock) );
            uvs.Add( new Vector2(x * oneBlock, (z * oneBlock) + oneBlock) );
            uvs.Add( new Vector2((x * oneBlock) + oneBlock, (z * oneBlock) + oneBlock) );
            uvs.Add( new Vector2((x * oneBlock) + oneBlock, z * oneBlock) );
        }
        else // 블럭이 블렌딩을 지원할 때
        {
            float halfBlock = oneBlock / 2;

            // DownLeft
            Vector2 vector = GetBlockPart(index, directions[2], directions[1], directions[0]);

            uvs.Add(new Vector2(vector.x * oneBlock, vector.y * oneBlock));
            uvs.Add(new Vector2(vector.x * oneBlock, (vector.y * oneBlock) + halfBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + halfBlock, (vector.y * oneBlock) + halfBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + halfBlock, vector.y * oneBlock));

            // UpLeft
            vector = GetBlockPart(index, directions[0], directions[7], directions[6]);

            uvs.Add(new Vector2(vector.x * oneBlock, (vector.y * oneBlock) + halfBlock));
            uvs.Add(new Vector2(vector.x * oneBlock, (vector.y * oneBlock) + oneBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + halfBlock, (vector.y * oneBlock) + oneBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + halfBlock, (vector.y * oneBlock) + halfBlock));

            // UpRight
            vector = GetBlockPart(index, directions[6], directions[5], directions[4]);

            uvs.Add(new Vector2((vector.x * oneBlock) + halfBlock, (vector.y * oneBlock) + halfBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + halfBlock, (vector.y * oneBlock) + oneBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + oneBlock, (vector.y * oneBlock) + oneBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + oneBlock, (vector.y * oneBlock) + halfBlock));

            // DownRight
            vector = GetBlockPart(index, directions[4], directions[3], directions[2]);

            uvs.Add(new Vector2((vector.x * oneBlock) + halfBlock, vector.y * oneBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + halfBlock, (vector.y * oneBlock) + halfBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + oneBlock, (vector.y * oneBlock) + halfBlock));
            uvs.Add(new Vector2((vector.x * oneBlock) + oneBlock, vector.y * oneBlock));
        }
        return uvs.ToArray();
    }

    Vector2 GetBlockPart(int index, bool look1, bool look2, bool look3)
    {
        int x = 0;
        int z = 0;
        bool[] lookup = new bool[2];

        if (look1 && look2 && look3)
        {
            x = (index + 4) % tilesetSize;
            z = (index + 4) / tilesetSize;
        }
        else
        {
            lookup[0] = look1;
            lookup[1] = look3;
            switch (string.Join(",", lookup))
            {
                case "False,False":
                    x = index % tilesetSize;
                    z = index / tilesetSize;
                    break;
                case "False,True":
                    x = (index + 1) % tilesetSize;
                    z = (index + 1) / tilesetSize;
                    break;
                case "True,False":
                    x = (index + 2) % tilesetSize;
                    z = (index + 2) / tilesetSize;
                    break;
                case "True,True":
                    x = (index + 3) % tilesetSize;
                    z = (index + 3) / tilesetSize;
                    break;
                default:
                    Debug.Log(lookup.ToString());
                    break;
            }
        }

        return new Vector2(x, z);
    }

}
