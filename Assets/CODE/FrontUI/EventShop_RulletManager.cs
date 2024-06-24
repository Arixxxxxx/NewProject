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

    //���� ���� ��ư
    Button selectRulletBtn;
    Button selectSlotMachineBtn;
    Button selectBingoBtn;

    //��� ��� Text �κ�
    TMP_Text soulText;
    TMP_Text boneText;
    TMP_Text bookText;
    TMP_Text starText;
    TMP_Text rubyText;

    // �ϴ� Ƽ�� Text 
    TMP_Text ticketText;

    // ����
    GameObject rulletAction;
    GameObject actionPs;
    Image actionBackground;

    // �귿â ��ư��
    Button exitRulletBtn, startRulletBtn, adStartRulletBtn;

    ParticleSystem rulletParticle;

    // ���Ըӽ�
    GameObject slotMachine;
    Animator head_Anim;

    GameObject[] headListObj = new GameObject[5];
    Material pandaTear;
    Animator winHand_Anim;
    GameObject waitHand;

    Material[] slot = new Material[3];
    // ���Ըӽ� ��ư��
    Button exitRulletsBtn, startSlotMachineBtn, startadSlotMachineBtn;

    // �ϴ� ���� Ƽ�� �ؽ�Ʈ�κ�
    GameObject haveTicket, nohaveTiket;
    Animator nohaveTiketAnim;

    // �귿
    Animator rulletArrowAnim;

    // ����
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
            //���� �ؽ�Ʈ �ʱ�ȭ
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
                    countText.text = "Ƽ��";
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
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(1), "��� " + CalCulator.inst.StringFourDigitAddFloatChanger(prodGold.ToString()) + "��");
                    break;
                case ProductTag.Star:
                    GameStatus.inst.PlusStar(count.ToString());
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), "�� " + CalCulator.inst.StringFourDigitAddFloatChanger(count.ToString()) + "��");
                    break;
                case ProductTag.Ruby:
                    GameStatus.inst.PlusRuby(count);
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "��� " + CalCulator.inst.StringFourDigitAddFloatChanger(count.ToString()) + "��");
                    break;
                case ProductTag.MiniGameTicket:
                    GameStatus.inst.MinigameTicket += count;
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(3), "�̺�ƮƼ�� " + count.ToString() + "��");
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

    // ���� ��ư��
    Button exitBingoBtn, startBingoBtn, startadBingoBtn;

    // �������
    int[] slotNumber = new int[3];
    bool doSlotMachine;

    GameObject[] slotMachineAdPlayBtnTextRef = new GameObject[2];
    GameObject[] rulletMachineAdPlayBtnTextRef = new GameObject[2];
    GameObject[] bingoAdPlayBtnTextRef = new GameObject[2];


    // �׽�Ʈ�� ġƮ��ư
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


        //�귿
        rulletPan = RulletRef.transform.Find("Window/Main/Rullet/Rullet/Pan").gameObject;
        exitRulletBtn = RulletRef.transform.Find("Window/Main/Rullet/RulletBtns/ExitBtn").GetComponent<Button>();
        startRulletBtn = RulletRef.transform.Find("Window/Main/Rullet/RulletBtns/RuuletBtn").GetComponent<Button>();
        adStartRulletBtn = RulletRef.transform.Find("Window/Main/Rullet/RulletBtns/AdRulletBtn").GetComponent<Button>();
        rulletMachineAdPlayBtnTextRef[0] = adStartRulletBtn.transform.Find("True").gameObject;
        rulletMachineAdPlayBtnTextRef[1] = adStartRulletBtn.transform.Find("False").gameObject;

        rulletArrowAnim = RulletRef.transform.Find("Window/Main/Rullet/Rullet/Arrow").GetComponent<Animator>();

        //���Ըӽ�

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

        //����
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

        // ����
        rulletAction = RulletRef.transform.Find("Window/Main/GembleBG").gameObject;
        actionPs = rulletAction.transform.Find("Ps").gameObject;
        actionBackground = RulletRef.transform.Find("Window/Main/GembleBG/BG").GetComponent<Image>();

        // �ϴ� ����
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
        //���� ���� üũ
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
        //���� ���� üũ
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
        //�밢�� ���� üũ
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
        //������ �� ä�������� üũ
        for (int iNum = 0; iNum < bingoCount; iNum++)
        {
            if (list_BingoBoard[iNum] == false)
            {
                //������ �߿� ��ä������ �ִٸ� �ڷ�ƾ ����;
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
    /// �̺�Ʈ�� On/Off
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

        //���Ըӽ� �Ϲ�
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

        //���Ըӽ� ����
        startadSlotMachineBtn.onClick.AddListener(() =>
        {
            if (doSlotMachine == true)
            {
                return;
            }
            ADViewManager.inst.AdMob_ActiveAndFuntion(() =>
            {
                // �Ϸ� �߰�
                GameStatus.inst.AdSlotMachineActive = true;
                PlaySlotMachine();
            });
        });

        //�귿 �Ϲ�
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

        //�귿 ����
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

        //���� �Ϲ�
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

        //���� ����
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
    /// AD���� ��ư Ȱ�� / ��Ȱ��ȭ ���ִ� �Լ�
    /// </summary>
    /// <param name="gameNumber"></param>
    /// <param name="Active"></param>
    public void AdPlayButtonInit(int gameNumber, bool Active)
    {
        // true �� ������ ��Ȱ��ȭ, false�� Ȱ��ȭ
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
            ticketText.text = $"�̺�Ʈ Ƽ�� ���� : {GameStatus.inst.MinigameTicket}";
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
    /// ���� �����ѱ� 0 �귿 / 1 ���Ըӽ� / 2 ����
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

    ///////////////////////////////////// ���� �ӽ� ////////////////////////////////////////////////

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
    float slotFloat = 0.168f; // Ÿ�ϸ� 1ĭ

    IEnumerator SlotPlay(int value, AudioSource obj)
    {
        UnityEngine.Vector2 tillingVec = UnityEngine.Vector2.zero;
        float timer = 0f;
        float randomStartValue = UnityEngine.Random.Range(0f, 1f);
        tillingVec.y = randomStartValue;
        rulletParticle.gameObject.SetActive(false);


        // ���� ���ư��� �ð� ����

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

        // Lerp�� ����� �ε巯�� ����
        float finalPosition = DetermineFinalPosition(slot[value].mainTextureOffset.y, value);
        float lerpTime = 0f;
        float duration = 1f; // Lerp�� �Ϸ�Ǵ� �� �ʿ��� �ð�
        bool once = false;

        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float lerpFactor = lerpTime / duration;
            tillingVec.y = Mathf.Lerp(slot[value].mainTextureOffset.y, finalPosition, lerpFactor);
            slot[value].mainTextureOffset = tillingVec;



            if (value == 2 && lerpTime > 0.15f && once == false) // 3���� ���� ���߸�
            {
                once = true;
                obj.Stop();
                //��÷ ����
                RewardItem();
                MaterialTextUpdater();
                RulletAction(false);
            }
            else if (value == 0 && lerpTime > 0.15f && once == false) // 3���� ���� ���߸�
            {
                once = true;
                AudioManager.inst.EventShop_Play_SFX(1, 1);
            }
            else if (value == 1 && lerpTime > 0.15f && once == false) // 3���� ���� ���߸�
            {
                once = true;
                AudioManager.inst.EventShop_Play_SFX(2, 1);
            }

            yield return null;
        }

        if (value == 2) // 3���� ���� ���߸�
        {
            doSlotMachine = false;
        }
    }

    Dictionary<int, int> checkCount = new Dictionary<int, int>();

    /// <summary>
    /// ������ ��÷ �з� �Լ�
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
                //2�� ��÷��

                case 2:
                    haveReward = true;
                    PlayPandaAnimation(2);
                    rulletParticle.gameObject.SetActive(true);
                    AudioManager.inst.EventShop_Play_SFX(3, 1);

                    if (pair.Key == 0) //��� 2�� ��÷
                    {
                        GameStatus.inst.PlusRuby(200); //����
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " ��� +200");
                    }
                    else if (pair.Key == 1) // (ȭ��) �� 2�� ��÷
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(1, 200);
                        GameStatus.inst.Set_crewMaterial(1, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(1), " ����ũ�� ȭ�� +200");
                    }
                    else if (pair.Key == 2) // ����(�� ��ȥ) 2�� ��÷
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(0, 200);
                        GameStatus.inst.Set_crewMaterial(0, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(0), " ��ȭ�� ���� +200");
                    }
                    else if (pair.Key == 3) //�� 2�� ��÷
                    {
                        GameStatus.inst.PlusStar("200");
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), "�� + 200");
                    }
                    else if (pair.Key == 5) //���ҽ�(�� å) 2�� ��÷
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(2, 200);
                        GameStatus.inst.Set_crewMaterial(2, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(2), " ȣ���� ���ҽ� +200");
                    }

                    break;

                //3�� ��÷��
                case 3:
                    haveReward = true;
                    PlayPandaAnimation(3);
                    rulletParticle.gameObject.SetActive(true);
                    AudioManager.inst.EventShop_Play_SFX(3, 1);

                    if (pair.Key == 0) //��� 3�� ��÷
                    {
                        GameStatus.inst.PlusRuby(500); //����
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " ��� +500");
                    }
                    else if (pair.Key == 1) // ȭ�� �� 3�� ��÷
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(1, 500);
                        GameStatus.inst.Set_crewMaterial(1, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(1), " ����ũ�� ȭ�� +500");
                    }
                    else if (pair.Key == 2) //��ȥ 3�� ��÷
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(0, 500);
                        GameStatus.inst.Set_crewMaterial(0, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(0), " ��ȭ�� ����  +500");
                    }
                    else if (pair.Key == 3) //�� 3�� ��÷
                    {
                        GameStatus.inst.PlusStar("500");
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), " �� + 500");
                    }
                    else if (pair.Key == 5) //å 3�� ��÷
                    {
                        //CrewGatchaContent.inst.MaterialCountEditor(2, 500);
                        GameStatus.inst.Set_crewMaterial(5, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(2), " ȣ���� ���ҽ� +500");
                    }
                    break;
            }
        }

        //��
        if (haveReward == false)
        {
            Debug.Log("��");
            AudioManager.inst.EventShop_Play_SFX(4, 1);
            PlayPandaAnimation(4);
        }
    }

    // ���Ըӽ� ���ߴ� �������� ��ȯ
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


    // ���Ըӽ� ��÷ ��ȣ ����
    private void SlotNumberWrite(int value, int Number)
    {
        slotNumber[value] = Number;
    }



    /// <summary>
    /// �� �ִϸ��̼� �۵� �� Active �۵�
    /// </summary>
    /// <param name="value"> 0 = End / 1 = Play / 2 = ����2 / 3 =����3 / 4 = ����(��) </param>
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

    // �ָ�, ���� On Off
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

    // ���� Effect
    private void SlotMachineCryEffect()
    {
        if (cry)
        {
            cryVec.y += Time.deltaTime * crySpeedMultiFlyer;
            cryVec.y = Mathf.Repeat(cryVec.y, 1);
            pandaTear.mainTextureOffset = cryVec;
        }
    }

    ////////////////////////////////////// �귿 ////////////////////////////////////////

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


    // �귿�� �ӵ�����
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
                // �귿 ����
                float checkvalue = rulletPan.transform.eulerAngles.z;
                Debug.Log(checkvalue);

                if ((checkvalue < 30 && checkvalue >= 0) || (330 <= checkvalue))
                {
                    rulletParticle.gameObject.SetActive(true);
                    GameStatus.inst.PlusRuby(10); //����
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " ��� +10");
                    AudioManager.inst.EventShop_Play_SFX(3, 0.5f);
                }
                else if (checkvalue >= 30 && checkvalue < 90)
                {
                    Debug.Log("��");
                    AudioManager.inst.EventShop_Play_SFX(4, 1f);
                }
                else if (checkvalue >= 90 && checkvalue < 150)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(0, 2, "���ݷ� ���� 2��");
                    AudioManager.inst.EventShop_Play_SFX(3, 0.5f);
                }
                else if (checkvalue >= 150 && checkvalue < 210)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(3, 2, "���� ���ݷ� ���� 2��");
                    AudioManager.inst.EventShop_Play_SFX(3, 0.5f);
                }
                else if (checkvalue >= 210 && checkvalue < 270)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(2, 2, "��� ȹ�淮 ���� ���� 2��");
                    AudioManager.inst.EventShop_Play_SFX(3, 0.5f);
                }
                else if (checkvalue >= 270 && checkvalue < 330)
                {
                    rulletParticle.gameObject.SetActive(true);
                    BuffManager.inst.ActiveBuff(1, 2, "�̵��ӵ� ���� ���� 2��");
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

    // Animator�� ���� ���°� 'stateName'���� Ȯ��
    private bool IsPlaying(string ClipName)
    {
        return rulletArrowAnim.GetCurrentAnimatorStateInfo(0).IsName(ClipName) &&
                     rulletArrowAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
    }

    /// <summary>
    /// UI ��� ���׸��� �������� (�ֽ�ȭ)
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
    /// Rullet 1ȸ �÷��� (��� Fade + Particle Effect)
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

    //////////////////////����///////////////////////////

    IEnumerator PlayBingo()
    {
        if (isDoBingo == false)
        {
            isDoBingo = true;
            //����ִ� ������ ã��
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
            //�����ȣ ���� ����
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

            //���� �ϳ� ä���ֱ�
            list_bingo[indexList[realindex]].SetMaskActive(false);
            realindex = UnityEngine.Random.Range(0, randomCount);
            list_BingoBoard[indexList[realindex]] = true;
            list_bingo[indexList[realindex]].SetMaskActive(true);
            yield return new WaitForSeconds(0.5f);
            list_bingo[indexList[realindex]].SetMaskTextActive(true);
            GameStatus.inst.SetBingoBoard(indexList[realindex], true);
            AudioManager.inst.Play_Ui_SFX(22, 1);

            //���� ����
            list_bingo[indexList[realindex]].GetReward();
            GameStatus.inst.MinigameTicket--;
            yield return new WaitForSeconds(0.5f);

            //Get�ؽ�Ʈ ǥ��
            list_bingo[indexList[realindex]].SetMaskTextActive(true);

            indexList.Clear();

            int beforeStack = bingoStack;
            //���� üũ
            //���� ���� üũ
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

            //���� ���� üũ
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

            //�밢�� ���� üũ
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
            //���� �޼� ���� ȹ�� �� ���� �غ�
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
            //����޼� ���� ���
            yield return StartCoroutine(BingoEffect(indexList));
            yield return new WaitForSeconds(0.3f);
            isDoBingo = false;

            //���� ȹ�� ���� ���� ���
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

            //������ �� ä�������� üũ
            for (int iNum = 0; iNum < bingoCount; iNum++)
            {
                if (list_BingoBoard[iNum] == false)
                {
                    //������ �߿� ��ä������ �ִٸ� �ڷ�ƾ ����;
                    yield break;
                }
            }

            isBingoClear = true;
        }
    }

    //������ ����
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

        //��ƼŬ ����
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_bingo[_indexlist[iNum]].SetParticalActive(true);
        }
        if (count != 0)
        {
            yield return new WaitForSeconds(1.5f);
        }

        //��ƼŬ ����
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_bingo[_indexlist[iNum]].SetParticalActive(false);
        }
    }

    void getBingoReward(int index)
    {

        switch (index)
        {
            case 0://��� 100��
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
