using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 내내 사라지지 않는 오브젝트 모음
public class EssentialManager : MonoBehaviour
{
    //로딩 화면을 표시하기 위한 위치
    [SerializeField]
    Transform loadingPos;

    //로딩 화면 컴포넌트
    LoadingScreen loadingScreen;

    //자기 자신 컴포넌트를 전역으로 사용
    private static EssentialManager manager;
    public static GameObject managerObj
    {
        get { return manager.gameObject; }
    }

    //로딩화면 리스트
    [SerializeField]
    ObjList loadingList;

    // 로딩창 인덱스값
    static int loadingIndex = 0;
    // 로딩 오프셋 시간
    WaitForSeconds loadingOffset;
    // 로딩창 작동시간 계산
    static List<float> loadTime = new List<float>();
    // 로딩창 끄는 코루틴
    static IEnumerator loadOffCoroutine;

    // 화면 설정값, 전역으로 참조 가능
    [SerializeField]
    Display _display;
    public static Display display
    {
        get { return manager._display; }
    }

    // 게임 전체화면을 실행시키기 위한 전체화면 크기
    Vector2Int fullscreen;

    // 각종 지문에 쓰일 스타일시트
    [SerializeField]
    TMP_StyleSheet _textStyleSheet;
    public static TMP_StyleSheet textStyleSheet
    {
        get
        {
            if (manager != null && manager._textStyleSheet != null)
                return manager._textStyleSheet;

            return null;
        }
    }

    //화면을 구성하기 위한 데이터, 전역으로 참조 가능
    [SerializeField]
    TitlePreset _titlePresets;
    public static TitlePreset titlePresets
    {
        get
        {
            if (manager != null && manager._titlePresets != null)
                return manager._titlePresets;

            return null;
        }
    }
    public static TitleData usingTitlePreset
    {
        get
        {
            if (manager != null && manager._titlePresets != null)
                return manager._titlePresets.list[settings.presetIndex];

            return null;
        }
    }

    //언어 설정을 위한 데이터, 전역으로 참조 가능
    [SerializeField]
    Language _languageData;
    public static Language languageData
    {
        get
        {
            if (manager != null && manager._languageData != null)
                return manager._languageData;

            return null;
        }
    }

    //음량 설정
    [SerializeField]
    Volume _volume;
    public static Volume volume
    {
        get
        {
            Volume newVolume = new Volume();
            SettingsToJson settingData = SettingsToJson.GetSettings();
            newVolume.bgm = settingData.bgmVolume;
            newVolume.sfx = settingData.sfxVolume;
            return newVolume;
        }
    }

    //컨트롤 설정
    [SerializeField]
    ControlType _controlType;
    public static ControlType controlType
    {
        get
        {
            if (manager != null && manager._controlType != null)
                return manager._controlType;

            return null;
        }
    }

    // 환경설정 값
    public static SettingsToJson settings;

    // 신 이름들
    static readonly string titleSceneName = "Title";
    static readonly string stageSelectSceneName = "StageSelect";
    static readonly string stageSceneName = "MainStage";
    static readonly string gallerySceneName = "Gallery";
    // 신을 이동할 때 사용되는 코루틴
    static IEnumerator sceneMoveCoroutine;

    // 스테이지 시작부터의 플레이타임
    static long _playTime;
    public static long playTime { get { return DateTime.Now.Ticks - _playTime; } }

    //갤러리 화면 리스트
    [SerializeField]
    GalleryData _gallery;
    public static GalleryData gallery
    {
        get
        {
            if (manager != null)
                return manager._gallery;

            return null;
        }
    }

    // 컷신 재생 전에 어느 신에 있었는지 인덱스 저장
    static int sceneIndex = 0;


    void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 초기화 메서드
    void Init()
    {
        // 중복검사
        var objs = FindObjectsOfType<EssentialManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one EssentialManager");
            Destroy(gameObject);
            return;
        }

        // 전역변수에 인스턴스 대입.
        manager = this;
        // 하드웨어 사이즈 추출
        fullscreen = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
        // 옵션 파일 로드
        settings = SettingsToJson.GetSettings();

