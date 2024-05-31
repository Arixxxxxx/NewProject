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
    [SerializeField] private Sprite[] stage1_Enemy;
    [SerializeField] private Sprite[] stage2_Enemy;
    [SerializeField] private Sprite[] stage3_Enemy;
    [Space]
    [SerializeField] private Sprite[] normal_relic_IMG;
    public Sprite[] Normal_Relic => normal_relic_IMG;

    [SerializeField] private Sprite[] epic_relic_IMG;
    public Sprite[] Epic_relic_IMG => epic_relic_IMG;

    [SerializeField] private Sprite[] legend_relic_IMG;
    public Sprite[] Legend_relic_IMG => legend_relic_IMG;



    /// <summary>
    /// 유물 스프라이트
    /// </summary>
    /// <param name="type"> 0 = Normal<br/> 1 = Epic <br/> 2 = Legend</param>
    /// <param name="number"></param>
    /// <returns></returns>
    public Sprite Relic_Sprite_TypeAndNumber(int type, int number)
    {
        switch (type)
        {
            case 0:
                return normal_relic_IMG[number];
            case 1:
                return epic_relic_IMG[number];
            case 2:
                return legend_relic_IMG[number];
        }
        return null;
    }

    int itemtype = 0;
    public Sprite Relic_SpriteNumber(int relicNum)
    {
        int itemtype = 0;

        // 정상적인 경우에 대한 relicNum 범위 확인
        if (relicNum >= 0 && relicNum < normal_relic_IMG.Length)
        {
            itemtype = 0;
        }
        else if (relicNum >= normal_relic_IMG.Length && relicNum < normal_relic_IMG.Length + epic_relic_IMG.Length)
        {
            itemtype = 1;
        }
        else if (relicNum >= normal_relic_IMG.Length + epic_relic_IMG.Length && relicNum < normal_relic_IMG.Length + epic_relic_IMG.Length + legend_relic_IMG.Length)
        {
            itemtype = 2;
        }
        else
        {
            // 범위를 벗어난 경우 null 반환
            return null;
        }

        switch (itemtype)
        {
            case 0:
                return normal_relic_IMG[relicNum];
            case 1:
                return epic_relic_IMG[relicNum - normal_relic_IMG.Length];
            case 2:
                return legend_relic_IMG[relicNum - normal_relic_IMG.Length - epic_relic_IMG.Length];
        }
        return null;
    }
    /// <summary>
    /// 에너미 스프라이트
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public Sprite[] enemySprite(int stage)
    {
        switch (stage)
        {
            case 1:
                return stage1_Enemy;
            case 2:
                return stage2_Enemy;
            case 3:
                return stage3_Enemy;
        }
        return null;
    }

    [Space]
    [SerializeField] private Sprite[] monsterLargeSize;
    public Sprite[] MonsterLargeSize { get { return monsterLargeSize; } }
    [Space]
    [Header("# PlayerCharactor Weapon Sprite")]
    [Space]
    [SerializeField] private Sprite[] weapons;
    public Sprite[] Weapons { get { return weapons; } }
    [Header("# 버프 아이콘")]
    [SerializeField] private Sprite[] buffIMG;
    [Space]
    [Header("# 재화 아이콘")]
    [SerializeField] private Sprite[] coinMG;
    [Header("# 동료 강화 재료 아이콘")]
    [SerializeField] private Sprite[] crewMaterialIMG;
    [Header("# Quest 아이콘")]
    [SerializeField] private Sprite[] questIcon;

    public Sprite Get_QuestIcon(int value) => questIcon[value];
    /// <summary>
    /// 버프 아이콘 스프라이트
    /// </summary>
    /// <param name="value"> 0공격력 / 1이속 / 2골드획득증가 / 3 쌘 공버프</param>
    /// <returns></returns>
    public Sprite BuffIMG(int value) => buffIMG[value];

    /// <summary>
    ///  재화 스프라이트
    /// </summary>
    /// <param name="value"> 0루비 / 1골드 / 2별 / 3도감 정수 </param>
    /// <returns></returns>
    public Sprite CoinIMG(int value) => coinMG[value];

    /// <summary>
    /// 동료 강화재료
    /// </summary>
    /// <param name="value"> 0 영혼 / 1 뼈 / 2책 / 3안쓰는 이미지</param>
    /// <returns></returns>
    public Sprite CrewMaterialIMG(int value) => crewMaterialIMG[value];
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

}
