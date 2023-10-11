using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 세팅 파일을 json 파일로 저장하는 클래스
[SerializeField]
public class SettingsToJson
{
    // 옵션 파일 이름과 확장자
    public static readonly string settingFileName = "/Settings.json";

    // 옵션 파일이 존재하는지 여부
    public static bool exist
    {
        get
        {
            FileInfo check = new FileInfo(Application.streamingAssetsPath + settingFileName);
            return check.Exists;
        }
    }

    // 구성요소
    public int ratioIndex, presetIndex, languageIndex, controlTypeIndex;
    public bool fixPreset, fullscreen;
    public float bgmVolume, sfxVolume;

    // 기존 옵션파일을 가져오는 시도
    public static SettingsToJson GetSettings()
    {
        if (exist)
            return JsonUtility.FromJson<SettingsToJson>(File.ReadAllText(Application.streamingAssetsPath + settingFileName));

        // 옵션파일이 없는 경우
        SettingsToJson settingJson = new SettingsToJson();
        settingJson.bgmVolume = 0.75f;
        settingJson.sfxVolume = 0.75f;

        settingJson.presetIndex = Option.defaultTitlePresetIndex;
        settingJson.languageIndex = 0;
        settingJson.ratioIndex = 0;
        settingJson.controlTypeIndex = 0;

        settingJson.fixPreset = false;
        settingJson.fullscreen = false;

        ApplySettings(settingJson);

        return settingJson;
    }

    // 변경된 설정값을 저장
    public static void ApplySettings(SettingsToJson changes)
    {
        EssentialManager.settings = changes;
        File.WriteAllText(Application.streamingAssetsPath + settingFileName, JsonUtility.ToJson(changes, true));
    }
}
