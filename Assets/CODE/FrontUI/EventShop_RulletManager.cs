using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using System.Numerics;
using System;

public class EventShop_RulletManager : MonoBehaviour
{
    public static EventShop_RulletManager inst;

    GameObject frontUIRef, RulletRef;
    GameObject rulletGameRef, slotMachineGameRef;
    GameObject rulletPan;

    //게임 선택 버튼
    Button selectRulletBtn;
    Button selectSlotMachineBtn;
    Button selectBingoBtn;

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
    Animator rulletArrowAnim;

    // 빙고
    [SerializeField] List<bingo> list_bingo = new List<bingo>();

    [Serializable]
    class bingo
    {
        public ProductTag rewardType;
        public int count;

        GameObject objBingo;
        GameObject objBingoMask;
        GameObject objBingoText;
        GameObject objParticle;
        TMP_Text countText;
        Image rewardImage;
        Animator objBingoAnim;

        public void initBingo(GameObject _obj)
        {
            objBingo = _obj;
            countText = objBingo.transform.Find("Text (TMP)").GetComponent<TMP_Text>();
            rewardImage = objBingo.transform.Find("Image").GetComponent<Image>();
            
            objParticle = objBingo.transform.Find("Fireworks").gameObject;

            objBingoMask = _obj.transform.Find("Mask").gameObject;
            objBingoText = objBingoMask.transform.GetChild(0).gameObject;
            objBingoAnim = objBingoMask.GetComponent<Animator>();

            rewardImage.sprite = UIManager.Instance.GetProdSprite((int)rewardType);
            rewardImage.SetNativeSize();
            float ratio = rewardImage.sprite.bounds.size.x / rewardImage.sprite.bounds.size.y;
            if (rewardType == ProductTag.MiniGameTicket)
            {
                rewardImage.rectTransform.sizeDelta = new UnityEngine.Vector2(30 * ratio, 30f);
                rewardImage.rectTransform.localEulerAngles = new UnityEngine.Vector3(0, 0, 16f);
            }
            else
            {
                rewardImage.rectTransform.sizeDelta = new UnityEngine.Vector2(40 * ratio, 40f);
                rewardImage.rectTransform.localEulerAngles = new UnityEngine.Vector3(0, 0, 0);
            }

        }

        public void initStart()
        {
            //갯수 텍스트 초기화
            switch (rewardType)
            {
                case ProductTag.Gold:
                    BigInteger prodGold = GameStatus.inst.TotalProdGold;
                    countText.text = (prodGold * count).ToString();
                    break;
                case ProductTag.Star:
                    countText.text = CalCulator.inst.StringFourDigitAddFloatChanger(count.ToString());
                    break;
                case ProductTag.Ruby:
                    countText.text = count.ToString();
                    break;
                case ProductTag.MiniGameTicket:
                    countText.text = "티켓";
                    break;
            }
        }

        public void SetGoldText()
        {
            if (rewardType == ProductTag.Gold)
            {
                BigInteger prodGold = GameStatus.inst.TotalProdGold;
                countText.text = CalCulator.inst.StringFourDigitAddFloatChanger((prodGold * count).ToString());
            }
        }

