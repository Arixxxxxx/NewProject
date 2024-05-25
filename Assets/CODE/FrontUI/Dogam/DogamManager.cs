using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DogamManager : MonoBehaviour
{
    public static DogamManager inst;

    [Tooltip(" # 0 ���� ���� ������ \n # 1 ���� ���� ������ ")]
    [SerializeField] GameObject[] slotPrefabs;
    GameObject[] slotParentTrs = new GameObject[2];

    // ���� ������ �� ��ũ��Ʈ
    Sprite[] WeaponSprite;
    DogamWeaponSlot[] weaponSlotsSc;
    int curWeaponNumber;
    // ���� ������ ��ũ��Ʈ



    //���� ����
    GameObject worldUIRef, frontUiRef, dogamMainRef, windowRef;
    GameObject[] bottomViewr = new GameObject[2];
    Button xBtn;
    Button[] topArrayBtn = new Button[2];
    Image[] topButtonImg;
    TMP_Text[] topBtnText;

    // ���� ����
    Image viewrBox_WeaponIMG;
    TMP_Text[] weaponInfoText = new TMP_Text[2];

    GameObject maskObj;
    TMP_Text bottomText;

    Image weaponeSelectCutton;


    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(inst);
        }

        AwakeInit();
        BtnInit();
    }

    private void AwakeInit()
    {
        worldUIRef = GameManager.inst.WorldUiRef;
        frontUiRef = GameManager.inst.FrontUiRef;
        dogamMainRef = frontUiRef.transform.Find("Dogam").gameObject;
        windowRef = dogamMainRef.transform.Find("Window").gameObject;
        bottomViewr[0] = windowRef.transform.Find("Weapon").gameObject;
        bottomViewr[1] = windowRef.transform.Find("Monster").gameObject;


        xBtn = windowRef.transform.Find("X_Btn").GetComponent<Button>();
        topArrayBtn = windowRef.transform.Find("TopBtnArray").GetComponentsInChildren<Button>();
        topButtonImg = new Image[topArrayBtn.Length];

        //��� ����,���� ��ư
        topBtnText = new TMP_Text[topArrayBtn.Length];
        for (int index = 0; index < topArrayBtn.Length; index++)
        {
            topButtonImg[index] = topArrayBtn[index].GetComponent<Image>();
            topBtnText[index] = topArrayBtn[index].GetComponentInChildren<TMP_Text>();
        }

        slotParentTrs[0] = bottomViewr[0].transform.Find("WeaponList/Scroll View/Viewport/Content").gameObject;
        slotParentTrs[1] = bottomViewr[1].transform.Find("EnemyList/Scroll View/Viewport/Content").gameObject;


        // ����
        viewrBox_WeaponIMG = bottomViewr[0].transform.Find("WeaponMainInfo/BG/Weapon").GetComponent<Image>();
        weaponInfoText[0] = bottomViewr[0].transform.Find("WeaponMainInfo/Box/WeaponText").GetComponent<TMP_Text>();
        weaponInfoText[1] = bottomViewr[0].transform.Find("WeaponMainInfo/Box/WeaponInfoText").GetComponent<TMP_Text>();

        weaponeSelectCutton = bottomViewr[0].transform.Find("WeaponMainInfo/BG/Cutton").GetComponent<Image>();

        bottomText = bottomViewr[0].transform.Find("BottomInfo/BottomBox/BottomText").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        Slot_Prefabs_Init();
    }
    private void BtnInit()
    {
        xBtn.onClick.AddListener(() => Active_DogamUI(false));
        topArrayBtn[0].onClick.AddListener(() => Acitve_Bottom_Viewr(0));
        topArrayBtn[1].onClick.AddListener(() => Acitve_Bottom_Viewr(1));
    }

    private void Slot_Prefabs_Init()
    {
        // Awake���� ���� �������� �޾ƿ;ߵ� (�������� �����)

        // ���� ���� �ʱ�ȭ
        WeaponSprite = SpriteResource.inst.Weapons;
        weaponSlotsSc = new DogamWeaponSlot[WeaponSprite.Length];

        GameObject obj = null;

        for (int index = 0; index < WeaponSprite.Length; index++)
        {
            obj = Instantiate(slotPrefabs[0], slotParentTrs[0].transform);
            weaponSlotsSc[index] = obj.GetComponent<DogamWeaponSlot>();
            weaponSlotsSc[index].Init_Prefabs(WeaponSprite[index], index);

        }

        // ���� ���� �ʱ�ȭ
    }




    int[] curWeaponLv;

    /// <summary>
    /// ���� ����â Ȱ��ȭ
    /// </summary>
    /// <param name="value"></param>
    public void Active_DogamUI(bool value)
    {
        if (value == true)
        {
            //������ ���� �ϴܿ� �����͵� ������ Ƚ���� ������
            //MasterWeaponCheker();
        }
        else
        {
            WorldUI_Manager.inst.RawImagePlayAcitve(false);
        }

        dogamMainRef.SetActive(value);
        Acitve_Bottom_Viewr(0); // �⺻���� �ʱ�ȭ
        WeaponInitBtn(); // �⺻�ʱ�ȭ

    }

    // ���� ������ Ƚ�� üĿ
    int masterWeaponCount = 0;
    public void MasterWeaponCheker()
    {
        curWeaponLv = GameStatus.inst.GetAryWeaponLv().ToArray();
        int weaponMasterCount = 0;
        for (int index = 0; index < curWeaponLv.Length; index++)
        {
            if (curWeaponLv[index] == 5)
            {
                weaponMasterCount++;
                weaponSlotsSc[index].MaskActiveFalse();
            }
            else if (curWeaponLv[index] < 5)
            {
                break;
            }
        }

        masterWeaponCount = weaponMasterCount;
        bottomText.text = $"< �߰� ���ݷ� {weaponMasterCount}%��ŭ ��� >   ���� ���� ���� ( {weaponMasterCount} / {curWeaponLv.Length} )";
    }



    Color fadeColor = new Color(0.3f, 0.3f, 0.3f, 1);
    private void Acitve_Bottom_Viewr(int value)
    {
        for (int index = 0; index < topArrayBtn.Length; index++)
        {
            if (index == value)
            {
                bottomViewr[index].gameObject.SetActive(true);
                topButtonImg[index].color = Color.white;
                topBtnText[index].color = Color.white;
            }
            else
            {
                bottomViewr[index].gameObject.SetActive(false);
                topButtonImg[index].color = fadeColor;
                topBtnText[index].color = fadeColor;
            }
        }

        if (value == 0)
        {
            WorldUI_Manager.inst.RawImagePlayAcitve(1, true);
        }
        else
        {
            WorldUI_Manager.inst.RawImagePlayAcitve(false);
        }
    }

    public void WeaponInitBtn()
    {
        viewrBox_WeaponIMG.sprite = WeaponSprite[0];
        string temp = weaponNameAndInfo[0];
        curWeaponNumber = 0;
        if (weaponSlotsSc[0].master)
        {
            weaponInfoText[0].text = temp.Split('-')[0];
            weaponInfoText[1].text = temp.Split('-')[1];
        }
        else
        {
            weaponInfoText[0].text = "? ? ?";
            weaponInfoText[1].text = " ȹ���غ����ʾƼ�.. \n��������..";
        }
    }


    // �ڽ� �������� ���õǾ����� ���̵� �ξƿ��Ǹ鼭 �̹��� ��ü
    bool once = false;
    public void Set_WeaponMainViewr(int childrenNumber)
    {
        if (once || curWeaponNumber == childrenNumber) { return; } // �ٲ���ִ����̰ų� ���� ��ȣ�̸� Return
        once = true;

        curWeaponNumber = childrenNumber;
        StartCoroutine(Change(childrenNumber));

    }

    float duration = 0.25f;
    float cuttonTimer = 0f;
    WaitForSeconds changeWaitTime = new WaitForSeconds(0.1f);
    Color zeroColor = new Color(0, 0, 0, 0);
    IEnumerator Change(int chidrenNumber)
    {
        cuttonTimer = 0;

        while (cuttonTimer < duration)
        {
            float alphaZ = Mathf.Lerp(0f, 1f, cuttonTimer / duration);
            weaponeSelectCutton.color = new Color(weaponeSelectCutton.color.r, weaponeSelectCutton.color.g, weaponeSelectCutton.color.b, alphaZ);
            cuttonTimer += Time.deltaTime;
            yield return null;
        }

        weaponeSelectCutton.color = Color.black;
        viewrBox_WeaponIMG.sprite = WeaponSprite[chidrenNumber];
        string temp = weaponNameAndInfo[chidrenNumber];

        // ȹ���غþ�� ����!
        if (weaponSlotsSc[chidrenNumber].master)
        {
            weaponInfoText[0].text = temp.Split('-')[0];
            weaponInfoText[1].text = temp.Split('-')[1];
        }
        else
        {
            weaponInfoText[0].text = "? ? ?";
            weaponInfoText[1].text = " ȹ���غ����ʾƼ�.. \n��������..";
        }

        yield return changeWaitTime;

        cuttonTimer = 0;

        while (cuttonTimer < duration)
        {
            float alphaZ = Mathf.Lerp(1f, 0f, cuttonTimer / duration);
            weaponeSelectCutton.color = new Color(weaponeSelectCutton.color.r, weaponeSelectCutton.color.g, weaponeSelectCutton.color.b, alphaZ);
            cuttonTimer += Time.deltaTime;
            yield return null;
        }
        weaponeSelectCutton.color = zeroColor;
        once = false;
    }


    //float getChance = 5f; // <�� ���� óġ�� �������� ���� Ȯ��

    /// <summary>
    /// ���� óġ�� �������� ��� �Լ�
    /// </summary>
    /// <param name="dogamIndex"></param>
    public void MosterDogamIndexValueUP(int stage, int enemyIndex)
    {
        //float randomValue = Random.Range(0,100f);
        //if (randomValue > getChance) { return; };

        //Sprite IMG = SpriteResource.inst.CoinIMG(3);
        //string text = $"{stage}-{enemyIndex} ��������";
        //WorldUI_Manager.inst.Get_ItemInfomation_UI_Active(IMG, text);
        //int indexNumber = ((stage-1) * 5) + enemyIndex;
        //mosterCollectionConut[indexNumber]++; // �� �ø�

    }

    string[] weaponNameAndInfo =
        {
            "�׵�-�볪���� ���� ���̴� ������ ���� �ʺ����� ���̴�",    //0
             "��-�ճ��� ���ѳ����� ���� ��\n���� �̻����� �߰��ϴ�",     //1
             "�߽ĵ�-ȣ�ΰ� ��������� ���\n�ٴϴ� �߽ĵ�",    //2 
             "�յ���-�� ������ �� ������",         //3 
             "��-�� ����� ��°� �γ���\n������ ��ɰ� ���� �������.\nƯ�� ���� �������",       //4 
             "����-��� 3���̸� ������ �ٷ��",        //5 
             "�ڵ�-�߱� �ϼ۴���� ����������� ���� �ΰ��� ������ �ʰ� ����� �������� �߱���",       //6 
             "�߱�â-�� ������� ���� ����� ����\r\n��⿡ Ưȭ�Ǿ��ִ�",         //7
            "�����-��⺸�ٴ� ���⿡ ����\r\n�ϵ��� ������� ����..",       //8 
            "ȭ����-���� ��ϰ� �οﶧ ����\r\n��ä�� �� ��� �� ����.......",         //9
            "����-3�� ����� �ϳ���.\r\n�ٸ� 2�ڷ�� ���� �Ϻ��� \r\n�ѱ��� �ִ�",         // 10
            "ȣ�α�-��� �߱����� ������ ����..\r\n������ �� Į���̴�",        //11
            "���Ǻ�-��ó ������ �����̰�\r\n������ ��� ������",        //12
            "���Ȼ��-�к����� �������� ���Դٰ�\r\n�ΰ� �� â�̴�",         //13
            "û������-�� ���� �ı� ���� ���ƿ´�\r\n���ݾƿ�",     //14
            "Ź��ä-���� ü������ �Ҿƹ�����\r\n���� Ź��ä",       //15
            "���̺�ä-������� ���� ��ä�� ��ϸ�\r\n���� ���󸸵� ��ä",         //16
            "������-100% �� ���ڱ⿡ ġ��, \r\n��Ŭ, ����, �丶��, ���ĸ�\r\n���� ���� ���� �ž�... ",       //17
            "���ڷ�-���� û�Ҵ�� ������?",        //18
            "�汤��-���� �� ���� �Ĵ�",         //19
            "������-�츮 ���̰� �����ϴ� ������",        //20
            "�վ-�� �Ⱑ����",         //21
            "�и�ġ-�и�ġ�� ���⸻�� ����\r\n�־��ٴ°� ��� .. ����",       //22
            "����-�Ǵٰ� �־�� �� ���� ��Ÿ���� �ʴ´ٸ� �� ������ ��� �ǰڽ��ϱ�?",     //23
            "����Ǳ�-�Ǵٰ� ���� �뷡�ϰ� �־�!\r\n�� ������ ��ü ����?!",         //24
            "û��ġ-�ϳ� ���� û��ġ���ٿ�",           //25
            "¯��-�ƺ��� �� ��������",           //26
            "�������-��Ʈ��",            //27
            "���ķ�-ġ���ǻ���� ���� ����� \r\n���",            //28
            "�˸����-���� �������� ���ϳ�?\r\n�������� ������",         //29
        };


}
