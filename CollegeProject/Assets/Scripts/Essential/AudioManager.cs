using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 하나의 오디오 소스를 재생하는 프리팹
    public GameObject audioPrefab;

    // 현재 재생되는 오디오 소스 모음
    private static Dictionary<int, GameObject> objects;

    // 현재 재생되는 오디오 볼륨 모음
    private static Dictionary<GameObject, float> volumes;

    // 종료된 후 재생 대기중인 오디오 소스 프리팹
    private static List<GameObject> soundQueue;

    // objects를 관리하기 위한 인덱스 오프셋 값
    private static int lastIndex;
    static List<int> unusingIndex;

    // 자기 자신 컴포넌트를 전역으로 사용
    private static AudioManager manager;
    
    // 초기화 메서드
    void Init() 
    {
        // 중복검사
        var objs = FindObjectsOfType<AudioManager>();
        if (objs.Length != 1)
        {
            Debug.Log("error: you can use only one AudioManager");
            Destroy(gameObject);
            return;
        }

        // 전역변수에 인스턴스 대입.
        manager = this;

        lastIndex = 0;
        objects = new Dictionary<int, GameObject>();
        soundQueue = new List<GameObject>();
        unusingIndex = new List<int>();
        volumes = new Dictionary<GameObject, float>();
    }

    // 오디오소스를 재생하고, 재생되는 결과를 인덱스로 반환
    public static int Play(AudioClip clip, bool isLoop, float _volume)
    {
        AudioSource source;
        GameObject sound;

        if (soundQueue.Count > 0)
        {
            sound = soundQueue[0];
            soundQueue.RemoveAt(0);
        }
        else
            sound = Instantiate(manager.audioPrefab);

        sound.transform.SetParent(manager.gameObject.transform);

        source = sound.GetComponent<AudioSource>();

        source.clip = clip;
        source.loop = isLoop;

        if (!volumes.ContainsKey(sound))
            volumes.Add(sound, _volume);
        else
            volumes[sound] = _volume;

        // 오디오 소스의 범위 값은 0.0f ~ 1.0f, 글로벌 음량 설정은 0.0f ~ 2.0f임
        if (isLoop)
            source.volume = _volume * EssentialManager.settings.bgmVolume;
        else
            source.volume = _volume * EssentialManager.settings.sfxVolume;

        source.Play();

        if(unusingIndex.Count > 0)
        {
            int value = unusingIndex[0];

            objects.Add(value, sound);
            unusingIndex.RemoveAt(0);
            return value;
        }
        else
        {
            objects.Add(lastIndex, sound);
            lastIndex++;
            return lastIndex - 1;
        }
    }

    // 인덱스에 해당하는 오디오 종료
    public static void Off(int _index)
    {
        if (objects.TryGetValue(_index, out GameObject sound))
        {
            objects.Remove(_index);
            sound.GetComponent<AudioSource>().Stop();
            soundQueue.Add(sound);
            unusingIndex.Add(_index);
        }
    }

    // 볼륨을 다시 적용
    public static void VolumeUpdate()
    {
        foreach(int index in objects.Keys)
        {
            AudioSource source = objects[index].GetComponent<AudioSource>();
            if (objects[index].activeSelf)
            {
                if (source.loop)
                    source.volume = volumes[objects[index]] * EssentialManager.settings.bgmVolume;
                else
                    source.volume = volumes[objects[index]] * EssentialManager.settings.sfxVolume;
            }
        }
    }

    private void Awake()
    {
        Init();
    }
}
