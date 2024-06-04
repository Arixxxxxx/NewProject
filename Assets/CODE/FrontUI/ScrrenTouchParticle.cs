using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ScrrenTouchParticle : MonoBehaviour
{
    [SerializeField] GameObject screenParticle;
    private RectTransform canvasRectTransform;
    private Canvas canvas;
    private Camera uiCamera;
    Queue<ParticleSystem> Ps = new Queue<ParticleSystem>();

    private void Awake()
    {
        canvasRectTransform = transform.parent.GetComponent<RectTransform>();
        canvas = transform.parent.GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;

        for (int i = 0; i < 10; i++)
        {
            prefabs_Maker();
        }
    }

    //프리펩 생성
    private void prefabs_Maker()
    {
        ParticleSystem PsObj = Instantiate(screenParticle, transform).GetComponent<ParticleSystem>();
        ParticleSystemRenderer renderer = PsObj.GetComponent<ParticleSystemRenderer>();
        renderer.enabled = true;
        renderer.sortingOrder = 100;
        PsObj.gameObject.SetActive(false);
        Ps.Enqueue(PsObj);
    }

    //재생 프리펩
    private void PlayPs(Vector2 localPosition)
    {
        if (Ps.Count <= 0)
        {
            prefabs_Maker();
        }
        AudioManager.inst.Play_Ui_SFX(1, 0.1f);
        ParticleSystem PsObj = Ps.Dequeue();
        PsObj.transform.localPosition = localPosition;
        PsObj.gameObject.SetActive(true);
        StartCoroutine(PlayReturn(PsObj));
    }

    WaitForSeconds waittime = new WaitForSeconds(0.5f);
    IEnumerator PlayReturn(ParticleSystem PsObj)
    {
        PsObj.Play();

        yield return waittime;

        PsObj.gameObject.SetActive(false);
        PsObj.transform.localPosition = Vector3.zero;
        Ps.Enqueue(PsObj);
    }

    List<RaycastResult> raycastResults = new List<RaycastResult>();
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.isActiveAndEnabled)
            {
                raycastResults.Clear();
                // 클릭을 감지한 이벤트시스템 가져오기
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                EventSystem.current.RaycastAll(pointerData, raycastResults);

                if (raycastResults.Count > 0)
                {
                    for (int index = 0; index < raycastResults.Count; index++)
                    {
                        if (raycastResults[index].gameObject.name == "Area")
                        {
                            Vector2 localPoint;
                            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                canvasRectTransform,
                                pointerData.position,
                                uiCamera,
                                out localPoint);

                            PlayPs(localPoint);
                        }
                    }
                }
            }
        }
    }

}




