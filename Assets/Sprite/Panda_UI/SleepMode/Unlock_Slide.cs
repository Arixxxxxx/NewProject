using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Unlock_Slide : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] float value;
    [SerializeField] bool dragStart = false;
    RectTransform rectTrs;

    float maxValue = 320f;
    float minValue = 0f;

    private Vector2 initialPosition;
    private Coroutine resetCoroutine;

    private Vector2 dragOffset;

    private void Awake()
    {
        rectTrs = GetComponent<RectTransform>();
    }

    private void Start()
    {
        initialPosition = rectTrs.anchoredPosition;
    }

    private void Update()
    {
        if (rectTrs != null)
        {
            value = rectTrs.anchoredPosition.x / maxValue;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragStart)
        {
            
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrs.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
            dragOffset = rectTrs.anchoredPosition - localPoint;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragStart)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrs.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

            // 오른쪽으로만 드래그 가능하게 제한
            float newX = Mathf.Clamp(localPoint.x + dragOffset.x, minValue, maxValue);
            rectTrs.anchoredPosition = new Vector2(newX, rectTrs.anchoredPosition.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragStart = false;

        // 잠금 해제 조건: 슬라이더가 최대값에 도달했을 때
        if (rectTrs.anchoredPosition.x >= maxValue)
        {
            Unlock();
        }
        else
        {
            // 슬라이더가 최대값에 도달하지 않으면 원래 위치로 부드럽게 되돌림
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }
            resetCoroutine = StartCoroutine(ResetPosition());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (dragStart == false)
        {
            dragStart = true;
            AudioManager.inst.SleepMode_SFX(11, 1f);
            SleepMode.inst.PandaIMGChanger(1);
        }

        // 드래그 시작 시, 초기화 애니메이션이 실행 중이면 중지
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(dragStart == true)
        {
            dragStart = false;
            SleepMode.inst.PandaIMGChanger(0);
        }
        
    }

    private void Unlock()
    {
        Debug.Log("잠금 해제됨!");
        SleepMode.inst.Active_SleepMode(false, rectTrs);
    }

    private IEnumerator ResetPosition()
    {
        float duration = 0.1f; // 애니메이션 지속 시간
        float elapsedTime = 0f;
        Vector2 startPosition = rectTrs.anchoredPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            rectTrs.anchoredPosition = Vector2.Lerp(startPosition, initialPosition, t);
            yield return null;
        }

        rectTrs.anchoredPosition = initialPosition;
    }
}