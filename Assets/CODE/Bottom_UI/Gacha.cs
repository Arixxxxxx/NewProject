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
    [SerializeField] List<Image> list_resultImage = new List<Image>();

    List<GameObject> list_haveRelic = new List<GameObject>();

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

    IEnumerator gachaEffect(int GachaCount)
    {
        int imagecount = list_resultImage.Count;
        for (int iNum = 0; iNum < imagecount; iNum++)
        {
            list_resultImage[iNum].gameObject.SetActive(false); ;
        }

        List<Sprite> ListResultSprite = new List<Sprite>();//���� ����̹��� ����

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

            int objPercent = UnityEngine.Random.Range(0, objCount);
            GameObject targetObj = aryRankClass[rankNum].AryObj[objPercent];

            int SerchCount = list_haveRelic.Count;
            bool ishave = false;
            int haveObjNum = -1;
            for (int iNum = 0; iNum < SerchCount; iNum++)//��ũ�� ������ ��í
            {
                if (list_haveRelic[iNum].GetComponent<ITypeGetable>().GetMyType() == targetObj.GetComponent<ITypeGetable>().GetMyType())
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
        int count = ListResultSprite.Count;
        gachaResultObj.SetActive(true);
        for (int iNum = 0; iNum < count; iNum++)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            list_resultImage[iNum].sprite = ListResultSprite[iNum];
            list_resultImage[iNum].gameObject.SetActive(true);
        }
    }

    public void clickGacha(int GachaCount)
    {
        StartCoroutine(gachaEffect(GachaCount));
    }
}
