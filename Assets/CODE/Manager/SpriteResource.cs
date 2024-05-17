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
    [Header("# PlayerCharactor Weapon Sprite")]
    [Space]
    [SerializeField] private Sprite[] weapons;
    public Sprite[] Weapons { get { return weapons; } }
    [Header("# ���� ������")]
    [SerializeField] private Sprite[] buffIMG;
    [Space]
    [Header("# ��ȭ ������")]
    [SerializeField] private Sprite[] coinMG;
    [Header("# ���� ��ȭ ��� ������")]
    [SerializeField] private Sprite[] crewMaterialIMG;
    [Header("# Quest ������")]
    [SerializeField] private Sprite[] questIcon;

    public Sprite Get_QuestIcon(int value) => questIcon[value];
    /// <summary>
    /// ���� ������ ��������Ʈ
    /// </summary>
    /// <param name="value"> 0���ݷ� / 1�̼� / 2���ȹ������ / 3 �� ������</param>
    /// <returns></returns>
    public Sprite BuffIMG(int value) => buffIMG[value];

    /// <summary>
    ///  ��ȭ ��������Ʈ
    /// </summary>
    /// <param name="value"> 0��� / 1��� / 2�� / 3���� ���� </param>
    /// <returns></returns>
    public Sprite CoinIMG(int value) => coinMG[value];

    /// <summary>
    /// ���� ��ȭ���
    /// </summary>
    /// <param name="value"> 0 ��ȥ / 1 �� / 2å / 3�Ⱦ��� �̹���</param>
    /// <returns></returns>
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
