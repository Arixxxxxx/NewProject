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

            // ���������θ� �巡�� �����ϰ� ����
            float newX = Mathf.Clamp(localPoint.x + dragOffset.x, minValue, maxValue);
            rectTrs.anchoredPosition = new Vector2(newX, rectTrs.anchoredPosition.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragStart = false;

        // ��� ���� ����: �����̴��� �ִ밪�� �������� ��
        if (rectTrs.anchoredPosition.x >= maxValue)
        {
            Unlock();
        }
        else
        {
            // �����̴��� �ִ밪�� �������� ������ ���� ��ġ�� �ε巴�� �ǵ���
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

        // �巡�� ���� ��, �ʱ�ȭ �ִϸ��̼��� ���� ���̸� ����
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
        Debug.Log("��� ������!");
        SleepMode.inst.Active_SleepMode(false, rectTrs);
    }

    private IEnumerator ResetPosition()
    {
        float duration = 0.1f; // �ִϸ��̼� ���� �ð�
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