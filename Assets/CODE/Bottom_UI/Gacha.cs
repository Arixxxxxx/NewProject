using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Gacha : MonoBehaviour
{
    [SerializeField] GachaRank[] aryRankClass;
    [SerializeField] Transform RelicParents;

    [SerializeField] GameObject gachaResultObj;
    [SerializeField] Transform ResultImageParents;
    [SerializeField] Button allOpenBtn;
    [SerializeField] Button OkBtn;
    List<GaChaEffect> list_resultImage = new List<GaChaEffect>();

    List<GameObject> list_haveRelic = new List<GameObject>();
    int openCount = 0;
    int maxOpenCount = 10;
    Button adRelicBtn;

    [Serializable]
    class GachaRank
    {
        [Header("확률범위 최소값")]
        [SerializeField] float minPercentage;
        [Header("확률범위 최대값")]
        [SerializeField] float maxPercentage;

        [SerializeField] GameObject[] aryObj;
        public GameObject[] AryObj
        {
            get => aryObj;
        }

        public bool CheckPercentage(float _percent)
        {
            if (_percent >= minPercentage && _percent < maxPercentage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void Start()
    {
        int count = ResultImageParents.childCount;
        for (int iNum = 0; iNum < count; iNum++)
        {
            GaChaEffect sc = ResultImageParents.GetChild(iNum).GetComponent<GaChaEffect>();
            list_resultImage.Add(sc);
            sc.GetOpenBtn().onClick.AddListener(() => SetOpenCount());

        }
        allOpenBtn.onClick.AddListener(() => ClickAllOpen());
        OkBtn.onClick.AddListener(() =>
        {
            OkBtn.gameObject.SetActive(false);
            gachaResultObj.SetActive(false);
        });

        adRelicBtn = transform.Find("Btns/RelicGachaBtn (2)").GetComponent<Button>();
        adRelicBtn.onClick.AddListener(() =>
        {
            ADViewManager.inst.SampleAD_Active_Funtion(() => clickGacha(10));
        });
    }

    IEnumerator gachaEffect(int GachaCount)
    {
        if (list_haveRelic.Count == 0)
        {
            UIManager.Instance.SetGotoGachaBtn(false);
        }
        int imagecount = list_resultImage.Count;
        for (int iNum = 0; iNum < imagecount; iNum++)
        {
            list_resultImage[iNum].gameObject.SetActive(false); ;
        }

        List<Sprite> ListResultSprite = new List<Sprite>();//연차 결과이미지 저장
        List<int> ListRank = new List<int>();

        for (int jNum = 0; jNum < GachaCount; jNum++)//연차 결과 연산
        {
            float rankPercent = UnityEngine.Random.Range(0, 100);
            int forcount = aryRankClass.Length;
            int rankNum = -1;
            for (int iNum = 0; iNum < forcount; iNum++)//랭크 가챠
            {
                if (aryRankClass[iNum].CheckPercentage(rankPercent) == true)
                {
                    rankNum = iNum;
                    break;
                }
            }

            int objCount = aryRankClass[rankNum].AryObj.Length;

            int objPercent = UnityEngine.Random.Range(0, objCount);//랭크내 아이템 가챠
            GameObject targetObj = aryRankClass[rankNum].AryObj[objPercent];

            int SerchCount = list_haveRelic.Count;
            bool ishave = false;
            int haveObjNum = -1;
            Vector2 targetType = targetObj.GetComponent<Relic>().GetMyType();
            ListRank.Add((int)targetType.x);
            for (int iNum = 0; iNum < SerchCount; iNum++)//같은 아이템이 있는지 비교
            {
                if (list_haveRelic[iNum].GetComponent<Relic>().GetMyType() == targetType)
                {
                    ishave = true;
                    haveObjNum = iNum;
                    break;
                }
            }

            if (ishave)//같은 아이템을 가지고 있다면 1레벨 업
            {
                Relic sc = list_haveRelic[haveObjNum].GetComponent<Relic>();
                sc.Lv += 1;
                ListResultSprite.Add(sc.GetSprite());
            }
            else//아니면 아이템 획득
            {
                GameObject obj = Instantiate(aryRankClass[rankNum].AryObj[objPercent], RelicParents);
                Relic sc = obj.GetComponent<Relic>();
                sc.initRelic();
                sc.Lv += 1;
                list_haveRelic.Add(obj);
                ListResultSprite.Add(sc.GetSprite());
            }
        }
        sortingRelicList();
        int count = ListResultSprite.Count;
        gachaResultObj.SetActive(true);
        for (int iNum = 0; iNum < count; iNum++)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            list_resultImage[iNum].Setsprite(ListResultSprite[iNum], (RankType)ListRank[iNum]);
            list_resultImage[iNum].gameObject.SetActive(true);
        }
        Invoke("allOpenBtnActive", 1f);
    }

    public void clickGacha(int GachaCount)
    {
        StartCoroutine(gachaEffect(GachaCount));
        maxOpenCount = GachaCount;
    }

    public void ClickAllOpen()
    {
        allOpenBtn.gameObject.SetActive(false);
        int count = list_resultImage.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            list_resultImage[iNum].GetOpenBtn().onClick?.Invoke();
        }

    }

    void sortingRelicList()
    {
        list_haveRelic.Sort(compareRelic);
        int count = list_haveRelic.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            int indexNum = (int)list_haveRelic[iNum].GetComponent<Relic>().GetMyType().y;
            list_haveRelic[iNum].transform.SetSiblingIndex(indexNum);
        }
    }

    int compareRelic(GameObject A, GameObject B)
    {
        if (A.GetComponent<Relic>().GetMyType().y < B.GetComponent<Relic>().GetMyType().y)
        {
            return -1;
        }
        else
        {
            return 1;
        }
        
    }

    void allOpenBtnActive()
    {
        allOpenBtn.gameObject.SetActive(true);
    }

    public void OkBtnActive()
    {
        OkBtn.gameObject.SetActive(true);
    }

    void SetOpenCount()
    {
        openCount++;
        if (openCount >= maxOpenCount)
        {
            openCount = 0;
            allOpenBtn.gameObject.SetActive(false);
            Invoke("OkBtnActive", 1f);
        }
    }
}
