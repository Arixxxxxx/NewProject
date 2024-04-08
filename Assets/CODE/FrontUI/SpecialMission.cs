using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialMission : MonoBehaviour
{
    [SerializeField] public string Name;
    [SerializeField] public int MissionIndex;
    [SerializeField] public MissionType missionType;
    [SerializeField] int index;
    [SerializeField] int maxCount;
    [SerializeField] string rewardCount;
    [SerializeField] ProductTag rewardTag;
    [SerializeField] Image imageIcon;
    [SerializeField] Button moveBtn;
    [SerializeField] Button clearBtn;
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text rewardText;
    int count;
    public int Count
    {
        get => count;
        set
        {
            if (count < maxCount)
            {
                count = value;
                if (count >= maxCount)
                {
                    clearBtn.gameObject.SetActive(true);
                }
            }
        }
    }

    private void Start()
    {
        //imageIcon = transform.Find("Icon_IMG").GetComponent<Image>();
        //moveBtn = transform.Find("MoveBtn").GetComponent<Button>();
        //clearBtn = transform.Find("ClearBtn").GetComponent<Button>();
        //NameText = transform.Find("Space/MissionText").GetComponent<TMP_Text>();
        //rewardText = transform.Find("Space/RewardText").GetComponent<TMP_Text>();

        //NameText.text = Name + $"{maxCount}달성";

        //switch (rewardTag)
        //{
        //    case ProductTag.Gold:
        //        rewardText.text = $"골드 +{rewardCount}개";
        //        break;
        //    case ProductTag.Ruby:
        //        rewardText.text = $"루비 +{rewardCount}개";
        //        break;
        //    case ProductTag.Star:
        //        rewardText.text = $"별 +{rewardCount}개";
        //        break;
        //}
        //imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);
    }

    public void ClickClearBtn()
    {
        switch (rewardTag)
        {
            case ProductTag.Gold:
                GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(rewardCount));
                break;
            case ProductTag.Ruby:
                GameStatus.inst.Ruby += int.Parse(CalCulator.inst.ConvertChartoIndex(rewardCount));
                break;
            case ProductTag.Star:
                GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(rewardCount));
                break;
        }
        clearBtn.gameObject.SetActive(false);
    }

    public void CheckMIssion(int index)
    {
        if (GameStatus.inst.GetAryQuestLv(index) >= maxCount - 1)
        {
            clearBtn.gameObject.SetActive(true);
        }
    }
}
