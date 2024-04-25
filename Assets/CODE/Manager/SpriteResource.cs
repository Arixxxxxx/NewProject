using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteResource : MonoBehaviour
{
    public static SpriteResource inst;

    [Header("# World Space Sprite")]
    [Space]
    [SerializeField] private Sprite[] map;
    public Sprite Map(int value) => map[value];
    [Space]
    [SerializeField] private Sprite[] monster;
    public Sprite[] MonsterSmall { get {  return monster; } }
    [Space]
    [SerializeField] private Sprite[] monsterLargeSize;
    public Sprite[] MonsterLargeSize { get { return monsterLargeSize; } }
    [Space]
    [Header("# 버프 아이콘")]
    [SerializeField] private Sprite[] buffIMG;
    [Space]
    [Header("# 재화 아이콘")]
    [SerializeField] private Sprite[] coinMG;
    [Header("# 동료 강화 재료 아이콘")]
    [SerializeField] private Sprite[] crewMaterialIMG;
    /// <summary>
    /// 버프 아이콘 스프라이트
    /// </summary>
    /// <param name="value"> 0공격력 / 1이속 / 2골드획득증가 / 3 쌘 공버프</param>
    /// <returns></returns>
    public Sprite BuffIMG(int value) => buffIMG[value];

    /// <summary>
    ///  재화 스프라이트
    /// </summary>
    /// <param name="value"> 0루비 / 1골드 </param>
    /// <returns></returns>
    public Sprite CoinIMG(int value) => coinMG[value];
    public Sprite CrewMaterialIMG(int value) => crewMaterialIMG[value];
    private void Awake()
    {
        if(inst == null)
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

}
