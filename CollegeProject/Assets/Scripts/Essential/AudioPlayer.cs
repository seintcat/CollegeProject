using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    int soundIndex;
    float delay;

    IEnumerator sound;
    // Start is called before the first frame update
    public void PlaySound(int index)
    {
        FilterSet filter;

        filter = FilterManager.GetFilter(index);
        soundIndex = AudioManager.Play(filter.sound, filter.isLoop, filter.volume) + 0;
        sound = AudioOff(delay);
        StartCoroutine(sound);

    }

    public void SetDelay(float _delay) 
    {
        delay = _delay;
    }

    IEnumerator AudioOff(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if(soundIndex > -1)
            {
                AudioManager.Off(soundIndex);
                soundIndex = -1;
            }
            StopCoroutine(sound);
        }
    }

    public void Off()
    {
        AudioManager.Off(soundIndex);
        StartCoroutine(sound);
    }
}
