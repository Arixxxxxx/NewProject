using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GoldShop : MonoBehaviour
{

    [SerializeField] List<Product> list_product = new List<Product>();
    [SerializeField] string price;
    [SerializeField] ProductTag priceType;
    [SerializeField] TMP_Text rewordText;
    [SerializeField] TMP_Text priceText;

    [Serializable]

    public class Product
    {
        [SerializeField] public ProductTag prodtag;
        [SerializeField] public Sprite sprite;
        [SerializeField] public string count;

        public void buyProduct()
        {
            switch (prodtag)
            {
                case ProductTag.Gold:
                    Debug.Log("��� " + count + "��ŭ ȹ��!");
                    break;
                case ProductTag.Ruby:
                    Debug.Log("��� " + count + "��ŭ ȹ��!");
                    break;
                case ProductTag.Star:
                    Debug.Log("�� " + count + "��ŭ ȹ��!");
                    break;
            }
        }
    }

    private void Start()
    {

    }

    public void ClickBuy()
    {
        ShopManager.Instance.SetCheckBuyActive(true);
        ShopManager.Instance.SetCheckBuy(list_product, price, priceType);
    }
}
