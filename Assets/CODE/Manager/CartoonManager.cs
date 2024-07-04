using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CartoonManager : MonoBehaviour
{
    public static CartoonManager inst;
    GameObject cartoonRedf, toonRef, endTextRef;
    GameObject[] toons; // ��ȭ�׸�

    //��ȭ ���
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

            switch (index) // ���Ŀ� ��ȭ�� �߰��Ǹ� ���⼭ ���
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
    /// ��ȭ ���
    /// </summary>
    /// <param name="index"> 0������ / 1����ũ / 2ȣ�� / 3��ȭ </param>
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

        switch (index) // ���Ŀ� ��ȭ�� �߰��Ǹ� ���⼭ ���
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

        // ����� �ʱ�ȭ
        for (int indexNum = 0; indexNum < playList.Length; indexNum++)
        {
            playList[indexNum].color = alphaZero;
        }
        yield return firstDealy; // ��ô��
        // �����
        for (int indexNum = 0; indexNum < playList.Length; indexNum++)
        {
            playList[indexNum].gameObject.SetActive(true);
            AudioManager.inst.CartoonSoundPlay(index, indexNum);

            //���̵� ����
            while (timer < fadetime)
            {
                if (click) { break; }

                playList[indexNum].color = new Color(1, 1, 1, Mathf.Lerp(0, 1, timer / fadetime));
                timer += Time.deltaTime;
                yield return null;
            }

            playList[indexNum].color = Color.white;
            timer = 0;

            //������ ����Ͻ� ��ġ�Ͽ� �ѱ�� ����
            if (indexNum == playList.Length - 1)
            {
                endTextRef.SetActive(true);
            }
            yield return nextDealy; // ��ô��
        }


        // Ŭ���ϸ�
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
