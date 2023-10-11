using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이지 선택창 관리자
public class StageSelectManager : MonoBehaviour
{
    //프리팹 리스트
    [SerializeField]
    ObjList selectors;

    //자기 자신 컴포넌트를 전역으로 사용
    private static StageSelectManager manager;

    //구현 데이터
    [SerializeField]
    StageSelects _data;
    public static StageSelects data
    {
        get { return manager._data; }
    }

    private void Awake()
    {
        Init();
    }

    //초기화 함수
    public void Init()
    {
        // 중복검사
        var objs = FindObjectsOfType<StageSelectManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one StageSelectManager");
            Destroy(gameObject);
            return;
        }

        // 전역변수에 인스턴스 대입.
        manager = this;
    }

    // 스테이지 정보 표시
    public static void MakeSelector()
    {
        GameObject selector = Instantiate(manager.selectors.list[data.list[Save.GetSave().stageSelectorDataIndex].selectorIndex]);
        RectTransform rect = selector.GetComponent<RectTransform>();
        rect.SetParent(manager.gameObject.transform);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.localScale = new Vector3(1, 1, 1);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        selector.GetComponent<StageSelector>().Init();
    }
}
