using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteResource : MonoBehaviour
{
    public static SpriteResource inst;

    [Header("# ���� ������")]
    [SerializeField] private Sprite[] buffIMG;
    [Space]
    [Header("# ��ȭ ������")]
    [SerializeField] private Sprite[] coinMG;
    /// <summary>
    /// ���� ������ ��������Ʈ
    /// </summary>
    /// <param name="value"> 0���ݷ� / 1�̼� / 2���ȹ������ / 3 �� ������</param>
    /// <returns></returns>
    public Sprite BuffIMG(int value) => buffIMG[value];

    /// <summary>
    ///  ��ȭ ��������Ʈ
    /// </summary>
    /// <param name="value"> 0��� / 1��� </param>
    /// <returns></returns>
    public Sprite CoinIMG(int value) => coinMG[value];
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