        // 신 이동, 로딩창 호출, 해상도 맞추기
        SceneSelect(1);
    }

    // 로딩창 호출시 자동 호출되는 로딩창 끄는 코루틴
    IEnumerator LoadingOff()
    {
        while (true)
        {
            yield return null;
            int lastCheckedIndex = 0;
            float time = 0f;
            List<float> timeList = new List<float>();
            timeList.CopyTo(loadTime.ToArray());

            while (true)
            {
                if (loadTime.Count != timeList.Count)
                {
                    timeList.Clear();
                    foreach (float val in loadTime)
                        timeList.Add(val);
                }
                else
                    break;

                while (lastCheckedIndex < timeList.Count)
                {
                    yield return null;
                    if (time < timeList[lastCheckedIndex])
                        time = timeList[lastCheckedIndex];
                    lastCheckedIndex++;
                }

                loadingOffset = new WaitForSeconds(time);

                yield return loadingOffset;
            }

            loadingScreen.Off();
            StopCoroutine(loadOffCoroutine);
        }
    }

    // 로딩창을 조작하는 메서드
    public static void Loading(float time = -1f, int index = -1)
    {
        if (manager == null)
            return;

        if(time < 0f)
        {
            Debug.Log("time");
            return;
        }

        manager.Loading_(time, index);
    }
    public void Loading_(float time, int index)
    {
        if (index != -1 && loadingIndex != index)
        {
            if (loadingScreen != null)
                Destroy(loadingScreen.gameObject);

            loadingIndex = index;
        }

        if (loadingScreen == null)
        {
            GameObject loadings = Instantiate(loadingList.list[loadingIndex]);
            loadings.transform.SetParent(loadingPos);
            loadingScreen = loadings.GetComponent<LoadingScreen>();
        }

        if (!loadingScreen.gameObject.activeSelf)
            loadTime.Clear();

        loadingScreen.On();
        loadTime.Add(time);
        loadOffCoroutine = LoadingOff();
        StartCoroutine(loadOffCoroutine);
    }

    // 화면 해상도 및 전체화면 여부를 재설정
    public static void SetResolution()
    {
        if (manager != null)
            manager.SetResolution_();
    }
    public void SetResolution_()
    {
        Vector2Int margin = new Vector2Int((int)(fullscreen.x * _display.padding), (int)(fullscreen.y * _display.padding));
        Vector2Int raitio = _display.ratio[settings.ratioIndex].value;
        Vector2Int windowed = new Vector2Int();
        Vector2Int canvasSize = _display.ratio[settings.ratioIndex].pixel;

        // 제공받은 기본 해상도 값과 하드웨어의 스펙(모니터 픽셀 가로 세로 값)을 가지고 창화면의 크기를 결정
        // Width / Height 의 값이 더 작을 수록 같은 너비대비 높이의 값이 더 크다 - 이해를 위한 설명
        if (fullscreen.x / fullscreen.y <= raitio.x / raitio.y)
        {
            // 가로길이를 기준으로, 세로길이를 계산 = 위아래 빈공간(창모드)
            windowed.y = margin.y - (margin.y % raitio.y);
            windowed.x = (windowed.y / raitio.y) * raitio.x;
        }
        else
        {
            // 세로길이를 기준으로, 가로길이를 계산 = 좌우로 빈공간(창모드)
            windowed.x = margin.x - (margin.x % raitio.x);
            windowed.y = (windowed.x / raitio.x) * raitio.y;
        }

        //UI 크기를 맞추기(불안정할 수 있음)
        var objs = FindObjectsOfType<CanvasScaler>();
        //text.text = objs.Length.ToString();
        foreach (CanvasScaler scaler in objs)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = canvasSize;
            scaler.matchWidthOrHeight = 1f;
        }

        BackgroundLayer.ApplyCanvasSize();

        // 화면 구성방식이 창, 전체, 전체창 같은 방식이 있으니까 창, 전체 말고도 하나더 가용가능 하게 신경쓸예정 // 창화면, 전체화면만 만들시 bool값으로 수정
        // 제공된 창의 크기를 받아서 해상도를 스케일링하여 게임출력화면을 맞추는 부분
        //text.text = "monitor = " + fullscreen.ToString() + "\n" + "calculated = " + margin.ToString() + "\n" + "game = " + windowed.ToString() + "\n";
        Screen.SetResolution(windowed.x, windowed.y, settings.fullscreen);
    }

    // 특정 신으로 이동
    public static void SceneSelect(int index)
    {
        BackgroundLayer.Off();
        FilterManager.Off();
        DontDestroyOnLoad(manager.gameObject);
        sceneIndex = index;

        switch (index)
        {
            case 1:
                Loading(0.5f, loadingIndex);
                SceneManager.LoadScene(titleSceneName);
                break;
            case 2:
                Loading(0.5f, loadingIndex);
                SceneManager.LoadScene(stageSelectSceneName);
                break;
            case 3:
                Loading(0.5f, loadingIndex);
                SceneManager.LoadScene(stageSceneName);
                break;
            case 4:
                Loading(0.5f, loadingIndex);
                SceneManager.LoadScene(gallerySceneName);
                break;
            default:
                Debug.Log("Scene Load Error");
                break;
        }
        sceneMoveCoroutine = manager.SceneMove(index);
        manager.StartCoroutine(sceneMoveCoroutine);
    }
    // 신 이동 후 처리를 담당하는 코루틴
    public IEnumerator SceneMove(int index)
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByBuildIndex(index));
            SetResolution();
            switch (index)
            {
                case 1:
                    if(TitleManager.usable)
                        TitleManager.MakeTitle();
                    break;
                case 2:
                    StageSelectManager.MakeSelector();

                    Save save = Save.GetSave();
                    int cutsceneIndex = save.cutscene;
                    if (cutsceneIndex > -1)
                    {
                        ToolTipManager.Show(cutsceneIndex);
                        Save.ApplySave(save);
                    }

                    break;
                case 3:
                    Map.MakeMap();
                    if (InGameUIManager.usable)
                        InGameUIManager.MakeUI();
                    _playTime = DateTime.Now.Ticks;
                    break;
                case 4:
                    GalleryManager.MakeTitle();
                    break;
                default:
                    break;
            }
            StopCoroutine(sceneMoveCoroutine);
        }
    }
}
