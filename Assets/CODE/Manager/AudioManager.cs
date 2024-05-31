using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;

    [SerializeField] AudioClip[] BGM;
    AudioSource bgmPlayer;

    [SerializeField] AudioClip[] Ui_SFX;

    Transform sfxTrs;

    Queue<AudioSource> audioQue = new Queue<AudioSource>();
    int channel = 32;
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
        sfxTrs = transform.Find("SFX");
        bgmPlayer =transform.Find("BGMPlayer").GetComponent<AudioSource>();

        for (int index = 0; index < channel; index++) 
        {
            MakeSoundClip();
        }
        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MakeSoundClip()
    {
        AudioSource audioObj = new GameObject("SFX").AddComponent<AudioSource>();
        audioObj.playOnAwake = false;
        audioObj.transform.SetParent(sfxTrs);
        audioObj.gameObject.SetActive(false);
        audioQue.Enqueue(audioObj);
    }

    /// <summary>
    /// SFX 재생기 (UI 효과음)
    /// </summary>
    /// <param name="index"> 0:로그인씬 탭투스크린<br/>1:</param>
    public void PlaySFX(int index, float Volume)
    {
        if(audioQue.Count <= 0)
        {
            MakeSoundClip();
        }

        StartCoroutine(SFX_SoundPlay(index, Volume)); 
    }

    IEnumerator SFX_SoundPlay(int index, float Volume)
    {
        AudioSource obj = audioQue.Dequeue();
        obj.volume = Volume;
        obj.clip = Ui_SFX[index];
        obj.gameObject.SetActive(true);
        obj.Play();

        yield return null;
        while (obj.isPlaying)
        {
            yield return null;
        }

        obj.Stop();
        obj.clip = null;
        obj.gameObject.SetActive (false);
        audioQue.Enqueue(obj);
    }
}
