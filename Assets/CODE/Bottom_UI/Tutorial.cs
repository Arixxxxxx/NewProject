using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Tutorial : MonoBehaviour
{
    public static Tutorial inst;

    public UnityEvent onStartTutorial;
    public UnityEvent onStopTutorial;

    GameObject tutorialintro;
    List<GameObject> list_QuestTutorial = new List<GameObject>();
    List<GameObject> list_WeaponTutorial = new List<GameObject>();
    List<GameObject> list_PetTutorial = new List<GameObject>();
    List<GameObject> list_RelicTutorial = new List<GameObject>();
    List<GameObject> list_HwanseangTutorial = new List<GameObject>();
    List<IEnumerator> list_Coroutine = new List<IEnumerator>();

    Button QuestBtn;
    Button WeaponBtn;
    Button PetBtn;
    Button RelicBtn;
    Button QuestLv1Btn;
    Button WeaponLv1Btn;
    Button HwanSeangBtn;
    Button RelicShopBtn;

    Transform TutorialParents;
    Button skipBtn;

    IEnumerator nowCoroutine;

    bool isClick;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        initRef();
        initButton();

        GameStatus.inst.OnStageChanged.AddListener(() => 
        {
            Debug.Log(GameStatus.inst.StageLv);
            if (GameStatus.inst.StageLv == 30 && GameStatus.inst.IsFirst30Stage)
            {
                GameStatus.inst.IsFirst30Stage = false;
                PlayTutorial(0, 0, 0, 0, 1);
                Debug.Log("30스테이지 달성!");
            }
        });
    }

    void initRef()
    {
        TutorialParents = transform.parent.Find("ScreenArea/Tutorial");
        tutorialintro = TutorialParents.Find("Intro").gameObject;
        list_QuestTutorial.Add(TutorialParents.Find("Quest").gameObject);
        list_QuestTutorial.Add(TutorialParents.Find("Quest (1)").gameObject);
        list_QuestTutorial.Add(TutorialParents.Find("Quest (2)").gameObject);
        list_WeaponTutorial.Add(TutorialParents.Find("Weapon").gameObject);
        list_WeaponTutorial.Add(TutorialParents.Find("Weapon (1)").gameObject);
        list_WeaponTutorial.Add(TutorialParents.Find("Weapon (2)").gameObject);
        list_PetTutorial.Add(TutorialParents.Find("Pet").gameObject);
        list_PetTutorial.Add(TutorialParents.Find("Pet (1)").gameObject);
        list_RelicTutorial.Add(TutorialParents.Find("Relic").gameObject);
        list_RelicTutorial.Add(TutorialParents.Find("Relic (1)").gameObject);
        list_RelicTutorial.Add(TutorialParents.Find("Relic (2)").gameObject);
        list_HwanseangTutorial.Add(TutorialParents.Find("Hwanseang").gameObject);
        list_HwanseangTutorial.Add(TutorialParents.Find("Hwanseang (1)").gameObject);

        QuestBtn = transform.parent.Find("ScreenArea/BackGround/BottomBtn/Quest").GetComponent<Button>();
        WeaponBtn = transform.parent.Find("ScreenArea/BackGround/BottomBtn/Weapon").GetComponent<Button>();
        PetBtn = transform.parent.Find("ScreenArea/BackGround/BottomBtn/Pet").GetComponent<Button>();
        RelicBtn = transform.parent.Find("ScreenArea/BackGround/BottomBtn/Relic").GetComponent<Button>();

        QuestLv1Btn = transform.parent.Find("ScreenArea/BackGround/Quest/Scroll View/Viewport/Content/Quest/LvUpBtn").GetComponent<Button>();
        WeaponLv1Btn = transform.parent.Find("ScreenArea/BackGround/Weapon/Scroll View/Viewport/Content/Weapon/LvUpBtn").GetComponent<Button>();
        HwanSeangBtn = GameObject.Find("---[World UI Canvas]").transform.Find("StageUI/HwanSeng").GetComponent<Button>();
        RelicShopBtn = transform.parent.Find("ScreenArea/BackGround/Relic/GotoRelicShopBtn").GetComponent<Button>();

        skipBtn = TutorialParents.Find("SkipBtn").GetComponent<Button>();
    }

    void initButton()
    {
        skipBtn.onClick.AddListener(skipTutorial);
        QuestBtn.onClick.AddListener(() => { isClick = true; });
        WeaponBtn.onClick.AddListener(() => { isClick = true; });
        PetBtn.onClick.AddListener(() => { isClick = true; });
        RelicBtn.onClick.AddListener(() => { isClick = true; });
        HwanSeangBtn.onClick.AddListener(() => { isClick = true; });
        QuestLv1Btn.onClick.AddListener(() => { isClick = true; });
        WeaponLv1Btn.onClick.AddListener(() => { isClick = true; });
        RelicShopBtn.onClick.AddListener(() => { isClick = true; });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            PlayTutorial(1, 1, 1, 1, 0);
        }
    }
    /// <summary>
    /// 0 = 해당 튜토리얼 제외, 1 = 해당 튜토리얼 포함 재생
    /// </summary>
    /// <param name="Quest"></param>
    /// <param name="Weapon"></param>
    /// <param name="Pet"></param>
    /// <param name="Relic"></param>
    /// <param name="HwanSeang"></param>
    public void PlayTutorial(int Quest, int Weapon, int Pet, int Relic, int HwanSeang)
    {

        if (Quest == 1)
        {
            list_Coroutine.Add(QuestTutorial());
        }
        if (Weapon == 1)
        {
            list_Coroutine.Add(WeaponTutorial());
        }
        if (Pet == 1)
        {
            list_Coroutine.Add(PetTutorial());
        }
        if (Relic == 1)
        {
            list_Coroutine.Add(RelicTutorial());
        }
        if (HwanSeang == 1)
        {
            list_Coroutine.Add(HwanseangTutorial());
        }
        onStartTutorial?.Invoke();
        nowCoroutine = coPlayTutorial();
        StartCoroutine(nowCoroutine);

    }

    void skipTutorial()
    {
        StopAllCoroutines();
        int count = TutorialParents.childCount;
        for (int iNum = 0; iNum < count - 1; iNum++)
        {
            TutorialParents.GetChild(iNum).gameObject.SetActive(false);
        }
        list_Coroutine.Clear();
        TutorialParents.gameObject.SetActive(false);
        UIManager.Instance.changeSortOder(12);
    }

    IEnumerator coPlayTutorial()
    {
        TutorialParents.gameObject.SetActive(true);
        UIManager.Instance.changeSortOder(17);
        int count = list_Coroutine.Count;
        for (int iNum = 0; iNum < count; iNum++)
        {
            yield return StartCoroutine(list_Coroutine[iNum]);
        }
        onStopTutorial?.Invoke();
        list_Coroutine.Clear();
        TutorialParents.gameObject.SetActive(false);
        UIManager.Instance.changeSortOder(12);
    }

    IEnumerator QuestTutorial()
    {
        tutorialintro.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        while (Input.anyKeyDown == false)
        {
            yield return null;
        }
        tutorialintro.SetActive(false);

        list_QuestTutorial[0].SetActive(true);
        isClick = false;
        while (isClick == false)
        {
            yield return null;

        }

        list_QuestTutorial[0].SetActive(false);
        list_QuestTutorial[1].SetActive(true);

        isClick = false;
        while (isClick == false)
        {
            yield return null;

        }

        list_QuestTutorial[1].SetActive(false);
        list_QuestTutorial[2].SetActive(true);
        while (Input.anyKeyDown == false)
        {
            yield return null;
        }
        list_QuestTutorial[2].SetActive(false);
    }
    IEnumerator WeaponTutorial()
    {
        list_WeaponTutorial[0].SetActive(true);

        isClick = false;
        while (isClick == false)
        {
            yield return null;

        }
        list_WeaponTutorial[0].SetActive(false);
        list_WeaponTutorial[1].SetActive(true);
        isClick = false;
        while (isClick == false)
        {
            yield return null;

        }
        list_WeaponTutorial[1].SetActive(false);
        list_WeaponTutorial[2].SetActive(true);
        while (Input.anyKeyDown == false)
        {
            yield return null;
        }
        list_WeaponTutorial[2].SetActive(false);
    }

    IEnumerator PetTutorial()
    {
        list_PetTutorial[0].SetActive(true);
        isClick = false;
        while (isClick == false)
        {
            yield return null;

        }
        list_PetTutorial[0].SetActive(false);
        list_PetTutorial[1].SetActive(true);
        while (Input.anyKeyDown == false)
        {
            yield return null;
        }
        list_PetTutorial[1].SetActive(false);
    }
    IEnumerator RelicTutorial()
    {
        list_RelicTutorial[0].SetActive(true);
        isClick = false;
        while (isClick == false)
        {
            yield return null;

        }
        list_RelicTutorial[0].SetActive(false);
        list_RelicTutorial[1].SetActive(true);
        isClick = false;
        while (isClick == false)
        {
            yield return null;
        }
        list_RelicTutorial[1].SetActive(false);
        list_RelicTutorial[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        while (Input.anyKeyDown == false)
        {
            yield return null;
        }
        list_RelicTutorial[2].SetActive(false);
        UIManager.Instance.ClickBotBtn(0);
    }

    IEnumerator HwanseangTutorial()
    {
        list_HwanseangTutorial[0].SetActive(true);
        while (Input.anyKeyDown == false)
        {
            yield return null;
        }
        list_HwanseangTutorial[0].SetActive(false);
        list_HwanseangTutorial[1].SetActive(true);
        isClick = false;
        while (isClick == false)
        {
            yield return null;

        }
        list_HwanseangTutorial[1].SetActive(false);
    }
}
