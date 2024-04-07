using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mission : MonoBehaviour
{
    [SerializeField] public string Name;
    [SerializeField] int maxCount;
    [SerializeField] string rewardCount;
    [SerializeField] ProductTag rewardTag;
     Image imageIcon;
     Button moveBtn;
     Button clearBtn;
     TMP_Text NameText;
     TMP_Text rewardText;
     TMP_Text BarText;
     Image imageBar;
    int count;
    public int Count
    {
        get => count;
        set
        {
            if (count < maxCount)
            {
                count = value;

                BarText.text = $"{count} / {maxCount}";
                imageBar.fillAmount = (float)Count / maxCount;
                if (count >= maxCount)
                {
                    count = maxCount;
                    moveBtn.gameObject.SetActive(false);
                    clearBtn.gameObject.SetActive(true);
                    if (Name == "일일미션 클리어")
                    {
                        MissionData.Instance.SetWeeklyMission("일일미션 모두 클리어", 1);
                    }
                }
            }
        }
    }

    private void Start()
    {
        imageIcon = transform.Find("Icon_IMG").GetComponent<Image>();
        moveBtn = transform.Find("MoveBtn").GetComponent<Button>();
        clearBtn = transform.Find("ClearBtn").GetComponent<Button>();
        NameText = transform.Find("Space/Title_Text").GetComponent<TMP_Text>();
        rewardText = transform.Find("Space/ReturnItem_Text").GetComponent<TMP_Text>();
        BarText = transform.Find("Space/Playbar/Text (TMP)").GetComponent<TMP_Text>();
        imageBar = transform.Find("Space/Playbar/PlayBar(Front)").GetComponent<Image>();

        NameText.text = Name;
        
        switch (rewardTag)
        {
            case ProductTag.Gold:
                rewardText.text = $"골드 +{rewardCount}개";
                break;
            case ProductTag.Ruby:
                rewardText.text = $"루비 +{rewardCount}개";
                break;
            case ProductTag.Star:
                rewardText.text = $"별 +{rewardCount}개";
                break;
        }
        BarText.text = $"{count} / {maxCount}";
        imageBar.fillAmount = (float)Count / maxCount;
        imageIcon.sprite = UIManager.Instance.GetProdSprite((int)rewardTag);
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
}
