using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ScrrenTouchParticle : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject screenParticle;
    private Canvas touchParticleCanvas;
    Transform objTrs;
    Queue<ParticleSystem> Ps = new Queue<ParticleSystem>();
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        touchParticleCanvas = GetComponentInParent<Canvas>();
        objTrs = transform.GetChild(0);
        for (int i = 0; i < 10; i++)
        {
            prefabs_Maker();
        }
    }

    private void prefabs_Maker()
    {
        ParticleSystem PsObj = Instantiate(screenParticle, objTrs).GetComponent<ParticleSystem>();
        PsObj.gameObject.SetActive(false);
        Ps.Enqueue(PsObj);
    }

    private void PlayPs(Vector2 pos)
    {
        if (Ps.Count <= 0)
        {
            prefabs_Maker();
        }
        ParticleSystem PsObj = Ps.Dequeue();
        PsObj.transform.position = pos;
        PsObj.gameObject.SetActive(true);
        StartCoroutine(PlayRetur(PsObj));
    }

    WaitForSeconds waittime = new WaitForSeconds(0.5f);
    IEnumerator PlayRetur(ParticleSystem obj)
    {
        obj.Play();

        yield return waittime;

        obj.gameObject.SetActive(false);
        obj.transform.localPosition = Vector3.zero;
        Ps.Enqueue(obj);

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector2 pos = Input.mousePosition;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //PlayPs(eventData.position);
        //HandleAllRaycasts(eventData);
    }

    List<RaycastResult> raycastResults = new List<RaycastResult>();
    private void HandleAllRaycasts(PointerEventData eventData)
    {

        AudioManager.inst.Play_Ui_SFX(1, 0.1f);
        raycastResults.Clear();

        EventSystem.current.RaycastAll(eventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            for (int index = 0; index < raycastResults.Count; index++)
            {
                RaycastResult firstResult = raycastResults[index];
                if (firstResult.gameObject.name == "BG") { return; }

                ExecuteEvents.Execute(firstResult.gameObject, eventData, ExecuteEvents.pointerClickHandler);
  
            }
        }
    }

}




