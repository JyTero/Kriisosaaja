using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopShelfMeat : ShopShelfBase
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.MeatFoodAmount++;

    }

    public override void UndoBuy(PlayerResourceData playerResourceData)
    {
        base.UndoBuy(playerResourceData);
        playerResourceData.MeatFoodAmount--;
    }
}
