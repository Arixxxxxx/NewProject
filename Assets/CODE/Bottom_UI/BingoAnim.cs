using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoAnim : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Button btn;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickBtn()
    {
        animator.SetTrigger("click");
        btn.interactable = false;
    }

    public void SetActive()
    {
        gameObject.SetActive(false);
    }
}
