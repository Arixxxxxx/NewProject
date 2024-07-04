using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CartoonManager : MonoBehaviour
{
    public static CartoonManager inst;
    GameObject cartoonRedf, toonRef, endTextRef;
    GameObject[] toons; // 만화항목

    //만화 목록
    Image[] openToon, sparkToon, hoduToon, ryungToon;

    public bool isPlaying;
    private void Awake()
    {
        #region
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion

        cartoonRedf = GameManager.inst.FrontUiRef.transform.Find("Cartoon").gameObject;
        toonRef = cartoonRedf.transform.Find("Toon").gameObject;
        endTextRef = cartoonRedf.transform.Find("End").gameObject;

        int toonCount = toonRef.transform.childCount;

        toons = new GameObject[toonCount];
        for (int index = 0; index < toonCount; index++)
        {
            toons[index] = toonRef.transform.GetChild(index).gameObject;

            switch (index) // 추후에 만화가 추가되면 여기서 등록
            {
                case 0:
                    openToon = toons[index].transform.GetComponentsInChildren<Image>(true);
                    break;

                case 1:
                    sparkToon = toons[index].transform.GetComponentsInChildren<Image>(true);
                    break;

                case 2:
                    hoduToon = toons[index].transform.GetComponentsInChildren<Image>(true);
                    break;

                case 3:
                    ryungToon = toons[index].transform.GetComponentsInChildren<Image>(true);
                    break;
            }
        }
    }

    
    /// <summary>
    /// 만화 재생
    /// </summary>
    /// <param name="index"> 0오프닝 / 1스파크 / 2호두 / 3령화 </param>
    public void Cartoon_Active(int index)
    {
        isPlaying = true;
        AudioManager.inst.Set_VoulemMute("SFX", true);
        cartoonRedf.SetActive(true);
        toons[index].SetActive(true);
        StartCoroutine(PlayToon(index));
    }

    Color alphaZero = new Color(1, 1, 1, 0);
    WaitForSeconds nextDealy = new WaitForSeconds(1f);
    WaitForSeconds firstDealy = new WaitForSeconds(0.5f);
    float fadetime = 0.8f;
    float timer = 0f;
    bool click = false;
    IEnumerator PlayToon(int index)
    {
        Image[] playList = null;

        switch (index) // 추후에 만화가 추가되면 여기서 등록
        {
            case 0:
                playList = openToon;
                break;

            case 1:
                playList = sparkToon;
                break;

            case 2:
                playList = hoduToon;
                break;

            case 3:
                playList = ryungToon;
                break;
        }

        // 사용전 초기화
        for (int indexNum = 0; indexNum < playList.Length; indexNum++)
        {
            playList[indexNum].color = alphaZero;
        }
        yield return firstDealy; // 잠시대기
        // 재생부
        for (int indexNum = 0; indexNum < playList.Length; indexNum++)
        {
            playList[indexNum].gameObject.SetActive(true);
            AudioManager.inst.CartoonSoundPlay(index, indexNum);

            //페이드 진행
            while (timer < fadetime)
            {
                if (click) { break; }

                playList[indexNum].color = new Color(1, 1, 1, Mathf.Lerp(0, 1, timer / fadetime));
                timer += Time.deltaTime;
                yield return null;
            }

            playList[indexNum].color = Color.white;
            timer = 0;

            //마지막 장면일시 터치하여 넘기기 오픈
            if (indexNum == playList.Length - 1)
            {
                endTextRef.SetActive(true);
            }
            yield return nextDealy; // 잠시대기
        }


        // 클릭하면
        while (click == false)
        {
            yield return null;
        }

        if (index == 0)
        {
            Tutorial.inst.PlayTutorial(1, 1, 1, 1, 0);
        }

        AudioManager.inst.Set_VoulemMute("SFX", false);
        toons[index].SetActive(false);
        endTextRef.SetActive(false);
        cartoonRedf.SetActive(false);
        isPlaying = false;
    }



    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && toonRef.activeInHierarchy && click == false)
        {
            StartCoroutine(clickCheck());
        }

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    Cartoon_Active(3);
        //}
    }

    IEnumerator clickCheck()
    {
        click = true;
        yield return null;
        click = false;
    }
    private void Start()
    {

    }
}
