using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class EventShop_RulletManager : MonoBehaviour
{
    public static EventShop_RulletManager inst;

    GameObject frontUIRef, RulletRef;
    GameObject rulletGameRef, slotMachineGameRef;
    GameObject rulletPan;

    //게임 선택 버튼
    Button selectRulletBtn;
    Button selectSlotMachineBtn;

    //상단 재료 Text 부분
    TMP_Text soulText;
    TMP_Text boneText;
    TMP_Text bookText;
    TMP_Text starText;
    TMP_Text rubyText;

    // 하단 티켓 Text 
    TMP_Text ticketText;

    // 연출
    GameObject rulletAction;
    GameObject actionPs;
    Image actionBackground;

    // 룰렛창 버튼들
    Button exitRulletBtn, startRulletBtn, adStartRulletBtn;

    ParticleSystem rulletParticle;

    // 슬롯머신
    GameObject slotMachine;
    Animator head_Anim;

    GameObject[] headListObj = new GameObject[5];
    Material pandaTear;
    Animator winHand_Anim;
    GameObject waitHand;

    Material[] slot = new Material[3];
    // 슬롯머신 버튼들
    Button exitRulletsBtn, startSlotMachineBtn, startadSlotMachineBtn;

    // 하단 소지 티겟 텍스트부분
    GameObject haveTicket, nohaveTiket;
    Animator nohaveTiketAnim;

    // 룰렛
    [SerializeField]
    Animator rulletArrowAnim;

    // 재생관련
    int[] slotNumber = new int[3];
    bool doSlotMachine;

    GameObject[] slotMachineAdPlayBtnTextRef = new GameObject[2];
    GameObject[] rulletMachineAdPlayBtnTextRef = new GameObject[2];

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        frontUIRef = GameManager.inst.FrontUiRef;
        RulletRef = frontUIRef.transform.Find("Rullet").gameObject;
        rulletGameRef = RulletRef.transform.Find("Window/Main/Rullet").gameObject;
        slotMachineGameRef = RulletRef.transform.Find("Window/Main/SlotMachine").gameObject;

        selectRulletBtn = RulletRef.transform.Find("Window/Main/BtnBg/RulletOnBtn").GetComponent<Button>();
        selectSlotMachineBtn = RulletRef.transform.Find("Window/Main/BtnBg/SlotMachineOnBtn").GetComponent<Button>();


        //룰렛
        rulletPan = RulletRef.transform.Find("Window/Main/Rullet/Rullet/Pan").gameObject;
        exitRulletBtn = RulletRef.transform.Find("Window/Main/Rullet/RulletBtns/ExitBtn").GetComponent<Button>();
        startRulletBtn = RulletRef.transform.Find("Window/Main/Rullet/RulletBtns/RuuletBtn").GetComponent<Button>();
        adStartRulletBtn = RulletRef.transform.Find("Window/Main/Rullet/RulletBtns/AdRulletBtn").GetComponent<Button>();
        rulletMachineAdPlayBtnTextRef[0] = adStartRulletBtn.transform.Find("True").gameObject;
        rulletMachineAdPlayBtnTextRef[1] = adStartRulletBtn.transform.Find("False").gameObject;

        rulletArrowAnim = RulletRef.transform.Find("Window/Main/Rullet/Rullet/Arrow").GetComponent<Animator>();

        //슬롯머신

        slotMachine = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachine").gameObject;
        slot[0] = slotMachine.transform.Find("Slot1/Ver").GetComponent<Image>().material;
        slot[1] = slotMachine.transform.Find("Slot2/Ver").GetComponent<Image>().material;
        slot[2] = slotMachine.transform.Find("Slot3/Ver").GetComponent<Image>().material;

        exitRulletsBtn = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachinetBtns/ExitBtn").GetComponent<Button>();
        startSlotMachineBtn = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachinetBtns/RuuletBtn").GetComponent<Button>();
        startadSlotMachineBtn = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachinetBtns/AdRulletBtn").GetComponent<Button>();
        slotMachineAdPlayBtnTextRef[0] = startadSlotMachineBtn.transform.Find("True").gameObject;
        slotMachineAdPlayBtnTextRef[1] = startadSlotMachineBtn.transform.Find("False").gameObject;

        rulletParticle = RulletRef.transform.Find("Window/Main/RulletPs").GetComponent<ParticleSystem>();

        soulText = RulletRef.transform.Find("Window/Material/Soul/Text").GetComponent<TMP_Text>();
        boneText = RulletRef.transform.Find("Window/Material/Bone/Text").GetComponent<TMP_Text>();
        bookText = RulletRef.transform.Find("Window/Material/Book/Text").GetComponent<TMP_Text>();
        starText = RulletRef.transform.Find("Window/Material/Star/Text").GetComponent<TMP_Text>();
        rubyText = RulletRef.transform.Find("Window/Material/Ruby/Text").GetComponent<TMP_Text>();
        ticketText = RulletRef.transform.Find("Window/Main/Bot_Text/TicketText").GetComponent<TMP_Text>();

        head_Anim = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachine/Head").GetComponent<Animator>();
        winHand_Anim = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachine/IMG_BOX/Hand_Win").GetComponent<Animator>();
        waitHand = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachine/IMG_BOX/Hand_Wait").gameObject;

        headListObj[0] = head_Anim.transform.Find("Wait").gameObject;
        headListObj[1] = head_Anim.transform.Find("Play").gameObject;
        headListObj[2] = head_Anim.transform.Find("Reward2").gameObject;
        headListObj[3] = head_Anim.transform.Find("Reward3").gameObject;
        headListObj[4] = head_Anim.transform.Find("Sad").gameObject;
        pandaTear = headListObj[4].transform.Find("LW").GetComponent<Image>().material;

        // 연출
        rulletAction = RulletRef.transform.Find("Window/Main/GembleBG").gameObject;
        actionPs = rulletAction.transform.Find("Ps").gameObject;
        actionBackground = RulletRef.transform.Find("Window/Main/GembleBG/BG").GetComponent<Image>();

        // 하단 문구
        haveTicket = RulletRef.transform.Find("Window/Main/Bot_Text/TicketText").gameObject;
        nohaveTiket = RulletRef.transform.Find("Window/Main/Bot_Text/NoTicket").gameObject;
        nohaveTiketAnim = nohaveTiket.GetComponent<Animator>();

        BtnInit();
    }
    void Start()
    {

    }

    Vector2 cryVec;
    float crySpeedMultiFlyer = 4f;
    void Update()
    {
        DownRulletSpinSpeed();
        SlotMachineCryEffect();

        if (Input.GetKeyDown(KeyCode.T))
        {
            GameStatus.inst.MinigameTicket++;
        };
    }





    /// <summary>
    /// 이벤트샵 On/Off
    /// </summary>
    /// <param name="value"></param>
    public void Active_RulletEventShop(bool value)
    {
        RulletRef.SetActive(value);
        MaterialTextUpdater();
        BotText_Updater();
    }



    private void BtnInit()
    {
        selectRulletBtn.onClick.AddListener(() =>
        {
            if (rulletGameRef.activeSelf == false && doSlotMachine == false)
            {
                WorldUI_Manager.inst.FrontUICuttonAction(() =>
                {
                    selectGame(0);
                });
            }
        });

        selectSlotMachineBtn.onClick.AddListener(() =>
        {
            if (slotMachineGameRef.activeSelf == false && doRullet == false)
            {
                WorldUI_Manager.inst.FrontUICuttonAction(() =>
                {
                    selectGame(1);
                });
            }

        });

        exitRulletBtn.onClick.AddListener(() => { if (doRullet == true) { return; } Active_RulletEventShop(false); });
        exitRulletsBtn.onClick.AddListener(() => { if (doSlotMachine == true) { return; } Active_RulletEventShop(false); });

        //슬롯머신 일반
        startSlotMachineBtn.onClick.AddListener(() =>
        {
            if (doSlotMachine == true) { return; }
            if (GameStatus.inst.MinigameTicket <= 0) { nohaveTiketAnim.SetTrigger("False"); return; }
            if (GameStatus.inst.MinigameTicket > 0)
            {
                GameStatus.inst.MinigameTicket--;
            }

            PlaySlotMachine();

        });

        //슬롯머신 광고
        startadSlotMachineBtn.onClick.AddListener(() =>
        {
            if (doSlotMachine == true) { return; }
            ADViewManager.inst.SampleAD_Active_Funtion(() =>
            {
                // 하루 추가
                GameStatus.inst.AdSlotMachineActive = true;
                PlaySlotMachine();
            });
        });

        //룰렛 일반
        startRulletBtn.onClick.AddListener(() =>
        {
            if (doRullet == true) { return; }
            if (GameStatus.inst.MinigameTicket <= 0) { nohaveTiketAnim.SetTrigger("False"); return; }
            if (GameStatus.inst.MinigameTicket > 0)
            {
                GameStatus.inst.MinigameTicket--;
            }

            RulletStart();
        });

        //룰렛 광고
        adStartRulletBtn.onClick.AddListener(() =>
        {
            if (doRullet == true) { return; }
            ADViewManager.inst.SampleAD_Active_Funtion(() => 
            {
                GameStatus.inst.AdRulletActive = true;
                RulletStart(); 
            });

        });


    }

    /// <summary>
    /// AD광고 버튼 활성 / 비활성화 해주는 함수
    /// </summary>
    /// <param name="gameNumber"></param>
    /// <param name="Active"></param>
    public void AdPlayButtonInit(int gameNumber, bool Active)
    {
        // true 로 들어오면 비활성화, false는 활성화
        if (!Active)
        {
            switch (gameNumber)
            {
                case 0:
                    adStartRulletBtn.interactable = true;
                    rulletMachineAdPlayBtnTextRef[0].SetActive(true);
                    rulletMachineAdPlayBtnTextRef[1].SetActive(false);
                    break;

                case 1:
                    startadSlotMachineBtn.interactable = true;
                    slotMachineAdPlayBtnTextRef[0].SetActive(true);
                    slotMachineAdPlayBtnTextRef[1].SetActive(false);
                    break;
            }
        }
        else
        {
            switch (gameNumber)
            {
                case 0:
                    adStartRulletBtn.interactable = false;
                    rulletMachineAdPlayBtnTextRef[0].SetActive(false);
                    rulletMachineAdPlayBtnTextRef[1].SetActive(true);
                    break;

                case 1:
                    startadSlotMachineBtn.interactable = false;
                    slotMachineAdPlayBtnTextRef[0].SetActive(false);
                    slotMachineAdPlayBtnTextRef[1].SetActive(true);
                    break;
            }
        }
        
    }
    public void BotText_Updater()
    {
        int ticket = GameStatus.inst.MinigameTicket;

        if (ticket > 0)
        {
            ticketText.text = $"이벤트 티켓 갯수 : {GameStatus.inst.MinigameTicket}";
            haveTicket.SetActive(true);
            nohaveTiket.SetActive(false);
        }
        else if (ticket <= 0)
        {
            haveTicket.SetActive(false);
            nohaveTiket.SetActive(true);
        }
    }

    /// <summary>
    /// 게임 끄고켜기 0 룰렛 / 1 슬롯머신
    /// </summary>
    /// <param name="indexNum"></param>

    public void selectGame(int indexNum)
    {
        if (indexNum == 0)
        {
            rulletGameRef.SetActive(true);
            PlayPandaAnimation(0);
            slotMachineGameRef.SetActive(false);

        }
        else if (indexNum == 1)
        {
            rulletGameRef.SetActive(false);
            PlayPandaAnimation(0);
            slotMachineGameRef.SetActive(true);
        }
    }

    ///////////////////////////////////// 슬롯 머신 ////////////////////////////////////////////////

    Coroutine[] slotMachines = new Coroutine[3];
    private void PlaySlotMachine()
    {
        doSlotMachine = true;
        PlayPandaAnimation(1);

        RulletAction(true);

        if (slotMachines[0] != null)
        {
            StopCoroutine(slotMachines[0]);
            StopCoroutine(slotMachines[1]);
            StopCoroutine(slotMachines[2]);
        }

        slotMachines[0] = StartCoroutine(SlotPlay(0));
        slotMachines[1] = StartCoroutine(SlotPlay(1));
        slotMachines[2] = StartCoroutine(SlotPlay(2));
    }


    float tillingSpeedMultiplyer = 3.8f;
    float slotFloat = 0.168f; // 타일링 1칸

    IEnumerator SlotPlay(int value)
    {
        Vector2 tillingVec = Vector2.zero;
        float timer = 0f;
        float randomStartValue = Random.Range(0f, 1f);
        tillingVec.y = randomStartValue;
        rulletParticle.gameObject.SetActive(false);
        // 슬롯 돌아가는 시간 생성

        float slotActiontime = 0f;
        switch (value)
        {
            case 0:
                slotActiontime = 2f;
                break;
            case 1:
                slotActiontime = 4f;
                break;
            case 2:
                slotActiontime = 6f;
                break;
        }

        while (timer < slotActiontime)
        {
            tillingVec.y += Time.deltaTime * tillingSpeedMultiplyer;
            tillingVec.y = Mathf.Repeat(tillingVec.y, 1);
            slot[value].mainTextureOffset = tillingVec;
            timer += Time.deltaTime;
            yield return null;
        }

        // Lerp를 사용한 부드러운 정지
        float finalPosition = DetermineFinalPosition(slot[value].mainTextureOffset.y, value);
        float lerpTime = 0f;
        float duration = 1f; // Lerp가 완료되는 데 필요한 시간
        bool once = false;

        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float lerpFactor = lerpTime / duration;
            tillingVec.y = Mathf.Lerp(slot[value].mainTextureOffset.y, finalPosition, lerpFactor);
            slot[value].mainTextureOffset = tillingVec;



            if (value == 2 && lerpTime > 0.15f && once == false) // 3번쨰 슬롯 멈추면
            {
                once = true;
                //당첨 로직
                RewardItem();
                MaterialTextUpdater();
                RulletAction(false);
            }

            yield return null;
        }

        if (value == 2) // 3번쨰 슬롯 멈추면
        {
            doSlotMachine = false;
        }
    }

    Dictionary<int, int> checkCount = new Dictionary<int, int>();

    /// <summary>
    /// 아이템 당첨 분류 함수
    /// </summary>
    private void RewardItem()
    {
        checkCount.Clear();
        bool haveReward = false;


        foreach (int item in slotNumber)
        {
            if (checkCount.ContainsKey(item))
            {
                checkCount[item]++;
            }
            else
            {
                checkCount[item] = 1;
            }
        }

        foreach (var pair in checkCount)
        {
            switch (pair.Value)
            {
                //2개 당첨시

                case 2:
                    haveReward = true;
                    PlayPandaAnimation(2);
                    rulletParticle.gameObject.SetActive(true);

                    if (pair.Key == 0) //루비 2개 당첨
                    {
                        GameStatus.inst.Ruby += 200; //실행
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " 루비 +200");
                    }
                    else if (pair.Key == 1) // 뼈 2개 당첨
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(1, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(1), " 뼈 +200");
                    }
                    else if (pair.Key == 2) // 영혼 2개 당첨
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(0, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(0), " 영혼 +200");
                    }
                    else if (pair.Key == 3) //별 2개 당첨
                    {
                        GameStatus.inst.PlusStar("200");
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), "별 + 200");
                    }
                    else if (pair.Key == 5) //책 2개 당첨
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(2, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(2), " 고서 +200");
                    }

                    break;

                //3개 당첨시
                case 3:
                    haveReward = true;
                    PlayPandaAnimation(3);
                    rulletParticle.gameObject.SetActive(true);

                    if (pair.Key == 0) //루비 3개 당첨
                    {
                        GameStatus.inst.Ruby += 500; //실행
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " 루비 +500");
                    }
                    else if (pair.Key == 1) //뼈 3개 당첨
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(1, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(1), " 뼈 +500");
                    }
                    else if (pair.Key == 2) //영혼 3개 당첨
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(0, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(0), " 영혼 +500");
                    }
                    else if (pair.Key == 3) //별 3개 당첨
                    {
                        GameStatus.inst.PlusStar("500");
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), " 별 + 500");
                    }
                    else if (pair.Key == 5) //책 3개 당첨
                    {
                        CrewGatchaContent.inst.MaterialCountEditor(2, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(2), " 고서 +500");
                    }
                    break;
            }
        }

        //꽝
        if (haveReward == false)
        {
            Debug.Log("꽝");
            PlayPandaAnimation(4);
        }
    }

    // 슬롯머신 멈추는 예상지점 반환
    private float DetermineFinalPosition(float currentY, int slotNumber)
    {
        int dotindex = currentY.ToString().IndexOf(".");
        float firstDigit = float.Parse(currentY.ToString().Substring(0, dotindex));
        float decimalNum = float.Parse(currentY.ToString().Substring(dotindex + 1, 2)) / 100;

        if (decimalNum > 0 && decimalNum <= slotFloat)
        {
            decimalNum = slotFloat;
            SlotNumberWrite(slotNumber, 1);
        }
        else if (decimalNum > slotFloat && decimalNum <= slotFloat * 2)
        {
            decimalNum = slotFloat * 2;
            SlotNumberWrite(slotNumber, 2);
        }
        else if (decimalNum > slotFloat * 2 && decimalNum <= slotFloat * 3)
        {
            decimalNum = slotFloat * 3;
            SlotNumberWrite(slotNumber, 3);
        }
        else if (decimalNum > slotFloat * 3 && decimalNum <= slotFloat * 4)
        {
            decimalNum = slotFloat * 4;
            SlotNumberWrite(slotNumber, 0);
        }
        else if (decimalNum > slotFloat * 4 && decimalNum <= slotFloat * 5)
        {
            decimalNum = slotFloat * 5;
            SlotNumberWrite(slotNumber, 5);
        }
        else if (decimalNum > slotFloat * 5)
        {
            decimalNum = slotFloat * 6;
            SlotNumberWrite(slotNumber, 0);
        }

        return firstDigit + decimalNum;
    }


    // 슬롯머신 당첨 번호 저장
    private void SlotNumberWrite(int value, int Number)
    {
        slotNumber[value] = Number;
    }



    /// <summary>
    /// 얼굴 애니메이션 작동 및 Active 작동
    /// </summary>
    /// <param name="value"> 0 = End / 1 = Play / 2 = 보상2 / 3 =보상3 / 4 = 슬픔(꽝) </param>
    private void PlayPandaAnimation(int value)
    {
        for (int index = 0; index < headListObj.Length; index++)
        {
            if (value == index)
            {
                headListObj[index].SetActive(true);
            }
            else
            {
                headListObj[index].SetActive(false);
            }
        }



        switch (value)
        {
            case 0:
                head_Anim.SetTrigger("End");
                HandAcitve(0, 0);
                cry = false;
                break;
            case 1:
                head_Anim.SetTrigger("Play");
                HandAcitve(0, 0);
                break;
            case 2:
                head_Anim.SetTrigger("Reward2");
                HandAcitve(1, 2);
                break;
            case 3:
                head_Anim.SetTrigger("Reward3");
                HandAcitve(1, 3);
                break;
            case 4:
                head_Anim.SetTrigger("Sad");
                HandAcitve(0, 0);
                cry = true;
                break;
        }

    }

    // 주먹, 흔든손 On Off
    private void HandAcitve(int value, int RewardNumber)
    {
        if (value == 0)
        {
            waitHand.gameObject.SetActive(true);
            winHand_Anim.gameObject.SetActive(false);

        }
        else
        {
            waitHand.gameObject.SetActive(false);
            winHand_Anim.gameObject.SetActive(true);
        }

        StartCoroutine(PlayAnim(RewardNumber));
    }

    IEnumerator PlayAnim(int RewardNumber)
    {
        yield return null;

        if (RewardNumber == 2)
        {
            winHand_Anim.SetTrigger("Reward2");
        }
        else if (RewardNumber == 3)
        {
            winHand_Anim.SetTrigger("Reward3");
        }
    }

    bool cry;

    // 눈물 Effect
    private void SlotMachineCryEffect()
    {
        if (cry)
        {
            cryVec.y += Time.deltaTime * crySpeedMultiFlyer;
            cryVec.y = Mathf.Repeat(cryVec.y, 1);
            pandaTear.mainTextureOffset = cryVec;
        }
    }

    ////////////////////////////////////// 룰렛 ////////////////////////////////////////

    bool doRullet = false;
    float spinSpeed;
    [SerializeField]
    float spinSpeedDownMulyfly;

    Vector3 rotZ;

    private void RulletStart()
    {
        if (RulletRef.activeSelf == true && doRullet == false)
        {
            rulletParticle.gameObject.SetActive(false);
            RulletAction(true);
            StartCoroutine(RulletSpinStart());
        }
    }

    WaitForSeconds rulletDealy = new WaitForSeconds(0.1f);
    IEnumerator RulletSpinStart()
    {
        yield return rulletDealy;
        doRullet = true;
        spinSpeed = 2500f;
        rotZ.z = Random.Range(0, 360);
        rulletPan.transform.eulerAngles = rotZ;
    }


    // 룰렛판 속도감소
    private void DownRulletSpinSpeed()
    {
        if (doRullet == true)
        {
            rotZ = rulletPan.transform.eulerAngles;
            rotZ.z = Mathf.Repeat(rotZ.z, 360);
            rulletPan.transform.eulerAngles = rotZ;

            rulletPan.transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);

            spinSpeed -= Time.deltaTime * spinSpeedDownMulyfly;
            StartCoroutine(Arrow());

            if (spinSpeed <= 0)
            {
                spinSpeed = 0;

                // 룰렛 보상
                float checkvalue = rulletPan.transform.eulerAngles.z;
                Debug.Log(checkvalue);

                if (330 < checkvalue && checkvalue < 30)
                {
                    rulletParticle.gameObject.SetActive(true);
                    GameStatus.inst.Ruby += 10; //실행
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " 루비 +10");
                }
                else if (checkvalue >= 30 && checkvalue < 90)
                {
                    Debug.Log("꽝");
                }
                else if (checkvalue >= 90 && checkvalue < 150)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(0, 2, "공격력 버프 2분");
                    //WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(0),"공격력 버프 2분");

                }
                else if (checkvalue >= 150 && checkvalue < 210)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(3, 2, "강한 공격력 버프 2분");

                }
                else if (checkvalue >= 210 && checkvalue < 270)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(2, 2, "골드 획득량 증가 버프 2분");
                }
                else if (checkvalue >= 270 && checkvalue < 330)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(1, 2, "이동속도 증가 버프 2분");
                }

                MaterialTextUpdater();
                RulletAction(false);
                doRullet = false;

            }
        }
    }


    IEnumerator Arrow()
    {
        if (IsPlaying("Play") == false)
        {
            rulletArrowAnim.SetTrigger("Play");
        }

        while (spinSpeed > 10f)
        {
            rulletArrowAnim.speed = spinSpeed * 0.0015f;
            yield return null;
        }

        rulletArrowAnim.SetTrigger("Wait");

    }

    // Animator의 현재 상태가 'stateName'인지 확인
    private bool IsPlaying(string ClipName)
    {
        return rulletArrowAnim.GetCurrentAnimatorStateInfo(0).IsName(ClipName) &&
                     rulletArrowAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }

    /// <summary>
    /// UI 상단 메테리얼 업데이터 (최신화)
    /// </summary>
    private void MaterialTextUpdater()
    {
        int[] material = CrewGatchaContent.inst.Get_CurCrewUpgreadMaterial();
        soulText.text = material[0].ToString("N0");
        boneText.text = material[1].ToString("N0");
        bookText.text = material[2].ToString("N0");
        starText.text = CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Star);
        rubyText.text = GameStatus.inst.Ruby.ToString("N0");
    }





    /// <summary>
    /// Rullet 1회 플레이 (배경 Fade + Particle Effect)
    /// </summary>
    /// <param name="value"></param>
    private void RulletAction(bool value)
    {
        StartCoroutine(RulletActionCoru(value));
    }

    Color fadeColor = new Color(0, 0, 0, 0.15f);
    float fadeSpeedMultiPly = 15;
    float fadeoutSpeedMultiPly = 10;
    IEnumerator RulletActionCoru(bool value)
    {
        if (value)
        {
            rulletAction.SetActive(true);
            actionPs.SetActive(true);
            while (actionBackground.color.a < 0.95f)
            {
                actionBackground.color += fadeColor * Time.deltaTime * fadeSpeedMultiPly;
                yield return null;
            }

        }
        else
        {
            while (actionBackground.color.a > 0)
            {
                actionBackground.color -= fadeColor * Time.deltaTime * fadeoutSpeedMultiPly;

                if (actionBackground.color.a < 0.7f && actionPs.activeSelf == true)
                {
                    actionPs.SetActive(false);
                }
                yield return null;
            }

            rulletAction.SetActive(false);
        }
    }
}
