using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyPrice : MonoBehaviour
{
    public static RubyPrice inst;


    [SerializeField] int[] buffRubyPrice = new int[3];
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
    /// <param name="value"> 0 ATK / 1 Gold / 2 Speed </param>
    /// <returns></returns>
    public int Get_buffRubyPrice(int value) => buffRubyPrice[value];

}
