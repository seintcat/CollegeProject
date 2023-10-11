using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이지 진행 도중 필요한 UI를 관리하는 컴포넌트
public class InGameUIManager : MonoBehaviour
{
    //각종 화면 리스트
    [SerializeField]
    ObjList optionList, pauseList, clearList, overList;

    //각각의 화면 구성 컴포넌트
    InGameMenu gameOver, stageClear, pause;

    //현재 옵션 화면
    Option option;

    //각각의 화면을 표시하기 위한 위치
    [SerializeField]
    RectTransform UIPos, menuPos, optionPos;

    static InGameUIManager manager;
    public static bool usable { get { return (manager != null); } }
    public static bool paused = false;

    //스테이지 클리어 구현을 위한 데이터
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

    //초기화 함수
    public void Init()
    {
        // 중복검사
        var objs = FindObjectsOfType<InGameUIManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one InGameUIManager");
            Destroy(gameObject);
            return;
        }

        // 전역변수에 인스턴스 대입.
        manager = this;
    }

    //UI화면 구성
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

    //옵션 창을 연다
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

    //승리 동작 처리
    public static void StageClear(ContextList hint = null, int stageSelectDataIndex = -1)
    {
        if (manager != null)
            manager.stageClear.StageClear(hint, stageSelectDataIndex);
    }

    //패배 동작 처리
    public static void GameOver(Sprite sprite, ContextList context)
    {
        if (manager != null)
            manager.gameOver.GameOver(sprite, context);
    }

    //일시정지 메뉴 처리
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

    // 옵션창의 버튼 잠금 제어
    public static void ButtonLock(bool btnLock)
    {
        manager.pause.ButtonLock(btnLock);
    }
}
