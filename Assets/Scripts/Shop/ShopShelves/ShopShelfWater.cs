using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopShelfWater : ShopShelfBase
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.WaterDrinkAmount++;

    }

    public override void UndoBuy(PlayerResourceData playerResourceData)
    {
        base.UndoBuy(playerResourceData);
        playerResourceData.WaterDrinkAmount--;
    }
}
