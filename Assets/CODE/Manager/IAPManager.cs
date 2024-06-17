using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager inst;

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
    // Start is called before the first frame update
    void Start()
    {
        InitIAP();
    }

    private IStoreController stroeController;

    string ruby100 = "ruby_100";
    string ruby800 = "ruby_800";
    string ruby1500 = "ruby_1500";
    string ruby3000 = "ruby_3000";
    string adDelete = "ad_delete";

    private void InitIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(ruby100,ProductType.Consumable);
        builder.AddProduct(ruby800,ProductType.Consumable);
        builder.AddProduct(ruby1500,ProductType.Consumable);
        builder.AddProduct(ruby3000,ProductType.Consumable);
        builder.AddProduct(adDelete,ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    // �ڵ����� �ʱ�ȭ
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        stroeController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"�ʱ�ȭ ���� :  {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"�ʱ�ȭ ���� :  {error}");
    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"���� ���� ");
    }


    //���� ���������� �̷������ ��ȭ�� ��ɱ���
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;

        if (product.definition.id == ruby100)
        {
            GameStatus.inst.PlusRuby(100);
            WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "��� 100��");
        }
        else if (product.definition.id == ruby800)
        {
            GameStatus.inst.PlusRuby(800);
            WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "��� 800��");
        }
        else if (product.definition.id == ruby1500)
        {
            GameStatus.inst.PlusRuby(1500);
            WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "��� 1,500��");

        }
        else if (product.definition.id == ruby3000)
        {
            GameStatus.inst.PlusRuby(3000);
            WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "��� 3,000��");
        }
        else if (product.definition.id == adDelete)
        {
            AdDelete.inst.ADDelete_1MonthAdd();
            WorldUI_Manager.inst.Set_RewardUI_Invoke(SpriteResource.inst.CoinIMG(0), "���� ���� ���� �Ϸ�");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void Buy_Item(string productID)
    {
        stroeController.InitiatePurchase(productID);
    }

}
