using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Button;
using UnityEngine.UI;
using TMPro;
using System;

// 스테이지 진행 도중 발생하는 상황 관련 컴포넌트
public abstract class InGameMenu : MonoBehaviour
{
    //확인창 구성요소
    [SerializeField]
    Confirm confirmWindow;

    //게임 오버 이미지
    [SerializeField]
    Image gameoverImage;
    public Sprite gameover
    {
        set
        {
            if(gameoverImage != null) 
                gameoverImage.sprite = value; 
        }
    }

    //진행정도 배경 이미지
    [SerializeField]
    Image progressBarImage;
    public Sprite progressBar
    {
        set
        {
            if (progressBarImage != null)
                progressBarImage.sprite = value;
        }
    }

    //진행정도 표시 이미지
    [SerializeField]
    Image progressIconImage;
    public Sprite progressIcon
    {
        set
        {
            if (progressIconImage != null)
                progressIconImage.sprite = value;
        }
    }

    //진행정도
    public float progress
    {
        set
        {
            if (progressIconImage != null)
            {
                RectTransform rect = progressIconImage.gameObject.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(value, 0f);
                rect.anchorMax = new Vector2(value, 0f);
                rect.pivot = new Vector2(0.5f, 0f);
            }
        }
    }

    //승리 일반대사, 패배 대사 구간
    [SerializeField]
    TextManager nomalText;

    //승리 분기점 대사 구간
    [SerializeField]
    TextManager hintText;

    //잠금 및 잠금해제동작에 쓰이는 버튼들 목록
    [SerializeField]
    List<Button> buttons;

    // 해당 스테이지 시간 경과 표시용
    [SerializeField]
    ContextList thisStageTime;

    // 전체 게임 진행시간 표기용
    [SerializeField]
    ContextList totalTime;

    //초기화 함수
    public virtual void Init(RectTransform parent = null)
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        if (parent != null)
            rect.SetParent(parent);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        rect.localScale = new Vector3(1f, 1f, 1f);
        Exit();
    }

    //진행중인 스테이지 재시작
    public virtual void Restart()
    {
        Time.timeScale = 1;
        ToolTipManager.Clear();
        EssentialManager.SceneSelect(3);
    }

    //게임 계속 진행
    public virtual void Resume()
    {
        Exit();
    }

    //환경설정
    public virtual void Option()
    {
        ButtonLock(true);
        InGameUIManager.Option();
    }

    //스테이지 선택 화면으로 이동
    public virtual void GoBack()
    {
        Time.timeScale = 1;
        ToolTipManager.Clear();
        EssentialManager.SceneSelect(2);
    }

    //승리 동작 처리
    public virtual void StageClear(ContextList hint = null, int stageSelectDataIndex = -1)
    {
        gameObject.SetActive(true);

        // 게임 시간 표시
        long time = EssentialManager.playTime;
        Save save = Save.GetSave();
        save.playTime = time;
        Save.ApplySave(save);
        string gameTime = thisStageTime.lang[EssentialManager.settings.languageIndex].text;
        gameTime += DateTime.FromBinary(time).ToString("HH.mm.ss.ff");
        gameTime += "\n\n";
        gameTime += totalTime.lang[EssentialManager.settings.languageIndex].text;
        gameTime += DateTime.FromBinary(save.playTime).ToString("HH.mm.ss.ff");

        nomalText.onlyText = gameTime;

        // 분기 표시
        if (hint == null)
            hintText.gameObject.SetActive(false);
        else
        {
            hintText.gameObject.SetActive(true);
            hintText.ApplyText(hint);
        }

        // 진행바 표시
        int indexReal;
        if (stageSelectDataIndex < 0)
            indexReal = save.stageSelectorDataIndex;
        else
            indexReal = stageSelectDataIndex;
        StageSelectData data = InGameUIManager.stageSelectDatas.list[indexReal];

        if (data.progressBackground != null)
        {
            progressBarImage.sprite = data.progressBackground;
            progressBarImage.type = Image.Type.Sliced;
        }
        else
            progressBarImage.color = new Color(0, 0, 0, 0);

        if (data.progressPointer != null)
        {
            progressIconImage.sprite = data.progressPointer;
            progress = data.progress;
        }
        else
            progressIconImage.color = new Color(0, 0, 0, 0);
    }

    //패배 동작 처리
    public virtual void GameOver(Sprite sprite, ContextList context)
    {
        gameObject.SetActive(true);
        gameover = sprite;
        nomalText.ApplyText(context);
        FilterManager.Off();
        FilterManager.Play(14);
    }

    //버튼들 활성화 / 비활성화
    public virtual void ButtonLock(bool btnLock)
    {
        if (buttons != null && buttons.Count > 0)
            foreach (Button btn in buttons)
                btn.interactable = !btnLock;
    }

    //화면 비활성화
    public virtual void Exit()
    {
        Time.timeScale = 1;
        ButtonLock(false);
        gameObject.SetActive(false);
    }

    // 확인창에 이벤트 붙여 실행
    public virtual void SetDialog(int index)
    {
        ButtonLock(true);
        confirmWindow.SetDialog(index);
    }
}
