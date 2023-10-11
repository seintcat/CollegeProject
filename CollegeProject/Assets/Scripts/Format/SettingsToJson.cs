using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// ���� ������ json ���Ϸ� �����ϴ� Ŭ����
[SerializeField]
public class SettingsToJson
{
    // �ɼ� ���� �̸��� Ȯ����
    public static readonly string settingFileName = "/Settings.json";

    // �ɼ� ������ �����ϴ��� ����
    public static bool exist
    {
        get
        {
            FileInfo check = new FileInfo(Application.streamingAssetsPath + settingFileName);
            return check.Exists;
        }
    }

    // �������
    public int ratioIndex, presetIndex, languageIndex, controlTypeIndex;
    public bool fixPreset, fullscreen;
    public float bgmVolume, sfxVolume;

    // ���� �ɼ������� �������� �õ�
    public static SettingsToJson GetSettings()
    {
        if (exist)
            return JsonUtility.FromJson<SettingsToJson>(File.ReadAllText(Application.streamingAssetsPath + settingFileName));

        // �ɼ������� ���� ���
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

    // ����� �������� ����
    public static void ApplySettings(SettingsToJson changes)
    {
        EssentialManager.settings = changes;
        File.WriteAllText(Application.streamingAssetsPath + settingFileName, JsonUtility.ToJson(changes, true));
    }
}
