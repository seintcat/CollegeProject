using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ü ������ �����ϴ� �Ŵ���
public class ToolTipManager : MonoBehaviour
{
    //���� ������� ����
    static List<int> playing = new List<int>();

    //�ѹ��̶� ���� ���� ����
    static Dictionary<int, ToolTip> cache = new Dictionary<int, ToolTip>();

    //������ �����ϴ� ��ġ
    [SerializeField]
    RectTransform toolTipPos;

    //���� ����Ʈ
    [SerializeField]
    ObjList frames;

    //���� ������ ����Ʈ
    [SerializeField]
    ToolTipDataList datas;

    //�ڱ� �ڽ� ������Ʈ�� �������� ���
    private static ToolTipManager manager;

    // �ʱ�ȭ �޼���
    void Init()
    {
        // �ߺ��˻�
        var objs = FindObjectsOfType<ToolTipManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one ToolTipManager");
            Destroy(gameObject);
            return;
        }

        manager = this;
    }

    //���� ���
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

    //���� ���� ����
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
