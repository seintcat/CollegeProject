using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 시작화면 전체를 관리하는 컴포넌트
public class TitleManager : MonoBehaviour
{
    //각각의 화면을 표시하기 위한 위치
    [SerializeField]
    RectTransform titlePos, otherPos;

    //각각의 화면 리스트
    [SerializeField]
    ObjList titleList, optionList;

    //자기 자신 컴포넌트를 전역으로 사용
    private static TitleManager manager;

    //현재 타이틀 화면
    Title title;
    //현재 옵션 화면
    Option option;

    // 사용 가능한지 여부
    public static bool usable = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        Init();
    }

    //초기화 함수
    public void Init()
    {
        // 중복검사
        var objs = FindObjectsOfType<TitleManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one TitleManager");
            Destroy(gameObject);
            return;
        }

        // 전역변수에 인스턴스 대입.
        manager = this;
        usable = true;
    }

    //타이틀화면을 다시 활성화시킴
    public static void ReturnTitle()
    {
        if(manager.title != null)
            manager.title.ButtonLock(false);
    }

    //타이틀화면 구성
    public static void MakeTitle()
    {
        manager.MakeTitle_();
    }
    public void MakeTitle_()
    {
        if (title != null)
            Destroy(title.gameObject);
        if (option != null)
            Destroy(option.gameObject);

        TitleData index = EssentialManager.usingTitlePreset;
        GameObject titleObj = Instantiate(titleList.list[index.titleIndex]);
        GameObject optionObj = Instantiate(optionList.list[index.optionIndex]);

        title = titleObj.GetComponent<Title>();
        option = optionObj.GetComponent<Option>();

        title.Init(titlePos);
        option.Init(otherPos);

        option.Exit();

        BackgroundLayer.Play(index.backgroundIndex);
    }

    // 타이틀 화면에서 특정 창을 연다
    public static void Open(int index)
    {
        manager.Open_(index);
    }
    public void Open_(int index)
    {
        switch (index)
        {
            case 2:
                option.Init();
                break;
            default:
                break;
        }
    }
}
