using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ���� ���� �ʿ��� UI�� �����ϴ� ������Ʈ
public class InGameUIManager : MonoBehaviour
{
    //���� ȭ�� ����Ʈ
    [SerializeField]
    ObjList optionList, pauseList, clearList, overList;

    //������ ȭ�� ���� ������Ʈ
    InGameMenu gameOver, stageClear, pause;

    //���� �ɼ� ȭ��
    Option option;

    //������ ȭ���� ǥ���ϱ� ���� ��ġ
    [SerializeField]
    RectTransform UIPos, menuPos, optionPos;

    static InGameUIManager manager;
    public static bool usable { get { return (manager != null); } }
    public static bool paused = false;

    //�������� Ŭ���� ������ ���� ������
    [SerializeField]
    StageSelects _stageSelectDatas;
    public static StageSelects stageSelectDatas
    {
        get
        {
            if (manager == null)
                return null;

            return manager._stageSelectDatas;
        }
    }

    private void Awake()
    {
        Init();
    }

    //�ʱ�ȭ �Լ�
    public void Init()
    {
        // �ߺ��˻�
        var objs = FindObjectsOfType<InGameUIManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one InGameUIManager");
            Destroy(gameObject);
            return;
        }

        // ���������� �ν��Ͻ� ����.
        manager = this;
    }

    //UIȭ�� ����
    public static void MakeUI()
    {
        if (manager != null)
            manager.MakeUI_();
    }
    public void MakeUI_()
    {
        if (gameOver != null)
            Destroy(gameOver.gameObject);
        if (stageClear != null)
            Destroy(stageClear.gameObject);
        if (pause != null)
            Destroy(pause.gameObject);
        if (option != null)
            Destroy(option.gameObject);

        pause = Instantiate(pauseList.list[EssentialManager.usingTitlePreset.pauseIndex]).GetComponent<InGameMenu>();
        pause.Init(menuPos);

        stageClear = Instantiate(clearList.list[EssentialManager.usingTitlePreset.clearIndex]).GetComponent<InGameMenu>();
        stageClear.Init(menuPos);

        gameOver = Instantiate(overList.list[EssentialManager.usingTitlePreset.overIndex]).GetComponent<InGameMenu>();
        gameOver.Init(menuPos);

        option = Instantiate(optionList.list[EssentialManager.usingTitlePreset.optionIndex]).GetComponent<Option>();
        option.Init(optionPos);
        option.Exit();
    }

    //�ɼ� â�� ����
    public static void Option()
    {
        if (manager != null)
            manager.Option_();
    }
    public void Option_()
    {
        if (option != null)
            option.Init(optionPos);
    }

    //�¸� ���� ó��
    public static void StageClear(ContextList hint = null, int stageSelectDataIndex = -1)
    {
        if (manager != null)
            manager.stageClear.StageClear(hint, stageSelectDataIndex);
    }

    //�й� ���� ó��
    public static void GameOver(Sprite sprite, ContextList context)
    {
        if (manager != null)
            manager.gameOver.GameOver(sprite, context);
    }

    //�Ͻ����� �޴� ó��
    public static void Pause()
    {
        if (manager != null)
            manager.Pause_();
    }
    public void Pause_()
    {
        Time.timeScale = 0;
        pause.ButtonLock(false);
        pause.gameObject.SetActive(true);
        paused = true;
        ToolTipManager.Hide(true);
    }

    public static void Resume()
    {
        if (manager != null)
            manager.Resume_();
    }
    public void Resume_()
    {
        paused = false;
        pause.gameObject.GetComponent<InGameMenu>().Resume();
        ToolTipManager.Hide(false);
    }

    // �ɼ�â�� ��ư ��� ����
    public static void ButtonLock(bool btnLock)
    {
        manager.pause.ButtonLock(btnLock);
    }
}
