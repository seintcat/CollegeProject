using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer2DFactory : MonoBehaviour
{
    // 생성에 필요한 프리팹 오브젝트
    public GameObject prefabObj;

    // 전체 레이어가 생성된후 인덱스에 맞추어 이쪽에 저장
    // 레이어 수는 정해져 있으므로, 배열로 다룸
    private Dictionary<int, Layer2D> layerList = new Dictionary<int, Layer2D>();

    // 레이어 오브젝트를 생성
    // layerList에 이미 인스턴스가 존재한다면 해당 오브젝트를 재활용
    public Layer2D GetLayer(ObjectType type)
    {
        int index = LayerSettings.GetHeight(type); 

        // layerList에 이미 인스턴스가 존재한다면 해당 오브젝트를 재활용, 없으면 새로 생성
        if (layerList.ContainsKey(index))
        {
            layerList[index].ResetLayer();
            return layerList[index].Init(index);
        }
        else
        {
            GameObject obj = Instantiate(prefabObj);

            // locationY는 index값을 높이로 받는다
            layerList.Add(index, obj.GetComponent<Layer2D>().Init(index));
            return obj.GetComponent<Layer2D>();
        }
    }

    // 레이어팩토리가 하나만 존재하는지 확인하는 메서드
    // 초기화 및 중복 방지
    public void Init()
    {
        var objs = FindObjectsOfType<Layer2DFactory>();
        if (objs.Length != 1)
        {
            Debug.Log("error : you can use only one LayerFactory.");
            Destroy(gameObject);
            return;
        }

        // 이전에 만든 레이어가 존재한다면 이전의 레이어 초기화
        if (layerList != null && layerList.Count > 0)
        {
            // 레이어를 초기화한다면 청크도 삭제해야 할 가능성이 높으므로 청크팩토리 초기화
            Map.chunkFactory.Init();

            // 각 형식의 레이어들 초기화 
            var layers = FindObjectsOfType<Layer2D>();
            foreach (Layer2D layer in layers)
                Destroy(layer.gameObject);
        }
    }

    private void Awake()
    {
        Init();
    }
}
