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
    [SerializeField] Image imageIcon;
    [SerializeField] Button moveBtn;
    [SerializeField] Button clearBtn;
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text rewardText;
    [SerializeField] TMP_Text BarText;
    [SerializeField] Image imageBar;
    [SerializeField] GameObject Mask;
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
        transform.SetAsLastSibling();
        Mask.SetActive(true);
    }
}
