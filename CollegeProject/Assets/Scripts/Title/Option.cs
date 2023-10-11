using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 옵션화면을 구성하는 추상 클래스
public abstract class Option : MonoBehaviour
{
    //확인창
    GameObject dialog;

    //UI 프리셋 리스트
    TitlePreset titlePreset;

    //화면 설정값
    Display display;

    // 언어 설정
    Language language;
    [SerializeField]
    TextManager languageText;

    // 소리 볼륨 설정
    Volume volume;

    // 컨트롤 설정
    ControlType control;

    // 옵션 기본 드롭다운 목록
    [SerializeField]
    TMP_Dropdown controlType, titleTheme, aspectRaitio, languages;
    //옵션 기본 체크박스 목록
    [SerializeField]
    Toggle fullScreen;
    // 언어 설정 아이콘 이미지
    [SerializeField]
    Image flagImage;
    // 볼륨 설정 슬라이더 
    [SerializeField]
    Slider bgmVol;
    [SerializeField]
    Slider sfxVol;
    // 조작타입 이미지
    [SerializeField]
    Image controlImage;

    // 타이틀화면 프리셋 목록 값과 실제 인덱스
    Dictionary<int, int> titleThemeIndex;

    // 세이브가 없을 때 기본 타이틀화면 프리셋 인덱스
    public static readonly int defaultTitlePresetIndex = 1;

    //초기화 함수, 이미 저장되어있는 각 옵션 정보 불러오기
    public virtual void Init(RectTransform parent = null)
    {
        RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
        if(parent != null)
            rectTrans.SetParent(parent);
        rectTrans.offsetMin = new Vector2(0, 0);
        rectTrans.offsetMax = new Vector2(0, 0);
        rectTrans.localScale = new Vector3(1f, 1f, 1f);

        gameObject.SetActive(true);
        display = EssentialManager.display;
        titlePreset = EssentialManager.titlePresets;
        language = EssentialManager.languageData;
        volume = EssentialManager.volume;
        control = EssentialManager.controlType;

        SettingsToJson settings = SettingsToJson.GetSettings();
        Save save = null;
        if (Save.exist)
            save = Save.GetSave();

        // 타이틀 화면 프레임 초기화
        List<string> names = new List<string>();
        int dropdown = 0, index = 0, selected = -1;
        titleThemeIndex = new Dictionary<int, int>();
        if(save != null)
            foreach (TitleData frame in titlePreset.list)
            {
                if (save.titlePresetUnlock[index])
                {
                    names.Add(frame.presetName);
                    titleThemeIndex.Add(dropdown, index);

                    if (index == settings.presetIndex)
                        selected = dropdown;

                    dropdown++;
                }
                index++;
            }
        else
        {
            names.Add(titlePreset.list[defaultTitlePresetIndex].presetName);
            titleThemeIndex.Add(dropdown, index);
            selected = dropdown;
        }

        titleTheme.ClearOptions();
        titleTheme.AddOptions(names);
        titleTheme.value = selected;

        // 화면비율 초기화
        names.Clear();
        foreach (Ratio ratio in display.ratio)
            names.Add(ratio.optionName);

        aspectRaitio.ClearOptions();
        aspectRaitio.AddOptions(names);
        aspectRaitio.value = settings.ratioIndex;

        // 전체화면 여부 반영
        fullScreen.SetIsOnWithoutNotify(settings.fullscreen);

        names.Clear();
        for (int i = 0; i < language.icons.Count; i++)
            names.Add(languageText.GetContext(i).text);
        languages.ClearOptions();

        languages.AddOptions(names);
        languageText.ApplyStyle();

        languages.value = settings.languageIndex;
        SetLanguageFlag();

        // 볼륨 옵션 초기화
        bgmVol.minValue = 0f;
        bgmVol.maxValue = 2f;
        bgmVol.value = volume.bgm;
        sfxVol.minValue = 0f;
        sfxVol.maxValue = 2f;
        sfxVol.value = volume.sfx;

        // 조작타입 초기화
        names.Clear();
        foreach (Control val in control.list)
            names.Add(val._name);
        controlType.ClearOptions();
        controlType.AddOptions(names);

        controlType.value = settings.controlTypeIndex;
        SetControlImage();
    }

    //화면 비활성화
    public virtual void Exit()
    {
        if (TitleManager.usable)
            TitleManager.ReturnTitle();
        if (InGameUIManager.usable)
            InGameUIManager.ButtonLock(false);

        gameObject.SetActive(false);
    }

    //옵션 적용
    public virtual void Apply()
    {
        bool makeTitle = false;

        EssentialManager.Loading(0.3f);

        SettingsToJson settingChanges = SettingsToJson.GetSettings();

        // 화면비율, 전체화면 여부 적용
        settingChanges.ratioIndex = aspectRaitio.value;
        settingChanges.fullscreen = fullScreen.isOn;

        // 타이틀화면 틀 적용
        if(settingChanges.presetIndex != titleThemeIndex[titleTheme.value])
        {
            settingChanges.presetIndex = titleThemeIndex[titleTheme.value];
            makeTitle = true;
        }

        // 언어 적용
        settingChanges.languageIndex = languages.value;

        // 볼륨 적용
        settingChanges.bgmVolume = bgmVol.value;
        settingChanges.sfxVolume = sfxVol.value;

        // 조작타입 적용
        settingChanges.controlTypeIndex = controlType.value;

        // 설정값 저장
        SettingsToJson.ApplySettings(settingChanges);

        // 이후 변동사항 적용
        var objs = FindObjectsOfType<TextManager>();
        foreach (TextManager manager in objs)
            manager.ApplyText();

        AudioManager.VolumeUpdate();

        EssentialManager.SetResolution();

        if (makeTitle)
        {
            if (TitleManager.usable)
                TitleManager.MakeTitle();
            if (InGameUIManager.usable)
            {
                InGameUIManager.MakeUI();
                InGameUIManager.Pause();
            }
        }

        Time.timeScale = 1;
        if (InGameUIManager.usable)
            InGameUIManager.Resume();
        Exit();
    }

    // 언어 옵션을 변경하였을 시 표시되는 국가 이미지 변경
    public virtual void SetLanguageFlag()
    {
        flagImage.sprite = language.icons[languages.value];
    }

    // 조작타입 이미지 변경
    public virtual void SetControlImage()
    {
        controlImage.sprite = control.list[controlType.value].image;
    }
}
