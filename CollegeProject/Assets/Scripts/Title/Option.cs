using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �ɼ�ȭ���� �����ϴ� �߻� Ŭ����
public abstract class Option : MonoBehaviour
{
    //Ȯ��â
    GameObject dialog;

    //UI ������ ����Ʈ
    TitlePreset titlePreset;

    //ȭ�� ������
    Display display;

    // ��� ����
    Language language;
    [SerializeField]
    TextManager languageText;

    // �Ҹ� ���� ����
    Volume volume;

    // ��Ʈ�� ����
    ControlType control;

    // �ɼ� �⺻ ��Ӵٿ� ���
    [SerializeField]
    TMP_Dropdown controlType, titleTheme, aspectRaitio, languages;
    //�ɼ� �⺻ üũ�ڽ� ���
    [SerializeField]
    Toggle fullScreen;
    // ��� ���� ������ �̹���
    [SerializeField]
    Image flagImage;
    // ���� ���� �����̴� 
    [SerializeField]
    Slider bgmVol;
    [SerializeField]
    Slider sfxVol;
    // ����Ÿ�� �̹���
    [SerializeField]
    Image controlImage;

    // Ÿ��Ʋȭ�� ������ ��� ���� ���� �ε���
    Dictionary<int, int> titleThemeIndex;

    // ���̺갡 ���� �� �⺻ Ÿ��Ʋȭ�� ������ �ε���
    public static readonly int defaultTitlePresetIndex = 1;

    //�ʱ�ȭ �Լ�, �̹� ����Ǿ��ִ� �� �ɼ� ���� �ҷ�����
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

        // Ÿ��Ʋ ȭ�� ������ �ʱ�ȭ
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

        // ȭ����� �ʱ�ȭ
        names.Clear();
        foreach (Ratio ratio in display.ratio)
            names.Add(ratio.optionName);

        aspectRaitio.ClearOptions();
        aspectRaitio.AddOptions(names);
        aspectRaitio.value = settings.ratioIndex;

        // ��üȭ�� ���� �ݿ�
        fullScreen.SetIsOnWithoutNotify(settings.fullscreen);

        names.Clear();
        for (int i = 0; i < language.icons.Count; i++)
            names.Add(languageText.GetContext(i).text);
        languages.ClearOptions();

        languages.AddOptions(names);
        languageText.ApplyStyle();

        languages.value = settings.languageIndex;
        SetLanguageFlag();

        // ���� �ɼ� �ʱ�ȭ
        bgmVol.minValue = 0f;
        bgmVol.maxValue = 2f;
        bgmVol.value = volume.bgm;
        sfxVol.minValue = 0f;
        sfxVol.maxValue = 2f;
        sfxVol.value = volume.sfx;

        // ����Ÿ�� �ʱ�ȭ
        names.Clear();
        foreach (Control val in control.list)
            names.Add(val._name);
        controlType.ClearOptions();
        controlType.AddOptions(names);

        controlType.value = settings.controlTypeIndex;
        SetControlImage();
    }

    //ȭ�� ��Ȱ��ȭ
    public virtual void Exit()
    {
        if (TitleManager.usable)
            TitleManager.ReturnTitle();
        if (InGameUIManager.usable)
            InGameUIManager.ButtonLock(false);

        gameObject.SetActive(false);
    }

    //�ɼ� ����
    public virtual void Apply()
    {
        bool makeTitle = false;

        EssentialManager.Loading(0.3f);

        SettingsToJson settingChanges = SettingsToJson.GetSettings();

        // ȭ�����, ��üȭ�� ���� ����
        settingChanges.ratioIndex = aspectRaitio.value;
        settingChanges.fullscreen = fullScreen.isOn;

        // Ÿ��Ʋȭ�� Ʋ ����
        if(settingChanges.presetIndex != titleThemeIndex[titleTheme.value])
        {
            settingChanges.presetIndex = titleThemeIndex[titleTheme.value];
            makeTitle = true;
        }

        // ��� ����
        settingChanges.languageIndex = languages.value;

        // ���� ����
        settingChanges.bgmVolume = bgmVol.value;
        settingChanges.sfxVolume = sfxVol.value;

        // ����Ÿ�� ����
        settingChanges.controlTypeIndex = controlType.value;

        // ������ ����
        SettingsToJson.ApplySettings(settingChanges);

        // ���� �������� ����
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

    // ��� �ɼ��� �����Ͽ��� �� ǥ�õǴ� ���� �̹��� ����
    public virtual void SetLanguageFlag()
    {
        flagImage.sprite = language.icons[languages.value];
    }

    // ����Ÿ�� �̹��� ����
    public virtual void SetControlImage()
    {
        controlImage.sprite = control.list[controlType.value].image;
    }
}
