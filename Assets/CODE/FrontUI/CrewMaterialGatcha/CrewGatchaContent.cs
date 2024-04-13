using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewGatchaContent : MonoBehaviour
{
    public static CrewGatchaContent inst;

    //Ref
    GameObject frontUi, crewGatchaRef, window;
    Transform dynamicRef;

    //Prefabs
    [SerializeField] GameObject boxPrefabs;
    Queue<GameObject> boxGatcha = new Queue<GameObject>();


    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }

        frontUi = GameManager.inst.FrontUiRef;
        crewGatchaRef = frontUi.transform.Find("CrewMaterialGatcha").gameObject;
        window = crewGatchaRef.transform.Find("Window").gameObject;
        dynamicRef = window.transform.Find("Main/Box_Layout").GetComponent<Transform>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
