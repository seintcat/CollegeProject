using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 청크를 생성하는 팩토리 클래스
// 맵 오브젝트에 컴포넌트로 부착
public class ChunkFactory : MonoBehaviour
{
    // 생성에 필요한 프리팹 오브젝트
    public GameObject prefabObj;

    // 이미 만들어진 청크 이중 리스트를 인덱스별로 따로 저장
    private Dictionary<int, List<List<Chunk>>> makedChunks;
    // 사용이 해제된 청크들을 따로 보관
    private List<Chunk> remainChunks;

    // 해당인덱스를 받아서 청크 이중 리스트 반환
    public List<List<Chunk>> GetChunks(int index)
    {
        // 이미 같은 인덱스의 청크가 나와있다면 makedChunks에 이미 저장된 이중 리스트를 초기화시켜서 반환
        if (makedChunks.ContainsKey(index))
        {
            foreach (List<Chunk> list in makedChunks[index])
                foreach (Chunk item in list)
                    item.ResetChunk();
            return makedChunks[index];
        }
        else
        {
            List<List<Chunk>> chunks = new List<List<Chunk>>();

            // 만든 청크는 Init()으로 초기화하고 리스트 더하기
            // LayerData의 chunkCountXZ로 총 청크 수를 알 수 있음
            for (int x = 0; x < LayerSettings.chunkCountX; x++)
            {
                List<Chunk> list = new List<Chunk>();
                for (int z = 0; z < LayerSettings.chunkCountZ; z++)
                {
                    // 이전에 사용했던 청크가 있다면 할당, 아니면 새로 생성
                    if(remainChunks.Count > 0)
                    {
                        // 청크 초기화
                        remainChunks[0].Init(index, Map.layerDatas[index].tileset.material);
                        list.Add(remainChunks[0]);
                        remainChunks.RemoveAt(0);
                    }
                    else
                    {
                        GameObject obj = Instantiate(prefabObj);
                        // 청크 초기화
                        // locationY는 index값을 높이로 받는다
                        list.Add(obj.GetComponent<Chunk>().Init(index, Map.layerDatas[index].tileset.material));
                    }
                }
                chunks.Add(list);
            }

            // 처음에 청크들을 만든 시점에서는, 해당 청크리스트를 makedChunks에 저장
            makedChunks.Add(index, chunks);
            return chunks;
        }
    }

    // 초기화 및 중복 방지
    public void Init()
    {
        var objs = FindObjectsOfType<ChunkFactory>();
        if (objs.Length != 1)
        {
            Debug.Log("error : you can use only one ChunkFactory.");
            Destroy(gameObject);
            return;
        }

        if (remainChunks == null) remainChunks = new List<Chunk>();

        // 이전에 만든 청크가 존재한다면 이전의 청크 따로 보관 >> 레이어 크기 변동되었을 시 청크도 달라져야 함
        if (makedChunks != null)
        {
            makedChunks.Clear();
            var chunks = FindObjectsOfType<Chunk>();
            foreach (Chunk chunk in chunks)
            {
                chunk.ResetChunk();
                remainChunks.Add(chunk);
            }
        }
        else makedChunks = new Dictionary<int, List<List<Chunk>>>();
    }

    private void Awake()
    {
        Init();
    }

    // 인덱스에 해당하는 청크 반환받음(가비지콜렉팅)
    public void ReturnChunks(int index)
    {
        if(makedChunks != null && makedChunks.ContainsKey(index))
        {
            foreach (List<Chunk> list in makedChunks[index])
                foreach (Chunk chunk in list)
                {
                    chunk.ResetChunk();
                    remainChunks.Add(chunk);
                }

            makedChunks.Remove(index);
        }
    }
}
