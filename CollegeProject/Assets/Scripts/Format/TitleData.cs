using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 화면을 구성하기 위한 데이터
[CreateAssetMenu(fileName = "Title(Name)", menuName = "menu/One Title preset data", order = 1)]
public class TitleData : ScriptableObject
{
    //각 화면을 표시할 인덱스
    public int titleIndex, optionIndex, galleryIndex;

    //화면 프리셋의 이름
    public string presetName;

    //배경화면, 배경음
    public int backgroundIndex;

    //게임 일시정지화면
    public int pauseIndex;

    //게임 클리어 UI
    public int clearIndex;

    //게임 오버 UI
    public int overIndex;
}
