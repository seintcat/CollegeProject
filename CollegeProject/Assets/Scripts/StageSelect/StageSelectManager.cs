using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ����â ������
public class StageSelectManager : MonoBehaviour
{
    //������ ����Ʈ
    [SerializeField]
    ObjList selectors;

    //�ڱ� �ڽ� ������Ʈ�� �������� ���
    private static StageSelectManager manager;

    //���� ������
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

    //�ʱ�ȭ �Լ�
    public void Init()
    {
        // �ߺ��˻�
        var objs = FindObjectsOfType<StageSelectManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one StageSelectManager");
            Destroy(gameObject);
            return;
        }

        // ���������� �ν��Ͻ� ����.
        manager = this;
    }

    // �������� ���� ǥ��
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
