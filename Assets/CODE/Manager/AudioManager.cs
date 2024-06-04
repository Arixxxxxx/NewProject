using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;

    [SerializeField] AudioClip[] BGM;
    [Space]
    AudioSource bgmPlayer;

    [SerializeField] AudioClip[] Ui_SFX;
    [Space]
    //월드 재생
    [SerializeField] AudioClip[] Wolrd_SFX;
    [SerializeField] AudioClip[] playerHit;
    [SerializeField] AudioClip[] playerCri;
    [SerializeField] AudioClip[] monsterHit;
    [SerializeField] AudioClip[] monsterDead;
    Transform sfxTrs;
    bool[] isSoundPlay;
    float[] worldSoundDealyTimer;
    float[] worldSoundDealy = { 0.25f, 0.25f };
    [Header("#Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerGroup SFXGroup;
    public bool noSound; 
    Queue<AudioSource> audioQue = new Queue<AudioSource>();
    int channel = 32;
    int curBGMNum;
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

        isSoundPlay = new bool[2];
        worldSoundDealyTimer = new float[2];

        for (int index = 0; index < channel; index++) 
        {
            MakeSoundClip();
        }
        
    }
    void Start()
    {
        bgmPlayer.volume = 0.8f;
        bgmPlayer.clip = BGM[0];
        bgmPlayer.Play();

        
    }

    // Update is called once per frame
 
    void Update()
    {
        WorldSoundDealyCheker(0);
        WorldSoundDealyCheker(1);
     }

    private void WorldSoundDealyCheker(int soundIndex)
    {
        if (isSoundPlay[soundIndex] == true)
        {
            worldSoundDealyTimer[soundIndex] += Time.deltaTime;
            
            if (worldSoundDealyTimer[soundIndex] >= worldSoundDealy[soundIndex])
            {
                worldSoundDealyTimer[soundIndex] = 0f;
                isSoundPlay[soundIndex] = false;
            }

        }
    }

    private void MakeSoundClip()
    {
        AudioSource audioObj = new GameObject("SFX").AddComponent<AudioSource>();
        audioObj.playOnAwake = false;
        audioObj.outputAudioMixerGroup = SFXGroup;
        audioObj.transform.SetParent(sfxTrs);
        audioObj.gameObject.SetActive(false);
        audioQue.Enqueue(audioObj);
    }

    //BGM
    public void PlayBGM(int number)
    {
        if (bgmPlayer.clip == BGM[number]) { return; };

        StartCoroutine(BGM_Changer(number));
    }

    float duration = 0.2f;
    float counter = 0f;
    IEnumerator BGM_Changer(int number)
    {
        counter = 0f;

        while (counter < duration)
        {
            bgmPlayer.volume = Mathf.Lerp(0.8f, 0f, counter / duration);
            counter += Time.deltaTime;
            yield return null;
        }
        bgmPlayer.clip = BGM[number];
        bgmPlayer.Play();
        counter = 0f;

        while (counter < duration)
        {
            bgmPlayer.volume = Mathf.Lerp(0f, 0.8f, counter / duration);
            counter += Time.deltaTime;
            yield return null;
        }
    }
    /// <summary>
    /// SFX 재생기 (UI 효과음)
    /// </summary>
    /// <param name="index"> 0:로그인씬 탭투스크린<br/>1:</param>
    public void Play_Ui_SFX(int index, float Volume)
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
        obj.volume = 1;
        obj.gameObject.SetActive (false);
        audioQue.Enqueue(obj);
    }


  
    /// <summary>
    /// World 재생
    /// </summary>
    /// <param name="index"> 0:아이템획득<br/></param>
    public void Play_World_SFX(int index, float Volume)
    {

        if (audioQue.Count <= 0)
        {
            MakeSoundClip();
        }

        StartCoroutine(SoundPlay(index, Volume));

        
    }

    IEnumerator SoundPlay(int index, float Volume)
    {
        AudioSource obj = audioQue.Dequeue();
        obj.volume = Volume;
        obj.clip = Wolrd_SFX[index];
        obj.gameObject.SetActive(true);
        obj.Play();

        yield return null;
        while (obj.isPlaying)
        {
            yield return null;
        }

        obj.Stop();
        obj.clip = null;
        obj.volume = 1;
        obj.gameObject.SetActive(false);
        audioQue.Enqueue(obj);
    }

    /// <summary>
    /// World 재생
    /// </summary>
    /// <param name="index"> 0:설아 HIT<br/></param>
    public void Play_HitOnly(int index, float Volume, bool Cri)
    {
        if(noSound) { return; }

        if (audioQue.Count <= 0)
        {
            MakeSoundClip();
        }

        if (isSoundPlay[0] == false)
        {
            isSoundPlay[0] = true;
            StartCoroutine(HitSoundPlay(index, Volume,Cri));
        }
    }

    IEnumerator HitSoundPlay(int index, float Volume, bool Cri)
    {
        AudioSource obj = audioQue.Dequeue();
        obj.volume = Volume;

        if (Cri)
        {
            obj.clip = playerCri[UnityEngine.Random.Range(0,playerCri.Length)];
        }
        else
        {
            obj.clip = playerHit[UnityEngine.Random.Range(0, playerHit.Length)];
        }
    
        obj.gameObject.SetActive(true);
        obj.Play();

        yield return null;
        while (obj.isPlaying)
        {
            yield return null;
        }

        obj.Stop();
        obj.clip = null;
        obj.volume = 1;
        obj.gameObject.SetActive(false);
        audioQue.Enqueue(obj);
    }

    public void MonsterHitOnly(bool enemyLive, float Volume)
    {
        if (noSound) { return; }

        if (audioQue.Count <= 0)
        {
            MakeSoundClip();
        }

        if (isSoundPlay[1] == false)
        {
            isSoundPlay[1] = true;
            StartCoroutine(MonsterHitSoundPlay(enemyLive, Volume));
        }
    }

    IEnumerator MonsterHitSoundPlay(bool enemyLive, float Volume)
    {
        AudioSource obj = audioQue.Dequeue();
        
        if(enemyLive == true)
        {
            obj.clip = monsterHit[UnityEngine.Random.Range(0, monsterHit.Length)];
        }
        else
        {
            obj.clip = monsterDead[UnityEngine.Random.Range(0, monsterDead.Length)];
        }
        obj.volume = Volume;
        obj.gameObject.SetActive(true);
        obj.Play();

        yield return null;
        while (obj.isPlaying)
        {
            yield return null;
        }

        obj.Stop();
        obj.clip = null;
        obj.volume = 1;
        obj.gameObject.SetActive(false);
        audioQue.Enqueue(obj);
    }


    float muteValue = -80f;
    float defaultVolumeValue = 0f;

    /// <summary>
    /// 오디오믹서 BGM / SFX Mute 기능 (for 환경설정)
    /// </summary>
    /// <param name="parameterName"> BGM  / SFX </param>
    /// <param name="value">true, false</param>
    public void Set_VoulemMute(string parameterName, bool value)
    {
        switch (parameterName) 
        {
            case "BGM":
                if (value)
                {
                    audioMixer.SetFloat("BGM", defaultVolumeValue);
                }
                else
                {
                    audioMixer.SetFloat("BGM", muteValue);
                }
                break;

            case "SFX":
                if (value)
                {
                    audioMixer.SetFloat("SFX", defaultVolumeValue);
                }
                else
                {
                    audioMixer.SetFloat("SFX", muteValue);
                }
                break;
        }

    }
}
