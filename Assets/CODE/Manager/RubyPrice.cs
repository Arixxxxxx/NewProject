using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyPrice : MonoBehaviour
{
    public static RubyPrice inst;

    [Header("#버프 가격")]
    [Space]
    [SerializeField] int[] buffRubyPrice = new int[3];
    [Header("#광고 제거 가격 ")]
    [Space]
    [SerializeField] int adDeletePrice;
    [Header("#환생 가격")]
    [Space]
    [SerializeField] int hwansengPrice;
    [Space]
    [Header("# 재료뽑기 가격표 = 3회 / 9회")]
    [SerializeField] int[] crewMaterialGachaPrice;
    public int CrewMaterialGachaPrice(int value) => crewMaterialGachaPrice[value];
    [Space]
    [Header("# 유물 뽑기 가격표 = 3회 / 9회")]
    [SerializeField] int[] relicGachaPrice;
    public int RelicGachaPrice(int value) => relicGachaPrice[value];

    public int HwansengPrice { get { return hwansengPrice; } }
    
    public int AdDeletePrice { get { return adDeletePrice; } }
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

    }
    void Start()
    {

    }

    /// <summary>
    /// 버프 루비 가격 가져오기
    /// </summary>
    /// <param name="value"> 0 ATK / 1 Speed / 2 Gold </param>
    /// <returns></returns>
    public int Get_buffRubyPrice(int value) => buffRubyPrice[value];


}
