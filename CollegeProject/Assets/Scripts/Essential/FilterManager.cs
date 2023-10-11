using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전체 LayerFilter를 관리하는 컴포넌트
public class FilterManager : MonoBehaviour
{
    // 전체 필터 정보가 저장되어있는 데이터 파일
    [SerializeField]
    private FilterList filterList;

    // 레이어 필터 프리팹
    [SerializeField]
    private GameObject layerFilterPrefab;

    // 자기 자신 컴포넌트를 전역으로 사용
    private static FilterManager manager;

    // 생성한 레이어 필터 리스트
    private static List<LayerFilter> activeFilter;

    // 비활성화상태로 대기중인 필터 리스트
    private static List<LayerFilter> filterQueue;

    // 초기화 메서드
    void Init()
    {
        // 중복검사
        var objs = FindObjectsOfType<FilterManager>();
        if(objs.Length != 1)
        {
            Debug.Log("error: you can use only one FilterManager");
            Destroy(gameObject);
            return;
        }

        // 전역변수에 인스턴스 대입.
        manager = this;

        activeFilter = new List<LayerFilter>();
        filterQueue = new List<LayerFilter>();
    }

    // index값에 해당하는 필터 재생
    public static void Play(int _index)
    {
        GameObject filter;
        LayerFilter layer;

        if (filterQueue.Count > 0)
        {
            layer = filterQueue[0];
            filterQueue.RemoveAt(0);
        }
        else
        {
            // 인스턴스 생성
            filter = Instantiate(manager.layerFilterPrefab);

            // 인스턴스를 부모 트랜스폼으로 할당
            filter.transform.SetParent(manager.gameObject.transform);

            // 필터 크기 맞춤
            RectTransform _transform = filter.GetComponent<RectTransform>();
            _transform.offsetMin = new Vector2(0, 0);
            _transform.offsetMax = new Vector2(0, 0);
            _transform.localScale = new Vector3(1f, 1f, 1f);

            // 리스트에 인스턴스 추가
            layer = filter.GetComponent<LayerFilter>();
        }

        activeFilter.Add(layer);
        layer.Play(_index);
    }

    // 전체 필터 끄기
    public static void Off()
    {
        if (activeFilter == null)
            return;

        while(activeFilter.Count > 0)
        {
            LayerFilter filter = activeFilter[0];
            activeFilter.RemoveAt(0);

            filterQueue.Add(filter);
            filter.Off();
        }
    }

    // 반복되는 필터만 끄기
    public static void LoopClose()
    {
        List<LayerFilter> list = new List<LayerFilter>();

        while (activeFilter.Count > 0)
        {
            LayerFilter filter = activeFilter[0];
            activeFilter.RemoveAt(0);

            if (filter.isLoop)
            {
                filterQueue.Add(filter);
                filter.Off();
            }
            else
                list.Add(filter);
        }

        activeFilter = list;
    }

    // 특정 종류의 필터만 끄기
    public static void Off(int _index)
    {
        List<LayerFilter> list = new List<LayerFilter>();

        while (activeFilter.Count > 0)
        {
            LayerFilter filter = activeFilter[0];
            activeFilter.RemoveAt(0);

            if (filter.CheckIndex(_index))
            {
                filterQueue.Add(filter);
                filter.Off();
            }
            else
                list.Add(filter);
        }

        activeFilter = list;
    }

    // 단발성 필터의 종료 여부 확인 및 처리
    public static void CheckOff()
    {
        List<LayerFilter> list = new List<LayerFilter>();

        while (activeFilter.Count > 0)
        {
            LayerFilter filter = activeFilter[0];
            activeFilter.RemoveAt(0);

            if (filter.gameObject.activeSelf)
                list.Add(filter);
            else
                filterQueue.Add(filter);
        }

        activeFilter = list;
    }

    // 필터 인덱스 참조 메서드
    public static FilterSet GetFilter(int index)
    {
        if (manager == null) return null;
        else return manager.filterList.filter[index];
    }

    private void Awake()
    {
        Init();
    }
}
