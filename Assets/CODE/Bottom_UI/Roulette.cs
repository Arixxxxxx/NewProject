using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Numerics;
using TMPro;

public class Roulette : MonoBehaviour
{
    [SerializeField] float setspd;
    [SerializeField] float despd;
    [SerializeField] List<RewardList> list_reward = new List<RewardList>();
    [Serializable]
    public class RewardList
    {
        [SerializeField] ProductTag rewardType;
        [SerializeField] int count;

        public ProductTag GetRewardType()
        {
            return rewardType;
        }

        public int GetCount()
        {
            return count;
        }
    }

    Image[] list_showImage;
    TMP_Text[] list_GetText;
    TMP_Text[] list_CountText;
    Transform BingoParents;
    Transform ShowRewardParents;
    Image roulette;
    Image player;
    Button openBtn;
    Button closeBtn;
    Button StartBtn;
    Button StopBtn;
    RectTransform rouletteRect;
    Animator animator;
    TMP_Text goldText2;
    TMP_Text goldText5;
    TMP_Text goldText10;

    BigInteger nowTotalGold;
    float speed;
    bool isSpin;

    bool[] bingoBoard = new bool[8];
    List<GameObject> bingoMask = new List<GameObject>();


    private void Start()
    {
        Transform canvas = GameObject.Find("---[UI Canvas]").transform;
        roulette = transform.Find("BackGround/Roulette").GetComponent<Image>();
        player = transform.Find("BackGround/Player").GetComponent<Image>();
        openBtn = canvas.Find("RouletteBtn").GetComponent<Button>();
        StartBtn = transform.Find("BackGround/StartBtn").GetComponent<Button>();
        StopBtn = transform.Find("BackGround/StopBtn").GetComponent<Button>();
        closeBtn = transform.Find("BackGround/CloseBtn").GetComponent<Button>();
        goldText2 = transform.Find("BackGround/Roulette/Gold2/Text (TMP)").GetComponent<TMP_Text>();
        goldText5 = transform.Find("BackGround/Roulette/Gold5/Text (TMP)").GetComponent<TMP_Text>();
        goldText10 = transform.Find("BackGround/Roulette/Gold10/Text (TMP)").GetComponent<TMP_Text>();

        //∫˘∞Ì∆« √ ±‚»≠
        BingoParents = transform.Find("BackGround/bingo");
        int BingoCount = BingoParents.childCount;
        for (int iNum = 0; iNum < BingoCount; iNum++)
        {
            bingoMask.Add(BingoParents.GetChild(iNum).Find("Mask").gameObject);
        }
        bingoMask.RemoveAt(4);

        //∫∏ªÛ »Æ¿Œ√¢ √ ±‚»≠
        ShowRewardParents = transform.Find("BackGround/ShowReward");
        int ShowCount = ShowRewardParents.childCount;
        list_showImage = new Image[BingoCount];
        list_GetText = new TMP_Text[BingoCount];
        list_CountText = new TMP_Text[BingoCount];
        for (int iNum = 0; iNum < ShowCount; iNum++)
        {
            list_showImage[iNum] = ShowRewardParents.GetChild(iNum).Find("Image").GetComponent<Image>();
            list_GetText[iNum] = ShowRewardParents.GetChild(iNum).Find("GetText").GetComponent<TMP_Text>();
            list_CountText[iNum] = ShowRewardParents.GetChild(iNum).Find("CountText").GetComponent<TMP_Text>();

        }
        clickOpen();
        rouletteRect = roulette.GetComponent<RectTransform>();
        animator = player.GetComponent<Animator>();
        openBtn.onClick.AddListener(clickOpen);
        StartBtn.onClick.AddListener(clickStart);
        StopBtn.onClick.AddListener(clickStop);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        rouletteRect.eulerAngles = UnityEngine.Vector3.zero;
        StartBtn.gameObject.SetActive(true);
        StartBtn.interactable = true;
        StopBtn.gameObject.SetActive(false);
        StopBtn.interactable = true;
    }

    private void Update()
    {
        spin();
    }

    void spin()
    {
        if (isSpin)
        {
            rouletteRect.eulerAngles = new UnityEngine.Vector3(0, 0, rouletteRect.eulerAngles.z + speed * Time.deltaTime);
        }
    }

