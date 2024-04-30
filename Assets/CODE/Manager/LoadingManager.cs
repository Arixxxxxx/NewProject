using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    Image lodingSceneFillBar;

    private static int sceneNumber;

    public static void LoadScene(int TargetSceneNumber)
    {
        sceneNumber = TargetSceneNumber;
        SceneManager.LoadScene(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
            lodingSceneFillBar = GameObject.Find("Canvas/LoadingBar/FillBar").GetComponent<Image>();
            StartCoroutine(LoadScene());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadScene()
    {

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneNumber);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.1f)
            {
                if (lodingSceneFillBar == null)
                {
                    lodingSceneFillBar = GameObject.Find("Canvas/LoadingBar/FillBar").GetComponent<Image>();
                }

                lodingSceneFillBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime * 0.5f;
                lodingSceneFillBar.fillAmount = Mathf.Lerp(0.1f, 1f, timer);
                if (lodingSceneFillBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
