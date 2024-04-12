using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GoldShop : MonoBehaviour
{
    [Header("ªÛ«∞ ∏Ò∑œ")]
    [SerializeField] List<Product> list_product = new List<Product>();
    [Header("ªÛ«∞ ∞°∞›")]
    [SerializeField] string price;
    [Header("∞°∞› ≈∏¿‘")]
    [SerializeField] ProductTag priceType;
    [Space]
    
    [SerializeField] TMP_Text priceText;
    [SerializeField] Transform imageParents;
    [SerializeField] GameObject obj_EmptyObj;
    [SerializeField] Image priceImage;
    List<Image> list_rewordImage = new List<Image>();
    List<TMP_Text> list_rewordText = new List<TMP_Text>();


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
                    GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(count));
                    Debug.Log("∞ÒµÂ " + count + "∏∏≈≠ »πµÊ!");
                    break;
                case ProductTag.Ruby:
                    GameStatus.inst.Ruby += int.Parse(count);
                    break;
                case ProductTag.Star:
                    GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(count));
                    Debug.Log("∫∞ " + count + "∏∏≈≠ »πµÊ!");
                    break;
            }
        }
    }

    private void Start()
    {
        //priceText.text = price;
        //priceImage.sprite = UIManager.Instance.GetProdSprite((int)priceType);
        //int prodCount = list_product.Count;
        //for (int iNum = 0; iNum < prodCount; iNum++)
        //{
        //    Instantiate(obj_EmptyObj, imageParents);
        //}

        //int imageCount = imageParents.childCount;
        //for (int iNum = 0; iNum < imageCount; iNum++)
        //{
        //    list_rewordImage.Add(imageParents.GetChild(iNum).GetComponent<Image>());
        //}

        //for (int iNum = 0; iNum < imageCount; iNum++)
        //{
        //    list_rewordText.Add(list_rewordImage[iNum].transform.GetChild(0).GetComponent<TMP_Text>());
        //}

        //for (int iNum = 0; iNum < imageCount; iNum++)
        //{
        //    list_rewordImage[iNum].sprite = list_product[iNum].sprite;
        //}

        //for (int iNum = 0; iNum < imageCount; iNum++)
        //{
        //    list_rewordText[iNum].text = list_product[iNum].count;
        //}
    }

    public void ClickBuy()
    {
        //ShopManager.Instance.SetCheckBuy(list_product, price, priceType);
    }
}