    void clickOpen()
    {
        nowTotalGold = GameStatus.inst.TotalProdGold;
        goldText2.text = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * 2).ToString());
        goldText5.text = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * 5).ToString());
        goldText10.text = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * 10).ToString());
    }

    void clickStart()
    {
        isSpin = true;
        animator.SetBool("spin", isSpin);
        speed = setspd;
        StartBtn.gameObject.SetActive(false);
        StopBtn.gameObject.SetActive(true);
        closeBtn.interactable = false;
    }

    void clickStop()
    {
        StartCoroutine(stopRoulett(10));
        StopBtn.interactable = false;
    }

    IEnumerator stopRoulett(int count)
    {
        //∫∏ªÛ ø¿∫Í¡ß∆Æ ∏µŒ ≤®¡÷±‚
        int ShowCount = ShowRewardParents.childCount;
        for (int iNum = 0; iNum < ShowCount; iNum++)
        {
            ShowRewardParents.GetChild(iNum).gameObject.SetActive(false);

        }

        //∞πºˆ∏∏≈≠ ∑Í∑ø µπ∏∞ »ƒ ¿˙¿Â
        List<UnityEngine.Vector2> ListRewardNum = new List<UnityEngine.Vector2>();        
        for (int iNum = 0; iNum < count; iNum++)
        {
            int rewardNumCount = ListRewardNum.Count;
            int value = UnityEngine.Random.Range(0, 8);
            int index = -1;
            //¿ÃπÃ »πµÊ«ﬂ¥¬¡ˆ »Æ¿Œ
            for (int jNum = 0; jNum < rewardNumCount; jNum++)
            {
                if (ListRewardNum[jNum].x == value)
                {
                    index = jNum;
                    break;
                }
            }
            //»πµÊ«œ¡ˆ æ æ“¥Ÿ∏È ªı∑Œ √ﬂ∞°
            if (index == -1)
            {
                ListRewardNum.Add(new UnityEngine.Vector2(value, 0));
            }
            //»πµÊ«ﬂ¥Ÿ∏È y∞™¿ª 1ø√∑¡¡‹
            else
            {
                ListRewardNum[index] = new UnityEngine.Vector2(ListRewardNum[index].x, ListRewardNum[index].y + 1);
            }

            switch (value)
            {
                case 0:
                    GameStatus.inst.Ruby += 100;
                    break;
                case 1:
                    GameStatus.inst.PlusStar("100");
                    break;
                case 2:
                    GameStatus.inst.PlusGold($"{nowTotalGold * 2}");
                    break;
                case 3:
                    GameStatus.inst.Ruby += 200;
                    break;
                case 4:
                    GameStatus.inst.PlusStar("1000");
                    break;
                case 5:
                    GameStatus.inst.PlusGold($"{nowTotalGold * 5}");
                    break;
                case 6:
                    GameStatus.inst.PlusStar("10000");
                    break;
                case 7:
                    GameStatus.inst.PlusGold($"{nowTotalGold * 10}");
                    break;
            }            
        }

        //∏Ò«• »∏¿¸∞™ ∞ËªÍ
        float targetRot = -22.5f + 45 * ListRewardNum[0].x;
        if (targetRot < 0)
        {
            targetRot = 360 + targetRot;
        }

        //∏Ò«• »∏¿¸∞™ µµ¥ﬁ ¥Î±‚
        while (Mathf.Abs(rouletteRect.eulerAngles.z - targetRot) >= 2)
        {
            yield return null;
        }

        //¡°¬˜ º”µµ ∞®º“
        while (speed > 0)
        {
            speed -= despd * Time.deltaTime;
            yield return null;
        }

        //∑Í∑ø ∏ÿ√· »ƒ «ÿæﬂµ… ¿œµÈ

        int rewardCount = ListRewardNum.Count;
        //∫˘∞Ì∆« »πµÊ√≥∏Æ
        for (int iNum = 0; iNum < rewardCount; iNum++)
        {
            bingoBoard[(int)ListRewardNum[iNum].x] = true;
            bingoMask[(int)ListRewardNum[iNum].x].SetActive(true);
        }

        speed = 0;
        isSpin = false;
        animator.SetBool("spin", isSpin);
        ShowRewardParents.gameObject.SetActive(true);

        for (int iNum = 0; iNum < rewardCount; iNum++)
        {
            int index = (int)ListRewardNum[iNum].x;

            //Ω∫«¡∂Û¿Ã∆Æ ±≥√º
            Sprite sprite = UIManager.Instance.GetProdSprite((int)list_reward[index].GetRewardType());
            list_showImage[iNum].sprite = sprite;
            float ratio = sprite.bounds.size.x / sprite.bounds.size.y;
            list_showImage[iNum].rectTransform.sizeDelta = new UnityEngine.Vector2(ratio * 40, 40);

            //»πµÊ ∞πºˆ ≈ÿΩ∫∆Æ ºˆ¡§
            switch (list_reward[index].GetRewardType())
            {
                case ProductTag.Gold:
                    list_CountText[iNum].text = CalCulator.inst.StringFourDigitAddFloatChanger((nowTotalGold * list_reward[index].GetCount()).ToString());
                    break;
                case ProductTag.Star:
                    list_CountText[iNum].text = CalCulator.inst.StringFourDigitAddFloatChanger(list_reward[index].GetCount().ToString());
                    break;
                case ProductTag.Ruby:
                    list_CountText[iNum].text = list_reward[index].GetCount().ToString();
                    break;
            }

            //¡ﬂ∫π »πµÊ ∞πºˆ ≈ÿΩ∫∆Æ ºˆ¡§
            if (ListRewardNum[iNum].y == 0)
            {
                list_GetText[iNum].text = $"Get!";
            }
            else
            {
                list_GetText[iNum].text = $"x{ListRewardNum[iNum].y + 1} Get!";
            }

            ShowRewardParents.GetChild(iNum).gameObject.SetActive(true);
        }

        closeBtn.interactable = true;
        StopBtn.interactable = true;
        StopBtn.gameObject.SetActive(false);
        StartBtn.gameObject.SetActive(true);
    }

    void checkBingo()
    {
        //∞°∑Œ∫˘∞Ì
        if (bingoBoard[0] && bingoBoard[1] && bingoBoard[2])
        {

        }

        if (bingoBoard[3] && bingoBoard[4])
        {

        }

        if (bingoBoard[5] && bingoBoard[6] && bingoBoard[7])
        {

        }

        //ºº∑Œ∫˘∞Ì
        if (bingoBoard[0] && bingoBoard[3] && bingoBoard[5])
        {

        }

        if (bingoBoard[1] && bingoBoard[6])
        {

        }

        //¥Î∞¢∫˘∞Ì
        if (bingoBoard[2] && bingoBoard[4] && bingoBoard[7])
        {

        }

        if (bingoBoard[0] && bingoBoard[7])
        {

        }

        if (bingoBoard[2] && bingoBoard[5])
        {

        }
    }
}
