using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전체 툴팁을 관리하는 매니저
public class ToolTipManager : MonoBehaviour
{
    //현재 재생중인 툴팁
    static List<int> playing = new List<int>();

    //한번이라도 사용된 툴팁 모음
    static Dictionary<int, ToolTip> cache = new Dictionary<int, ToolTip>();

    //툴팁을 생성하는 위치
    [SerializeField]
    RectTransform toolTipPos;

    //툴팁 리스트
    [SerializeField]
    ObjList frames;

    //툴팁 데이터 리스트
    [SerializeField]
    ToolTipDataList datas;

    //자기 자신 컴포넌트를 전역으로 사용
    private static ToolTipManager manager;

    // 초기화 메서드
    void Init()
    {
        // 중복검사
        var objs = FindObjectsOfType<ToolTipManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one ToolTipManager");
            Destroy(gameObject);
            return;
        }

        manager = this;
    }

    //툴팁 재생
    public static void Show(int index)
    {
        if (manager == null)
            return;

        if (manager.gameObject.activeSelf == false)
            manager.gameObject.SetActive(true);

        ToolTipData data = manager.datas.list[index];
        if (cache.ContainsKey(data.frameIndex))
        {
            if (playing.Contains(data.frameIndex))
            {
                cache[data.frameIndex].Off(false);
                playing.Remove(data.frameIndex);
            }
        }
        else
        {
            GameObject toolTipObject = Instantiate(manager.frames.list[data.frameIndex]);
            ToolTip toolTip = toolTipObject.GetComponent<ToolTip>();

            toolTipObject.GetComponent<RectTransform>().SetParent(manager.toolTipPos);
            toolTip.Init();
            cache.Add(data.frameIndex, toolTip);
        }
        playing.Add(data.frameIndex);
        cache[data.frameIndex].Make(data);
    }

    //툴팁 전부 종료
    public static void Clear()
    {
        if (manager == null)
            return;

        foreach (int index in playing)
            cache[index].Off(false);

        playing.Clear();
    }

    public static void Hide(bool value)
    {
        if (manager == null)
            return;
        manager.gameObject.SetActive(!value);

        if (!value)
            for (int index = 0; index < playing.Count; index++)
                    if (cache.ContainsKey(index) && cache[index].gameObject.activeSelf == true)
                        cache[index].Resume();
    }

    private void Awake()
    {
        Init();
    }
}
