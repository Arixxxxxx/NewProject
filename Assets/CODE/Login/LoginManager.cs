using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public static LoginManager inst;

    //Ref
    GameObject worldRef, canvasRef;
    GameObject loginRef, taptoScrrenRef;
    Button googlePlayBtn, guestBtn;

    // 로그인관련
    GameObject loadingRef;
    Image loadingFillBar;
    int sceneNumber;


    // 닉네임설정
    GameObject guestInputFieldRef;
    TMP_InputField inputField;
    Button backBtn, accpectBtn;
    TMP_Text errorMsgText;
    TMP_Text loadingText;

    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        worldRef = GameObject.Find("World").gameObject;
        canvasRef = GameObject.Find("Canvas").gameObject;

        loginRef = canvasRef.transform.Find("SignUp").gameObject;
        taptoScrrenRef = canvasRef.transform.Find("TapToScreen").gameObject;
        guestInputFieldRef = canvasRef.transform.Find("GuestInputField").gameObject;

          googlePlayBtn = loginRef.transform.Find("GoogleBtn").GetComponent<Button>();
        guestBtn = loginRef.transform.Find("GuestBtn").GetComponent<Button>();

        loadingRef = canvasRef.transform.Find("Loading").gameObject;
        loadingFillBar = loadingRef.transform.Find("Bar/FillBar").GetComponent<Image>();
        loadingText = loadingRef.transform.Find("Bar/LoadingText").GetComponent<TMP_Text>();

        backBtn = guestInputFieldRef.transform.Find("BackBtn").GetComponent<Button>();
        accpectBtn = guestInputFieldRef.transform.Find("AcceptBtn").GetComponent<Button>();
        errorMsgText = guestInputFieldRef.transform.Find("Window/ErrorMsg").GetComponent<TMP_Text>();

        inputField = guestInputFieldRef.transform.Find("Window/TextBoxIMG/Input").GetComponent<TMP_InputField>();

    }
    void Start()
    {
        if (!taptoScrrenRef.gameObject.activeSelf)
        {
            taptoScrrenRef.SetActive(true);
        }

        BtnInit();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void BtnInit()
    {
        //구글플레이
        googlePlayBtn.onClick.AddListener(() =>
        {

        });

        //게스프 플레이
        guestBtn.onClick.AddListener(() =>
        {
            guestInputFieldRef.SetActive(true);
        });

        backBtn.onClick.AddListener(() =>
        {
            inputField.text = string.Empty;
            guestInputFieldRef.SetActive(false);
        });

        accpectBtn.onClick.AddListener(() =>
        {
            Create_Account_SetName(inputField.text);
        });

        inputField.onSubmit.AddListener((text) =>
        {
            Create_Account_SetName(inputField.text);
        });
    }



    //닉네임 셋업
    private void Create_Account_SetName(string inputText)
    {
        int count = inputText.Count();
        
        if (count < 2 || count > 6)
        {
            errorMsgText.text = $"최소 2자이상 6자 이내여야 합니다.";
            errorMsgText.gameObject.SetActive(true);
        }
        else if (IsHangulJamoOnly(inputText) == 0)
        {
            errorMsgText.text = $"한글과 영어를 혼합할 수 없습니다.";
            errorMsgText.gameObject.SetActive(true);
        }
        else if (IsHangulJamoOnly(inputText) == 1)
        {
            errorMsgText.text = $"완성형 한글을 사용해야 합니다.";
            errorMsgText.gameObject.SetActive(true);
        }
        else if (IsHangulJamoOnly(inputText) == 2)
        {
            errorMsgText.text = $"완성형 한글을 사용해야 합니다.";
            errorMsgText.gameObject.SetActive(true);
        }
        else if (IsHangulJamoOnly(inputText) == 3)
        {
            //창닫고 로딩
            DataManager.inst.Save_NewCreateAccount(inputText);
            inputField.text = string.Empty;
            loginRef.SetActive(false);
            guestInputFieldRef.SetActive(false);
            LoadScene(1);
        }
    }

    //닉네임 한글 완성형 체크
    private int IsHangulJamoOnly(string text)
    {
        bool containsHangul = false;
        bool containsEnglish = false;

        foreach (char c in text)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) // 영문자 범위 검사
            {
                if (containsHangul) return 0; // 한글이 이미 포함된 경우 false
                containsEnglish = true;
            }
            else if (c >= 0xAC00 && c <= 0xD7AF) // 완성형 한글 범위 검사
            {
                if (containsEnglish) return 1; // 영어가 이미 포함된 경우 false
                containsHangul = true;
            }
            else
            {
                return 2; // 영어나 한글이 아닌 다른 문자가 포함되면 false
            }
        }

        return 3; 
    }


    public void NextStep()
    {
        if (taptoScrrenRef.gameObject.activeSelf)
        {
            if (DataManager.inst.IshaveJsonFile == false)
            {
                taptoScrrenRef.SetActive(false);
                loginRef.SetActive(true);
            }
            else if(DataManager.inst.IshaveJsonFile == true)
            {
                taptoScrrenRef.SetActive(false);
                LoadScene(1);
            }
        }
    }
    private void LoadScene(int TargetSceneNumber)
    {
        sceneNumber = TargetSceneNumber;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        loadingRef.gameObject.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneNumber);
        op.allowSceneActivation = false;
        loadingFillBar.fillAmount = 0;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.1f)
            {
                if (loadingFillBar == null)
                {
                    loadingFillBar = GameObject.Find("Canvas/LoadingBar/FillBar").GetComponent<Image>();
                }

                loadingFillBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime * 0.5f;
                loadingFillBar.fillAmount = Mathf.Lerp(0.1f, 1f, timer);
                if (loadingFillBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }

            loadingText.text = $"Loading ({(int)(loadingFillBar.fillAmount * 100f)}%)";
        }
    }


}
