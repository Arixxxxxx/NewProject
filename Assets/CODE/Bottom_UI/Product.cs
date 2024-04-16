using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Product : MonoBehaviour
{
    List<ProductList> list_product = new List<ProductList>();
    string price;
    ProductTag priceType;
    TMP_Text priceText;
    Transform imageParents;
    Image priceImage;
    List<Image> list_rewordImage = new List<Image>();
    List<TMP_Text> list_rewordText = new List<TMP_Text>();


    [Serializable]
    public class ProductList
    {
        [SerializeField] public ProductTag prodtag;
        [SerializeField] public string count;

        public void buyProduct()
        {
            switch (prodtag)
            {
                case ProductTag.Gold:
                    GameStatus.inst.PlusGold(CalCulator.inst.ConvertChartoIndex(count));
                    break;
                case ProductTag.Ruby:
                    GameStatus.inst.Ruby += int.Parse(count);
                    break;
                case ProductTag.Star:
                    GameStatus.inst.PlusStar(CalCulator.inst.ConvertChartoIndex(count));
                    break;
            }
        }

        public Sprite GetSprite()
        {
            switch (prodtag)
            {
                case ProductTag.Gold:
                    return UIManager.Instance.GetProdSprite(0);

                case ProductTag.Star:
                    return UIManager.Instance.GetProdSprite(1);

                case ProductTag.Ruby:
                    return UIManager.Instance.GetProdSprite(2);

            }
            return null;
        }
    }

    public void InitStart(List<ProductList> _list, string _price, ProductTag _priceType)
    {
        list_product = _list;
        price = _price;
        priceType = _priceType;
        priceText = transform.Find("PriceText").GetComponent<TMP_Text>();
        imageParents = transform.Find("ProductList");
        priceImage = transform.Find("priceImage").GetComponent<Image>();
        transform.GetComponent<Button>().onClick.AddListener(ClickBuy);

        priceText.text = _price;
        priceImage.sprite = UIManager.Instance.GetProdSprite((int)_priceType);
        int prodCount = list_product.Count;//상품 이미지 생성
        for (int iNum = 0; iNum < prodCount; iNum++)
        {
            GameObject obj = Instantiate(ShopManager.Instance.GetEmptyImage(), imageParents);
            list_rewordImage.Add(obj.transform.Find("Image").GetComponent<Image>());
            list_rewordText.Add(obj.transform.Find("RewordText").GetComponent<TMP_Text>());
            list_rewordImage[iNum].sprite = list_product[iNum].GetSprite();
            list_rewordText[iNum].text = list_product[iNum].count;
        }
    }

    public void ClickBuy()
    {
        ShopManager.Instance.SetCheckBuy(list_product, price, priceType);
    }
}
