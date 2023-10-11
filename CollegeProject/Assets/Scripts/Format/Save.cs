using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 게임 저장 파일
public class Save 
{
    public static readonly string fileName = "/Save.json";

    // 세이브 파일이 존재하는지 여부
    public static bool exist
    {
        get
        {
            FileInfo check = new FileInfo(Application.streamingAssetsPath + fileName);
            return check.Exists;
        }
    }

    // 타이틀 화면 해금여부
    [SerializeField]
    List<bool> _titlePresetUnlock;
    public List<bool> titlePresetUnlock { get { return _titlePresetUnlock; } }

    //실제 스테이지 인덱스
    [SerializeField]
    int _stageIndex;
    public int stageIndex { get { return _stageIndex; } }

    //스테이지 선택창 인덱스
    [SerializeField]
    int _stageSelectorDataIndex;
    public int stageSelectorDataIndex { get { return _stageSelectorDataIndex; } }

    //게임 플레이타임
    [SerializeField]
    long _playTime;
    public long playTime
    {
        get { return _playTime; }
        set { _playTime += value; }
    }

    //게임 시작시간
    [SerializeField]
    long _startTime;
    public long startTime { get { return _startTime; } }

    //재생할 컷신 인덱스
    [SerializeField]
    int _cutscene;
    public int cutscene
    {
        get
        {
            int value = _cutscene;
            _cutscene = -1;
            return value;
        }
        set { _cutscene = value; }
    }

    //새로운 세이브파일을 설정
    public static Save NewSave()
    {
        Save save = new Save();
        
        // 타이틀화면 해금 데이터
        save._titlePresetUnlock = new List<bool>();
        for (int i = 0; i < EssentialManager.titlePresets.list.Count; i++)
            save._titlePresetUnlock.Add(false);
        save._titlePresetUnlock[Option.defaultTitlePresetIndex] = true;

        // 스테이지 인덱스, 스테이지 선택창 인덱스
        save._stageIndex = 0;
        save._stageSelectorDataIndex = 0;

        // 게임 시간 설정
        save._playTime = 0;
        save._startTime = DateTime.Now.Ticks;

        // 컷신 재생 설정
        save._cutscene = 0;

        File.WriteAllText(Application.streamingAssetsPath + fileName, JsonUtility.ToJson(save, true));
        return save;
    }

    // 기존 세이브파일을 가져오는 시도
    public static Save GetSave()
    {
        if (exist)
            return JsonUtility.FromJson<Save>(File.ReadAllText(Application.streamingAssetsPath + fileName));

        return NewSave();
    }

    // 변경된 세이브파일을 저장
    public static void ApplySave(Save save)
    {
        File.WriteAllText(Application.streamingAssetsPath + fileName, JsonUtility.ToJson(save, true));
    }

    public static void UnlockPreset(int index)
    {
        Save save = GetSave();
        if (save._titlePresetUnlock.Count > index)
        {
            save._titlePresetUnlock[index] = true;
            ApplySave(save);
        }
    }
}
