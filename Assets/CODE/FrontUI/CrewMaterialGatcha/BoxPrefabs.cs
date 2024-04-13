using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxPrefabs : MonoBehaviour
{
    Button thisBtn;
    Animator boxAnim;
    private void Awake()
    {
        thisBtn = GetComponent<Button>();
        boxAnim = GetComponent<Animator>();
    }
    void Start()
    {
        thisBtn.onClick.AddListener(() => 
        {
            thisBtn.interactable = false;
            boxAnim.SetTrigger("Open"); 
        });  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
