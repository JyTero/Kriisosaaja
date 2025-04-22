using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopShelfMilk : ShopShelfBase
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.MilkDrinkAmount++;

    }
    public override void UndoBuy(PlayerResourceData playerResourceData)
    {
        base.UndoBuy(playerResourceData);
        playerResourceData.MilkDrinkAmount--;

    }
}
