using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public string ItemName;
    public string ItemDescription;
    public float ShopPrice;

    protected virtual void Start()
    {
       // ItemName = this.name;
    }

    public virtual void BuyItem(PlayerResourceData playerResourceData)
    {
        playerResourceData.MoneyAmount -= ShopPrice;
    }

    public virtual void UndoBuy (PlayerResourceData playerResourceData)
    {
        playerResourceData.MoneyAmount += ShopPrice;

    }
}

