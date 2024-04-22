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
        [Header("Ȯ������ �ּҰ�")]
        [SerializeField] float minPercentage;
        [Header("Ȯ������ �ִ밪")]
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

        List<Sprite> ListResultSprite = new List<Sprite>();//���� ����̹��� ����
        List<int> ListRank = new List<int>();

        for (int jNum = 0; jNum < GachaCount; jNum++)//���� ��� ����
        {
            float rankPercent = UnityEngine.Random.Range(0, 100);
            int forcount = aryRankClass.Length;
            int rankNum = -1;
            for (int iNum = 0; iNum < forcount; iNum++)//��ũ ��í
            {
                if (aryRankClass[iNum].CheckPercentage(rankPercent) == true)
                {
                    rankNum = iNum;
                    break;
                }
            }

            int objCount = aryRankClass[rankNum].AryObj.Length;

            int objPercent = UnityEngine.Random.Range(0, objCount);//��ũ�� ������ ��í
            GameObject targetObj = aryRankClass[rankNum].AryObj[objPercent];

            int SerchCount = list_haveRelic.Count;
            bool ishave = false;
            int haveObjNum = -1;
            Vector2 targetType = targetObj.GetComponent<Relic>().GetMyType();
            ListRank.Add((int)targetType.x);
            for (int iNum = 0; iNum < SerchCount; iNum++)//���� �������� �ִ��� ��
            {
                if (list_haveRelic[iNum].GetComponent<Relic>().GetMyType() == targetType)
                {
                    ishave = true;
                    haveObjNum = iNum;
                    break;
                }
            }

            if (ishave)//���� �������� ������ �ִٸ� 1���� ��
            {
                Relic sc = list_haveRelic[haveObjNum].GetComponent<Relic>();
                sc.Lv += 1;
                ListResultSprite.Add(sc.GetSprite());
            }
            else//�ƴϸ� ������ ȹ��
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
