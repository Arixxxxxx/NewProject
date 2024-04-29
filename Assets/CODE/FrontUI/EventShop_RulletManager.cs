using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventShop_RulletManager : MonoBehaviour
{
    public static EventShop_RulletManager inst;

    GameObject frontUIRef, RulletRef;
    GameObject rulletGameRef, slotMachineGameRef;
    GameObject rulletPan;

    //���� ���� ��ư
    Button selectRulletBtn;
    Button selectSlotMachineBtn;

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
    [SerializeField]
    ParticleSystem rulletParticle;

    // ���Ըӽ�
    GameObject slotMachine;

    Material[] slot = new Material[3];
    // ���Ըӽ� ��ư��
    Button exitRulletsBtn, startSlotMachineBtn, startadSlotMachineBtn;
    [SerializeField]
    ParticleSystem[] slotPs = new ParticleSystem[3];

    // �������
    int[] slotNumber = new int[3];
    bool doSlotMachine;

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

        selectRulletBtn = RulletRef.transform.Find("Window/Main/RulletOnBtn").GetComponent<Button>();
        selectSlotMachineBtn = RulletRef.transform.Find("Window/Main/SlotMachineOnBtn").GetComponent<Button>();


        //�귿
        rulletPan = RulletRef.transform.Find("Window/Main/Rullet/Rullet/Pan").gameObject;
        exitRulletBtn = RulletRef.transform.Find("Window/Main/Rullet/RulletBtns/ExitBtn").GetComponent<Button>();
        startRulletBtn = RulletRef.transform.Find("Window/Main/Rullet/RulletBtns/RuuletBtn").GetComponent<Button>();
        adStartRulletBtn = RulletRef.transform.Find("Window/Main/Rullet/RulletBtns/AdRulletBtn").GetComponent<Button>();

        //���Ըӽ�

        slotMachine = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachine").gameObject;
        slot[0] = slotMachine.transform.Find("Slot1/Ver").GetComponent<Image>().material;
        slot[1] = slotMachine.transform.Find("Slot2/Ver").GetComponent<Image>().material;
        slot[2] = slotMachine.transform.Find("Slot3/Ver").GetComponent<Image>().material;
        exitRulletsBtn = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachinetBtns/ExitBtn").GetComponent<Button>();
        startSlotMachineBtn = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachinetBtns/RuuletBtn").GetComponent<Button>();
        startadSlotMachineBtn = RulletRef.transform.Find("Window/Main/SlotMachine/SlotMachinetBtns/AdRulletBtn").GetComponent<Button>();
        slotPs[0] = RulletRef.transform.Find("Window/Main/SlotPs0").GetComponent<ParticleSystem>();
        slotPs[1] = RulletRef.transform.Find("Window/Main/SlotPs1").GetComponent<ParticleSystem>();
        slotPs[2] = RulletRef.transform.Find("Window/Main/SlotPs2").GetComponent<ParticleSystem>();
        rulletParticle = RulletRef.transform.Find("Window/Main/RulletPs").GetComponent<ParticleSystem>();

        soulText = RulletRef.transform.Find("Window/Material/Soul/Text").GetComponent<TMP_Text>();
        boneText = RulletRef.transform.Find("Window/Material/Bone/Text").GetComponent<TMP_Text>();
        bookText = RulletRef.transform.Find("Window/Material/Book/Text").GetComponent<TMP_Text>();
        starText = RulletRef.transform.Find("Window/Material/Star/Text").GetComponent<TMP_Text>();
        rubyText = RulletRef.transform.Find("Window/Material/Ruby/Text").GetComponent<TMP_Text>();
        ticketText = RulletRef.transform.Find("Window/Main/Bot_Text/TicketText").GetComponent<TMP_Text>();

        // ����
        rulletAction = RulletRef.transform.Find("Window/Main/GembleBG").gameObject;
        actionPs = rulletAction.transform.Find("Ps").gameObject;
        actionBackground = RulletRef.transform.Find("Window/Main/GembleBG/BG").GetComponent<Image>();

        BtnInit();
    }
    void Start()
    {

    }

    void Update()
    {
        DownRulletSpinSpeed();
    }

    /// <summary>
    /// �̺�Ʈ�� On/Off
    /// </summary>
    /// <param name="value"></param>
    public void Active_RulletEventShop(bool value)
    {
        RulletRef.SetActive(value);
        MaterialTextUpdater();
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

        //���Ըӽ�
        startSlotMachineBtn.onClick.AddListener(() =>
        {
            if (doSlotMachine == true) { return; }
            PlaySlotMachine();
        });

        startadSlotMachineBtn.onClick.AddListener(() =>
        {
            if (doSlotMachine == true) { return; }
            ADViewManager.inst.SampleAD_Active_Funtion(() => PlaySlotMachine());
        });

        //�귿
        startRulletBtn.onClick.AddListener(() =>
        {
            if (doRullet == true) { return; }
            RulletStart();
        });
        adStartRulletBtn.onClick.AddListener(() =>
        {
            if (doRullet == true) { return; }
            ADViewManager.inst.SampleAD_Active_Funtion(() => RulletStart());
        });


    }

    /// <summary>
    /// ���� �����ѱ� 0 �귿 / 1 ���Ըӽ�
    /// </summary>
    /// <param name="indexNum"></param>

    public void selectGame(int indexNum)
    {
        if (indexNum == 0)
        {
            rulletGameRef.SetActive(true);
            slotMachineGameRef.SetActive(false);
        }
        else if (indexNum == 1)
        {
            rulletGameRef.SetActive(false);
            slotMachineGameRef.SetActive(true);
        }
    }

    ///////////////////////////////////// ���� �ӽ� ////////////////////////////////////////////////
    
    Coroutine[] slotMachines = new Coroutine[3];
    private void PlaySlotMachine()
    {
        doSlotMachine = true;
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
    float slotFloat = 0.17f; // Ÿ�ϸ� 0.17�� 1ĭ

    IEnumerator SlotPlay(int value)
    {
        Vector2 tillingVec = Vector2.zero;
        float timer = 0f;
        float randomStartValue = Random.Range(0f, 1f);
        tillingVec.y = randomStartValue;

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

        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float lerpFactor = lerpTime / duration;
            tillingVec.y = Mathf.Lerp(slot[value].mainTextureOffset.y, finalPosition, lerpFactor);
            slot[value].mainTextureOffset = tillingVec;
            yield return null;
        }

        //slotPs[value].gameObject.SetActive(true);

        if (value == 2)
        {
            doSlotMachine = false;

            //��÷ ����
            RewardItem();
            MaterialTextUpdater();
            RulletAction(false);
        }
    }


    private void RewardItem()
    {

        rulletParticle.Play();

        bool haveReward = false;

        Dictionary<int, int> checkCount = new Dictionary<int, int>();
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
                case 2:
                    haveReward = true;

                    if (pair.Key == 0)
                    {
                        Debug.Log("��� 2�� ��÷");
                        GameStatus.inst.Ruby += 200; //����
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " ��� +200");
                    }
                    else if (pair.Key == 1)
                    {
                        Debug.Log("�� 2�� ��÷");
                        CrewGatchaContent.inst.MaterialCountEditor(1, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(1), " �� +200");
                    }
                    else if (pair.Key == 2)
                    {
                        Debug.Log("��ȥ 2�� ��÷");
                        CrewGatchaContent.inst.MaterialCountEditor(0, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(0), " ��ȥ +200");
                    }
                    else if (pair.Key == 3)
                    {
                        Debug.Log("�� 2�� ��÷");
                        GameStatus.inst.PlusStar("200");
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), "�� + 200");
                    }
                    else if (pair.Key == 5)
                    {
                        Debug.Log("å 2�� ��÷");
                        CrewGatchaContent.inst.MaterialCountEditor(2, 200);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(2), " �� +200");
                    }

                    break;

                case 3:
                    haveReward = true;

                    if (pair.Key == 0)
                    {
                        Debug.Log("��� 3�� ��÷");
                        GameStatus.inst.Ruby += 500; //����
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " ��� +500");
                    }
                    else if (pair.Key == 1)
                    {
                        Debug.Log("�� 3�� ��÷");
                        CrewGatchaContent.inst.MaterialCountEditor(1, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(1), " �� +500");
                    }
                    else if (pair.Key == 2)
                    {
                        Debug.Log("��ȥ 3�� ��÷");
                        CrewGatchaContent.inst.MaterialCountEditor(0, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(0), " ��ȥ +500");
                    }
                    else if (pair.Key == 3)
                    {
                        Debug.Log("�� 3�� ��÷");
                        GameStatus.inst.PlusStar("500");
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(2), " �� + 500");
                    }
                    else if (pair.Key == 5)
                    {
                        Debug.Log("å 3�� ��÷");
                        CrewGatchaContent.inst.MaterialCountEditor(2, 500);
                        WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CrewMaterialIMG(2), " �� +500");
                    }
                    break;
            }
        }

        if (haveReward == false)
        {
            Debug.Log("��");
        }
    }
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

    private void SlotNumberWrite(int value, int Number)
    {
        slotNumber[value] = Number;
    }

    ////////////////////////////////////// �귿 ////////////////////////////////////////

    bool doRullet = false;
    float spinSpeed;
    [SerializeField]
    float spinSpeedDownMulyfly;

    Vector3 rotZ;
    private void RulletStart()
    {
        if (RulletRef.activeSelf == true && doRullet == false)
        {
            
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

    float panCountSize = 72f;
    private void DownRulletSpinSpeed()
    {
        if (doRullet == true)
        {
            rotZ = rulletPan.transform.eulerAngles;
            rotZ.z = Mathf.Repeat(rotZ.z, 360);
            rulletPan.transform.eulerAngles = rotZ;

            rulletPan.transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);

            spinSpeed -= Time.deltaTime * spinSpeedDownMulyfly;

            if (spinSpeed <= 0)
            {
                spinSpeed = 0;

                // �귿 ����
                float checkvalue = rulletPan.transform.eulerAngles.z;
                Debug.Log(checkvalue);

                if (checkvalue >= 0 && checkvalue < panCountSize)
                {
                    Debug.Log("��� +10");
                    GameStatus.inst.Ruby += 10; //����
                    WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), " ��� +10");
                }
                else if (checkvalue >= panCountSize && checkvalue < panCountSize * 2)
                {
                    Debug.Log("���ݷ� ����");
                    BuffManager.inst.ActiveBuff(0, 2, "���ݷ� ���� 2��");
                    //WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.BuffIMG(0),"���ݷ� ���� 2��");
                }
                else if (checkvalue >= panCountSize * 2 && checkvalue < panCountSize * 3)
                {
                    Debug.Log("���� ���ݷ� ����");
                    BuffManager.inst.ActiveBuff(3, 2, "���� ���ݷ� ���� 2��");
                }
                else if (checkvalue >= panCountSize * 3 && checkvalue < panCountSize * 4)
                {
                    Debug.Log("��� ����");
                    BuffManager.inst.ActiveBuff(2, 2, "��� ȹ�淮 ���� ���� 2��");
                }
                else if (checkvalue >= panCountSize * 4 && checkvalue < panCountSize * 5)
                {
                    Debug.Log("�̼� ����");
                    BuffManager.inst.ActiveBuff(1, 2, "�̵��ӵ� ���� ���� 2��");
                }

                MaterialTextUpdater();
                RulletAction(false);
                doRullet = false;

            }
        }
    }

    /// <summary>
    /// UI ��� ���׸��� �������� (�ֽ�ȭ)
    /// </summary>
    private void MaterialTextUpdater()
    {
        int[] material = CrewGatchaContent.inst.Get_CurCrewUpgreadMaterial();
        soulText.text = material[0].ToString("N0");
        boneText.text = material[1].ToString("N0");
        bookText.text = material[2].ToString("N0");
        starText.text = CalCulator.inst.StringFourDigitAddFloatChanger(GameStatus.inst.Star);
        rubyText.text = GameStatus.inst.Ruby.ToString("N0");
        ticketText.text = $"�̺�Ʈ Ƽ�� ���� : {GameStatus.inst.MinigameTicket}";
    }





    /// <summary>
    /// ��� Fade + Particle Effect
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
                
                if(actionBackground.color.a < 0.7f && actionPs.activeSelf == true)
                {
                    actionPs.SetActive(false);
                }
                yield return null;
            }
            
            rulletAction.SetActive(false);
        }
    }
}
