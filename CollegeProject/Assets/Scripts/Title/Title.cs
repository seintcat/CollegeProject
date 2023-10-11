using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

// 타이틀 화면을 구성하는 추상 클래스
public abstract class Title : MonoBehaviour
{
    //컨티뉴, 새 게임, 옵션, 갤러리, 종료 버튼 순서로 연결됨
    [SerializeField]
    List<Button> buttons;

    //확인창 구성요소
    [SerializeField]
    Confirm confirmWindow;

    // 버튼들이 존재하는 위치를 담당하는 컴포넌트
    [SerializeField]
    VerticalLayoutGroup buttonField;

    // 초기화 코루틴
    static IEnumerator init;

    //게임을 이어한다
    public virtual void Continue()
    {
        EssentialManager.SceneSelect(2);
    }

    //새 게임
    public virtual void NewGame()
    {
        Save.NewSave();
        EssentialManager.SceneSelect(2);
    }

    //겔러리 화면
    public virtual void Gallery()
    {
        EssentialManager.SceneSelect(4);
    }

    //환경설정
    public virtual void Option()
    {
        TitleManager.Open(2);
        ButtonLock(true);
    }

    //게임 종료
    public virtual void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    //버튼들 활성화 / 비활성화
    public virtual void ButtonLock(bool value)
    {
        foreach (Button btn in buttons)
            btn.interactable = !value;
    }

    // 확인창 체크
    public virtual void CheckDialog(int index)
    {
        ButtonLock(true);

        switch (index)
        {
            case 0:
                if (!Save.exist)
                    NewGame();
                else
                    SetDialog(index);
                break;
            case 1:
                SetDialog(index);
                break;
            default:
                break;
        }
    }

    // 확인창에 이벤트 붙여 실행
    public virtual void SetDialog(int index)
    {
        ButtonLock(true);

        confirmWindow.GetComponent<Confirm>().SetDialog(index);
    }

    // 타이틀 초기화
    public virtual void Init(RectTransform parent = null)
    {
        RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
        if (parent != null)
            rectTrans.SetParent(parent);
        rectTrans.offsetMin = new Vector2(0, 0);
        rectTrans.offsetMax = new Vector2(0, 0);
        rectTrans.localScale = new Vector3(1f, 1f, 1f);

        // UI의 버튼크기 조절
        buttonField.childControlHeight = true;
        buttonField.childForceExpandHeight = true;

        init = Init_();
        StartCoroutine(init);
    }
    public virtual IEnumerator Init_()
    {
        yield return new WaitForEndOfFrame();

        buttonField.childControlHeight = false;

        if (!Save.exist)
            buttons[0].gameObject.SetActive(false);

        StopCoroutine(init);
    }

}