        public void GetReward()
        {
            switch (rewardType)
            {
                case ProductTag.Gold:
                    BigInteger prodGold = GameStatus.inst.TotalProdGold;
                    prodGold = prodGold * count;
                    GameStatus.inst.PlusGold(prodGold.ToString());
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(1), "골드 " + CalCulator.inst.StringFourDigitAddFloatChanger(prodGold.ToString()) + "개");
                    break;
                case ProductTag.Star:
                    GameStatus.inst.PlusStar(count.ToString());
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), "별 " + CalCulator.inst.StringFourDigitAddFloatChanger(count.ToString()) + "개");
                    break;
                case ProductTag.Ruby:
                    GameStatus.inst.PlusRuby(count);
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "루비 " + CalCulator.inst.StringFourDigitAddFloatChanger(count.ToString()) + "개");
                    break;
                case ProductTag.MiniGameTicket:
                    GameStatus.inst.MinigameTicket += count;
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(3), "이벤트티켓 " + count.ToString() + "장");
                    break;
            }
        }

        public void SetParticalActive(bool value)
        {
            objParticle.SetActive(value);
        }

        public GameObject GetObj()
        {
            return objBingo;
        }

        public void SetMaskActive(bool value)
        {
            objBingoMask.SetActive(value);
        }

        public void SetMaskTextActive(bool value)
        {
            objBingoText.SetActive(value);
        }

        public Animator GetAnim()
        {
            return objBingoAnim;
        }

        public void SetBingo(ProductTag _type, int _count)
        {
            rewardType = _type;
            count = _count;

            rewardImage.sprite = UIManager.Instance.GetProdSprite((int)rewardType);
            rewardImage.SetNativeSize();
            float ratio = rewardImage.sprite.bounds.size.x / rewardImage.sprite.bounds.size.y;
            if (rewardType == ProductTag.MiniGameTicket)
            {
                rewardImage.rectTransform.sizeDelta = new UnityEngine.Vector2(30 * ratio, 30f);
                rewardImage.rectTransform.localEulerAngles = new UnityEngine.Vector3(0, 0, 16f);
            }
            else
            {
                rewardImage.rectTransform.sizeDelta = new UnityEngine.Vector2(40 * ratio, 40f);
                rewardImage.rectTransform.localEulerAngles = new UnityEngine.Vector3(0, 0, 0);

            }
        }
    }
    List<GameObject> list_ObjBingoReward = new List<GameObject>();
    List<Image> list_ObjBingoRewardImage = new List<Image>();
    List<TMP_Text> list_ObjBingoRewardText = new List<TMP_Text>();
    GameObject objShowBingoReward;
    Transform BingoRef;


    int bingoStack = 0;
    bool isDoBingo = false;
    bool isBingoClear = false;

    bool[] list_BingoBoard = new bool[9];
    bool[] verticalBingo = new bool[3];
    bool[] horiBingo = new bool[3];
    bool[] crossBingo = new bool[2];

    // 빙고 버튼들
    Button exitBingoBtn, startBingoBtn, startadBingoBtn;

    // 재생관련
    int[] slotNumber = new int[3];
    bool doSlotMachine;

    GameObject[] slotMachineAdPlayBtnTextRef = new GameObject[2];
    GameObject[] rulletMachineAdPlayBtnTextRef = new GameObject[2];
    GameObject[] bingoAdPlayBtnTextRef = new GameObject[2];


    // 테스트용 치트버튼
    Button cheetBtn;

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
        selectBingoBtn = RulletRef.transform.Find("Window/Main/BtnBg/BingoBtn").GetComponent<Button>();


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

        //빙고
        BingoRef = RulletRef.transform.Find("Window/Main/Bingo");


        objShowBingoReward = BingoRef.Find("ShowReward").gameObject;
        objShowBingoReward.GetComponent<Button>().onClick.AddListener(() => { objShowBingoReward.SetActive(false); });

        Transform BingoBoardRef = BingoRef.Find("BingoBoard");
        int BingoCount = list_bingo.Count;
        for (int iNum = 0; iNum < BingoCount; iNum++)
        {
            GameObject obj = BingoBoardRef.GetChild(iNum).gameObject;
            list_bingo[iNum].initBingo(obj);
        }

        int bingoRewardCount = objShowBingoReward.transform.childCount;
        for (int iNum = 0; iNum < bingoRewardCount; iNum++)
        {
            GameObject obj = objShowBingoReward.transform.GetChild(iNum).gameObject;
            list_ObjBingoReward.Add(obj);
            list_ObjBingoRewardImage.Add(obj.transform.Find("Image").GetComponent<Image>());
            list_ObjBingoRewardText.Add(obj.transform.Find("CountText").GetComponent<TMP_Text>());
        }


        exitBingoBtn = RulletRef.transform.Find("Window/Main/Bingo/BingoBtns/ExitBtn").GetComponent<Button>();
        startBingoBtn = RulletRef.transform.Find("Window/Main/Bingo/BingoBtns/RuuletBtn").GetComponent<Button>();
        startadBingoBtn = RulletRef.transform.Find("Window/Main/Bingo/BingoBtns/AdRulletBtn").GetComponent<Button>();
        slotMachineAdPlayBtnTextRef[0] = startadSlotMachineBtn.transform.Find("True").gameObject;
        slotMachineAdPlayBtnTextRef[1] = startadSlotMachineBtn.transform.Find("False").gameObject;
        bingoAdPlayBtnTextRef[0] = startadBingoBtn.transform.Find("True").gameObject;
        bingoAdPlayBtnTextRef[1] = startadBingoBtn.transform.Find("False").gameObject;

        // 연출
        rulletAction = RulletRef.transform.Find("Window/Main/GembleBG").gameObject;
        actionPs = rulletAction.transform.Find("Ps").gameObject;
        actionBackground = RulletRef.transform.Find("Window/Main/GembleBG/BG").GetComponent<Image>();

        // 하단 문구
        haveTicket = RulletRef.transform.Find("Window/Main/Bot_Text/TicketText").gameObject;
        nohaveTiket = RulletRef.transform.Find("Window/Main/Bot_Text/NoTicket").gameObject;
        nohaveTiketAnim = nohaveTiket.GetComponent<Animator>();

        cheetBtn = RulletRef.transform.Find("Window/Main/Cheet").GetComponent<Button>();
        cheetBtn.onClick.AddListener(() => { GameStatus.inst.MinigameTicket++; });
        BtnInit();
    }
    void Start()
    {
        int BingoCount = list_bingo.Count;
        for (int iNum = 0; iNum < BingoCount; iNum++)
        {
            if (GameStatus.inst.GetBingoClass(iNum).count != 0)
            {
                list_bingo[iNum].SetBingo(GameStatus.inst.GetBingoClass(iNum).type, GameStatus.inst.GetBingoClass(iNum).count);
            }
            list_bingo[iNum].initStart();
        }


        int boardCount = list_BingoBoard.Length;
        for (int iNum = 0; iNum < boardCount; iNum++)
        {
            setBingoBoard(iNum, GameStatus.inst.GetBingoBoard(iNum));
        }
        //수직 빙고 체크
        if (verticalBingo[0] == false && list_BingoBoard[0] == true && list_BingoBoard[3] == true && list_BingoBoard[6] == true)
        {
            verticalBingo[0] = true;
        }
        if (verticalBingo[1] == false && list_BingoBoard[1] == true && list_BingoBoard[4] == true && list_BingoBoard[7] == true)
        {
            verticalBingo[1] = true;
        }
        if (verticalBingo[2] == false && list_BingoBoard[2] == true && list_BingoBoard[5] == true && list_BingoBoard[8] == true)
        {
            verticalBingo[2] = true;
        }
        //수평 빙고 체크
        if (horiBingo[0] == false && list_BingoBoard[0] == true && list_BingoBoard[1] == true && list_BingoBoard[2] == true)
        {
            horiBingo[0] = true;
            bingoStack++;
        }
        if (horiBingo[1] == false && list_BingoBoard[3] == true && list_BingoBoard[4] == true && list_BingoBoard[5] == true)
        {
            horiBingo[1] = true;
            bingoStack++;
        }
        if (horiBingo[2] == false && list_BingoBoard[6] == true && list_BingoBoard[7] == true && list_BingoBoard[8] == true)
        {
            horiBingo[2] = true;
            bingoStack++;
        }
        //대각선 빙고 체크
        if (crossBingo[0] == false && list_BingoBoard[0] == true && list_BingoBoard[4] == true && list_BingoBoard[8] == true)
        {
            crossBingo[0] = true;
            bingoStack++;
        }
        if (crossBingo[1] == false && list_BingoBoard[2] == true && list_BingoBoard[4] == true && list_BingoBoard[6] == true)
        {
            crossBingo[1] = true;
            bingoStack++;
        }

        int bingoCount = list_BingoBoard.Length;
        //빙고판 다 채워졌는지 체크
        for (int iNum = 0; iNum < bingoCount; iNum++)
        {
            if (list_BingoBoard[iNum] == false)
            {
                //빙고판 중에 안채워진게 있다면 코루틴 종료;
                return;
            }
        }

        isBingoClear = true;



    }

    UnityEngine.Vector2 cryVec;
    float crySpeedMultiFlyer = 4f;
    void Update()
    {
        DownRulletSpinSpeed();
        SlotMachineCryEffect();

        if (Input.GetKeyDown(KeyCode.T))
        {
            GameStatus.inst.MinigameTicket++;
            resetBingo();
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
        setGoldText();
    }



    private void BtnInit()
    {
        selectRulletBtn.onClick.AddListener(() =>
        {
            if (rulletGameRef.activeSelf == false && doSlotMachine == false && isDoBingo == false)
            {
                WorldUI_Manager.inst.FrontUICuttonAction(() =>
                {
                    selectGame(0);
                });
            }

        });

        selectSlotMachineBtn.onClick.AddListener(() =>
        {
            if (slotMachineGameRef.activeSelf == false && doRullet == false && isDoBingo == false)
            {
                WorldUI_Manager.inst.FrontUICuttonAction(() =>
                {
                    selectGame(1);
                });
            }

        });

        selectBingoBtn.onClick.AddListener(() =>
        {
            if (BingoRef.gameObject.activeSelf == false && doRullet == false && doSlotMachine == false)
            {
                WorldUI_Manager.inst.FrontUICuttonAction(() =>
                {
                    selectGame(2);
                });
            }
        });

        exitRulletBtn.onClick.AddListener(() => { if (doRullet == true) { return; } Active_RulletEventShop(false); });
        exitRulletsBtn.onClick.AddListener(() => { if (doSlotMachine == true) { return; } Active_RulletEventShop(false); });
        exitBingoBtn.onClick.AddListener(() => { if (isDoBingo) { return; } Active_RulletEventShop(false); });

        //슬롯머신 일반
        startSlotMachineBtn.onClick.AddListener(() =>
        {
            if (doSlotMachine == true)
            {
                return;
            }
            if (GameStatus.inst.MinigameTicket <= 0)
            {
                AudioManager.inst.Play_Ui_SFX(24, 1f);
                nohaveTiketAnim.SetTrigger("False");
                return;
            }
            if (GameStatus.inst.MinigameTicket > 0)
            {
                GameStatus.inst.MinigameTicket--;
            }

            PlaySlotMachine();

        });

        //슬롯머신 광고
        startadSlotMachineBtn.onClick.AddListener(() =>
        {
            if (doSlotMachine == true)
            {
                return;
            }
            ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
            {
                // 하루 추가
                GameStatus.inst.AdSlotMachineActive = true;
                PlaySlotMachine();
            });
        });

        //룰렛 일반
        startRulletBtn.onClick.AddListener(() =>
        {
            if (doRullet == true)
            {
                return;
            }
            if (GameStatus.inst.MinigameTicket <= 0)
            {
                AudioManager.inst.Play_Ui_SFX(24, 1f);
                nohaveTiketAnim.SetTrigger("False");
                return;
            }
            if (GameStatus.inst.MinigameTicket > 0)
            {
                GameStatus.inst.MinigameTicket--;
            }

            RulletStart();
        });

        //룰렛 광고
        adStartRulletBtn.onClick.AddListener(() =>
        {
            if (doRullet == true)
            {
                return;
            }
            ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
            {
                GameStatus.inst.AdRulletActive = true;
                RulletStart();
            });

        });

        //빙고 일반
        startBingoBtn.onClick.AddListener(() =>
        {
            if (isDoBingo || isBingoClear)
            {
                return;
            }
            if (GameStatus.inst.MinigameTicket <= 0)
            {
                AudioManager.inst.Play_Ui_SFX(24, 1f);
                nohaveTiketAnim.SetTrigger("False");
                return;
            }
            else
            {
                StartCoroutine(PlayBingo());
            }
        });

        //빙고 광고
        startadBingoBtn.onClick.AddListener(() =>
        {
            if (isDoBingo || isBingoClear)
            {
                return;
            }
            ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
            {
                GameStatus.inst.AdBingoActive = true;
                StartCoroutine(PlayBingo());
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
                case 2:
                    startadBingoBtn.interactable = true;
                    bingoAdPlayBtnTextRef[0].SetActive(true);
                    bingoAdPlayBtnTextRef[1].SetActive(false);
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
                case 2:
                    startadBingoBtn.interactable = false;
                    bingoAdPlayBtnTextRef[0].SetActive(false);
                    bingoAdPlayBtnTextRef[1].SetActive(true);
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
    /// 게임 끄고켜기 0 룰렛 / 1 슬롯머신 / 2 빙고
    /// </summary>
    /// <param name="indexNum"></param>

    public void selectGame(int indexNum)
    {
        if (indexNum == 0)
        {
            slotMachineGameRef.SetActive(false);
            BingoRef.gameObject.SetActive(false);
            PlayPandaAnimation(0);
            rulletGameRef.SetActive(true);

        }
        else if (indexNum == 1)
        {
            rulletGameRef.SetActive(false);
            BingoRef.gameObject.SetActive(false);
            PlayPandaAnimation(0);
            slotMachineGameRef.SetActive(true);
        }
        else if (indexNum == 2)
        {
            rulletGameRef.SetActive(false);
            slotMachineGameRef.SetActive(false);
            PlayPandaAnimation(0);
            BingoRef.gameObject.SetActive(true);
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
        AudioSource obj = AudioManager.inst.EventShop_Play_SFX(0, 0.5f);
        slotMachines[0] = StartCoroutine(SlotPlay(0, obj));
        slotMachines[1] = StartCoroutine(SlotPlay(1, obj));
        slotMachines[2] = StartCoroutine(SlotPlay(2, obj));
    }


    float tillingSpeedMultiplyer = 3.8f;
    float slotFloat = 0.168f; // 타일링 1칸

    IEnumerator SlotPlay(int value, AudioSource obj)
    {
        UnityEngine.Vector2 tillingVec = UnityEngine.Vector2.zero;
        float timer = 0f;
        float randomStartValue = UnityEngine.Random.Range(0f, 1f);
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
                obj.Stop();
                //당첨 로직
                RewardItem();
                MaterialTextUpdater();
                RulletAction(false);
            }
            else if (value == 0 && lerpTime > 0.15f && once == false) // 3번쨰 슬롯 멈추면
            {
                once = true;
                AudioManager.inst.EventShop_Play_SFX(1, 1);
            }
            else if (value == 1 && lerpTime > 0.15f && once == false) // 3번쨰 슬롯 멈추면
            {
                once = true;
                AudioManager.inst.EventShop_Play_SFX(2, 1);
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
                    AudioManager.inst.EventShop_Play_SFX(3, 1);

                    if (pair.Key == 0) //루비 2개 당첨
                    {
                        GameStatus.inst.PlusRuby(200); //실행
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " 루비 +200");
                    }
                    else if (pair.Key == 1) // (화약) 뼈 2개 당첨
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(1, 200);
                        GameStatus.inst.Set_crewMaterial(1, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(1), " 스파크의 화약 +200");
                    }
                    else if (pair.Key == 2) // 부적(구 영혼) 2개 당첨
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(0, 200);
                        GameStatus.inst.Set_crewMaterial(0, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(0), " 령화의 부적 +200");
                    }
                    else if (pair.Key == 3) //별 2개 당첨
                    {
                        GameStatus.inst.PlusStar("200");
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), "별 + 200");
                    }
                    else if (pair.Key == 5) //굴소스(구 책) 2개 당첨
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(2, 200);
                        GameStatus.inst.Set_crewMaterial(2, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(2), " 호두의 굴소스 +200");
                    }

                    break;

                //3개 당첨시
                case 3:
                    haveReward = true;
                    PlayPandaAnimation(3);
                    rulletParticle.gameObject.SetActive(true);
                    AudioManager.inst.EventShop_Play_SFX(3, 1);

                    if (pair.Key == 0) //루비 3개 당첨
                    {
                        GameStatus.inst.PlusRuby(500); //실행
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " 루비 +500");
                    }
                    else if (pair.Key == 1) // 화약 뼈 3개 당첨
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(1, 500);
                        GameStatus.inst.Set_crewMaterial(1, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(1), " 스파크의 화약 +500");
                    }
                    else if (pair.Key == 2) //영혼 3개 당첨
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(0, 500);
                        GameStatus.inst.Set_crewMaterial(0, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(0), " 령화의 부적  +500");
                    }
                    else if (pair.Key == 3) //별 3개 당첨
                    {
                        GameStatus.inst.PlusStar("500");
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), " 별 + 500");
                    }
                    else if (pair.Key == 5) //책 3개 당첨
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(2, 500);
                        GameStatus.inst.Set_crewMaterial(5, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(2), " 호두의 굴소스 +500");
                    }
                    break;
            }
        }

        //꽝
        if (haveReward == false)
        {
            Debug.Log("꽝");
            AudioManager.inst.EventShop_Play_SFX(4, 1);
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

    UnityEngine.Vector3 rotZ;
    AudioSource rulletAudio;
    private void RulletStart()
    {
        if (RulletRef.activeSelf == true && doRullet == false)
        {
            rulletParticle.gameObject.SetActive(false);
            RulletAction(true);
            rulletAudio = AudioManager.inst.EventShop_Play_SFX(0, 0.5f);
            StartCoroutine(RulletSpinStart());
        }
    }

    WaitForSeconds rulletDealy = new WaitForSeconds(0.1f);
    IEnumerator RulletSpinStart()
    {
        yield return rulletDealy;
        doRullet = true;
        spinSpeed = 2500f;
        rotZ.z = UnityEngine.Random.Range(0, 360);
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

            rulletPan.transform.Rotate(UnityEngine.Vector3.forward * spinSpeed * Time.deltaTime);

            spinSpeed -= Time.deltaTime * spinSpeedDownMulyfly;
            StartCoroutine(Arrow());

            if (spinSpeed <= 0)
            {
                spinSpeed = 0;
                rulletAudio.Stop();
                // 룰렛 보상
                float checkvalue = rulletPan.transform.eulerAngles.z;
                Debug.Log(checkvalue);

                if ((checkvalue < 30 && checkvalue >= 0) || (330 <= checkvalue))
                {
                    rulletParticle.gameObject.SetActive(true);
                    GameStatus.inst.PlusRuby(10); //실행
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " 루비 +10");
                    AudioManager.inst.EventShop_Play_SFX(3, 0.5f);
                }
                else if (checkvalue >= 30 && checkvalue < 90)
                {
                    Debug.Log("꽝");
                    AudioManager.inst.EventShop_Play_SFX(4, 1f);
                }
                else if (checkvalue >= 90 && checkvalue < 150)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(0, 2, "공격력 버프 2분");
                    AudioManager.inst.EventShop_Play_SFX(3, 0.5f);
                }
                else if (checkvalue >= 150 && checkvalue < 210)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(3, 2, "강한 공격력 버프 2분");
                    AudioManager.inst.EventShop_Play_SFX(3, 0.5f);
                }
                else if (checkvalue >= 210 && checkvalue < 270)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(2, 2, "골드 획득량 증가 버프 2분");
                    AudioManager.inst.EventShop_Play_SFX(3, 0.5f);
                }
                else if (checkvalue >= 270 && checkvalue < 330)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(1, 2, "이동속도 증가 버프 2분");
                    AudioManager.inst.EventShop_Play_SFX(3, 0.5f);
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
        int[] material = GameStatus.inst.CrewMaterial;
        soulText.text = material[0].ToString("N0");
        boneText.text = material[1].ToString("N0");
        bookText.text = material[2].ToString("N0");
        starText.text = CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Star);
        rubyText.text = GameStatus.inst.Ruby.ToString("N0");
    }





    Coroutine ps;
    /// <summary>
    /// Rullet 1회 플레이 (배경 Fade + Particle Effect)
    /// </summary>
    /// <param name="value"></param>
    private void RulletAction(bool value)
    {
        if (ps != null)
        {
            StopCoroutine(ps);
            ps = null;
        }

        ps = StartCoroutine(RulletActionCoru(value));
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

    //////////////////////빙고///////////////////////////

    IEnumerator PlayBingo()
    {
        if (isDoBingo == false)
        {
            isDoBingo = true;
            //비어있는 빙고판 찾기
            List<int> indexList = new List<int>();
            int bingoCount = list_BingoBoard.Length;
            for (int iNum = 0; iNum < bingoCount; iNum++)
            {
                if (list_BingoBoard[iNum] == false)
                {
                    indexList.Add(iNum);
                }
            }
            float time = 0.1f;
            //빙고번호 랜덤 연출
            int randomCount = indexList.Count;

            for (int iNum = 0; iNum < randomCount; iNum++)
            {
                int index = UnityEngine.Random.Range(0, randomCount);
                list_bingo[indexList[index]].SetMaskActive(true);
                AudioManager.inst.Play_Ui_SFX(21, 1);
                yield return new WaitForSeconds(time);
                list_bingo[indexList[index]].SetMaskActive(false);
                time += 0.05f;

            }
            int realindex = UnityEngine.Random.Range(0, randomCount);
            list_bingo[indexList[realindex]].SetMaskActive(true);
            AudioManager.inst.Play_Ui_SFX(21, 1);
            yield return new WaitForSeconds(time);

            //빙고 하나 채워주기
            list_bingo[indexList[realindex]].SetMaskActive(false);
            realindex = UnityEngine.Random.Range(0, randomCount);
            list_BingoBoard[indexList[realindex]] = true;
            list_bingo[indexList[realindex]].SetMaskActive(true);
            yield return new WaitForSeconds(0.5f);
            list_bingo[indexList[realindex]].SetMaskTextActive(true);
            GameStatus.inst.SetBingoBoard(indexList[realindex], true);
            AudioManager.inst.Play_Ui_SFX(22, 1);

            //보상 지급
            list_bingo[indexList[realindex]].GetReward();
            GameStatus.inst.MinigameTicket--;
            yield return new WaitForSeconds(0.5f);

            //Get텍스트 표시
            list_bingo[indexList[realindex]].SetMaskTextActive(true);

            indexList.Clear();

            int beforeStack = bingoStack;
            //빙고 체크
            //수직 빙고 체크
            if (verticalBingo[0] == false && list_BingoBoard[0] == true && list_BingoBoard[3] == true && list_BingoBoard[6] == true)
            {
                verticalBingo[0] = true;
                indexList.Add(0);
                indexList.Add(3);
                indexList.Add(6);
                bingoStack++;
            }
            if (verticalBingo[1] == false && list_BingoBoard[1] == true && list_BingoBoard[4] == true && list_BingoBoard[7] == true)
            {
                verticalBingo[1] = true;
                indexList.Add(1);
                indexList.Add(4);
                indexList.Add(7);
                bingoStack++;
            }
            if (verticalBingo[2] == false && list_BingoBoard[2] == true && list_BingoBoard[5] == true && list_BingoBoard[8] == true)
            {
                verticalBingo[2] = true;
                indexList.Add(2);
                indexList.Add(5);
                indexList.Add(8);
                bingoStack++;
            }

            //수평 빙고 체크
            if (horiBingo[0] == false && list_BingoBoard[0] == true && list_BingoBoard[1] == true && list_BingoBoard[2] == true)
            {
                horiBingo[0] = true;
                indexList.Add(0);
                indexList.Add(1);
                indexList.Add(2);
                bingoStack++;
            }
            if (horiBingo[1] == false && list_BingoBoard[3] == true && list_BingoBoard[4] == true && list_BingoBoard[5] == true)
            {
                horiBingo[1] = true;
                indexList.Add(3);
                indexList.Add(4);
                indexList.Add(5);
                bingoStack++;
            }
            if (horiBingo[2] == false && list_BingoBoard[6] == true && list_BingoBoard[7] == true && list_BingoBoard[8] == true)
            {
                horiBingo[2] = true;
                indexList.Add(6);
                indexList.Add(7);
                indexList.Add(8);
                bingoStack++;
            }

            //대각선 빙고 체크
            if (crossBingo[0] == false && list_BingoBoard[0] == true && list_BingoBoard[4] == true && list_BingoBoard[8] == true)
            {
                crossBingo[0] = true;
                indexList.Add(0);
                indexList.Add(4);
                indexList.Add(8);
                bingoStack++;
            }
            if (crossBingo[1] == false && list_BingoBoard[2] == true && list_BingoBoard[4] == true && list_BingoBoard[6] == true)
            {
                crossBingo[1] = true;
                indexList.Add(2);
                indexList.Add(4);
                indexList.Add(6);
                bingoStack++;
            }

            List<Sprite> rewardSprite = new List<Sprite>();
            List<string> rewardText = new List<string>();
            //빙고 달성 보상 획득 및 연출 준비
            for (int iNum = beforeStack; iNum < bingoStack; iNum++)
            {
                switch (iNum)
                {
                    case 0:
                        BigInteger getgold5 = GameStatus.inst.TotalProdGold * 25;
                        GameStatus.inst.PlusGold(getgold5.ToString());
                        rewardSprite.Add(UIManager.Instance.GetProdSprite(0));
                        rewardText.Add(CalCulator.inst.StringFourDigitAddFloatChanger(getgold5.ToString()));
                        break;
                    case 1:
                        GameStatus.inst.PlusStar("500");
                        rewardSprite.Add(UIManager.Instance.GetProdSprite(1));
                        rewardText.Add("500");
                        break;
                    case 2:
                        GameStatus.inst.PlusRuby(200);
                        rewardSprite.Add(UIManager.Instance.GetProdSprite(2));
                        rewardText.Add("200");
                        break;
                    case 3:
                        BigInteger getgold10 = GameStatus.inst.TotalProdGold * 50;
                        GameStatus.inst.PlusGold(getgold10.ToString());
                        rewardSprite.Add(UIManager.Instance.GetProdSprite(0));
                        rewardText.Add(CalCulator.inst.StringFourDigitAddFloatChanger(getgold10.ToString()));
                        break;
                    case 4:
                        GameStatus.inst.PlusStar("700");
                        rewardSprite.Add(UIManager.Instance.GetProdSprite(1));
                        rewardText.Add("700");
                        break;
                    case 5:
                        GameStatus.inst.PlusRuby(300);
                        rewardSprite.Add(UIManager.Instance.GetProdSprite(2));
                        rewardText.Add("300");
                        break;
                    case 6:
                        BigInteger getgold15 = GameStatus.inst.TotalProdGold * 100;
                        GameStatus.inst.PlusGold(getgold15.ToString());
                        rewardSprite.Add(UIManager.Instance.GetProdSprite(0));
                        rewardText.Add(CalCulator.inst.StringFourDigitAddFloatChanger(getgold15.ToString()));
                        break;
                    case 7:
                        GameStatus.inst.PlusStar("1000");
                        rewardSprite.Add(UIManager.Instance.GetProdSprite(1));
                        rewardText.Add("1A");
                        break;
                }
            }

            yield return new WaitForSeconds(0.5f);
            //빙고달성 연출 재생
            yield return StartCoroutine(BingoEffect(indexList));
            yield return new WaitForSeconds(0.3f);
            isDoBingo = false;

            //빙고 획득 보상 연출 재생
            int effectCount = rewardSprite.Count;
            for (int iNum = 0; iNum < effectCount; iNum++)
            {
                list_ObjBingoReward[iNum].SetActive(true);
                list_ObjBingoRewardText[iNum].text = rewardText[iNum];
                list_ObjBingoRewardImage[iNum].sprite = rewardSprite[iNum];
                float ratio = rewardSprite[iNum].bounds.size.x / rewardSprite[iNum].bounds.size.y;
                list_ObjBingoRewardImage[iNum].rectTransform.sizeDelta = new UnityEngine.Vector2(40 * ratio, 40);
            }
            if (effectCount != 0)
            {
                objShowBingoReward.SetActive(true);
                AudioManager.inst.Play_Ui_SFX(23, 1);
            }

            while (objShowBingoReward.activeSelf)
            {
                yield return null;
            }

            for (int iNum = 0; iNum < list_ObjBingoReward.Count; iNum++)
            {
                list_ObjBingoReward[iNum].SetActive(false);
            }

            //빙고판 다 채워졌는지 체크
            for (int iNum = 0; iNum < bingoCount; iNum++)
            {
                if (list_BingoBoard[iNum] == false)
                {
                    //빙고판 중에 안채워진게 있다면 코루틴 종료;
                    yield break;
                }
            }

            isBingoClear = true;
        }
    }

    //빙고판 리셋
    public void resetBingo()
    {
        int bingoCount = list_BingoBoard.Length;
        for (int iNum = 0; iNum < bingoCount; iNum++)
        {
            setBingoBoard(iNum, false);
            list_bingo[iNum].SetMaskActive(false);
            list_bingo[iNum].SetMaskTextActive(false);
        }
        isDoBingo = false;
        isBingoClear = false;
        verticalBingo[0] = false;
        verticalBingo[1] = false;
        verticalBingo[2] = false;
        horiBingo[0] = false;
        horiBingo[1] = false;
        horiBingo[2] = false;
        crossBingo[0] = false;
        crossBingo[1] = false;
        bingoStack = 0;
        suffleBingo();
    }
    System.Random rnd = new System.Random();
    void suffleBingo()
    {
        int bingoCount = list_BingoBoard.Length;
        for (int iNum = bingoCount - 1; iNum > 0; iNum--)
        {
            int rand = rnd.Next(iNum + 1);
            var objTmep = list_bingo[rand];


            list_bingo[rand] = list_bingo[iNum];


            list_bingo[iNum] = objTmep;

        }

        for (int iNum = 0; iNum < bingoCount; iNum++)
        {
            list_bingo[iNum].GetObj().transform.SetSiblingIndex(iNum);
            GameStatus.inst.SetBingoClass(iNum, list_bingo[iNum].rewardType, list_bingo[iNum].count);
        }
    }

    IEnumerator BingoEffect(List<int> _indexlist)
    {
        int count = _indexlist.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_bingo[_indexlist[iNum]].GetAnim().Play("BingoGetEffect", -1, 0);
            yield return new WaitForSeconds(0.2f);
        }

        //파티클 켜줌
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_bingo[_indexlist[iNum]].SetParticalActive(true);
        }
        if (count != 0)
        {
            yield return new WaitForSeconds(1.5f);
        }

        //파티클 꺼줌
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_bingo[_indexlist[iNum]].SetParticalActive(false);
        }
    }

    void getBingoReward(int index)
    {

        switch (index)
        {
            case 0://루비 100개
                GameStatus.inst.PlusRuby(20);
                break;
            case 1:
                GameStatus.inst.PlusStar("10");
                break;
            case 2:
                BigInteger nowGold2 = GameStatus.inst.TotalProdGold;
                GameStatus.inst.PlusGold($"{nowGold2 * 2}");
                break;
            case 3:
                GameStatus.inst.PlusRuby(30);
                break;
            case 4:
                GameStatus.inst.MinigameTicket++;
                break;
            case 5:
                GameStatus.inst.PlusStar("50");
                break;
            case 6:
                BigInteger nowGold5 = GameStatus.inst.TotalProdGold;
                GameStatus.inst.PlusGold($"{nowGold5 * 5}");
                break;
            case 7:
                GameStatus.inst.PlusStar("100");
                break;
            case 8:
                GameStatus.inst.PlusGold($"{nowGold5 * 10}");
                break;

        }
    }
    void setBingoBoard(int index, bool value)
    {
        list_BingoBoard[index] = value;
        list_bingo[index].SetMaskActive(value);
        list_bingo[index].SetMaskTextActive(value);
        GameStatus.inst.SetBingoBoard(index, value);
    }

    void setGoldText()
    {
        int count = list_bingo.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_bingo[iNum].SetGoldText();
        }
    }
}
