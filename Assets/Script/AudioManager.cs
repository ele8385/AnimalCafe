using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // 싱글턴 인스턴스

    public AudioSource musicSource; // 배경음악을 위한 AudioSource
    public AudioSource sfxSource; // 효과음을 위한 AudioSource

    private void Awake()
    {
        // 싱글턴 패턴 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 배경음악 재생 메소드
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true; // 배경음악은 루프
        musicSource.Play();
    }

    public void PlaySFX(string sourceName)
    {
        GameObject Source = transform.Find(sourceName).gameObject;
        if(Source != null)
        {
            Source.gameObject.GetComponent<AudioSource>().Play();
        }
        else { Debug.Log("효과음 없음"); }

    }

    public void StopSFX(string sourceName)
    {
        GameObject Source = transform.Find(sourceName).gameObject;
        if (Source != null)
        {
            Source.gameObject.GetComponent<AudioSource>().Stop();
        }
        else { Debug.Log("효과음 없음"); }

    }

}
