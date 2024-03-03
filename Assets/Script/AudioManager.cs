using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // �̱��� �ν��Ͻ�

    public AudioSource musicSource; // ��������� ���� AudioSource
    public AudioSource sfxSource; // ȿ������ ���� AudioSource

    private void Awake()
    {
        // �̱��� ���� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ �ı����� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ������� ��� �޼ҵ�
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true; // ��������� ����
        musicSource.Play();
    }

    public void PlaySFX(string sourceName)
    {
        GameObject Source = transform.Find(sourceName).gameObject;
        if(Source != null)
        {
            Source.gameObject.GetComponent<AudioSource>().Play();
        }
        else { Debug.Log("ȿ���� ����"); }

    }

    public void StopSFX(string sourceName)
    {
        GameObject Source = transform.Find(sourceName).gameObject;
        if (Source != null)
        {
            Source.gameObject.GetComponent<AudioSource>().Stop();
        }
        else { Debug.Log("ȿ���� ����"); }

    }

}
