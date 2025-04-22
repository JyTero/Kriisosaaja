using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopShelfCannedFood : ShopShelfBase
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.CannedFoodAmount++;

    }

    public override void UndoBuy(PlayerResourceData playerResourceData)
    {
        base.UndoBuy(playerResourceData);
        playerResourceData.CannedFoodAmount--;
    }
}
