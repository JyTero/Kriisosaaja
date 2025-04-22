using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValues;

public class ShopShelfVegetables : ShopShelfBase
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.VegetableFoodAmount++;
        
    }

    public override void UndoBuy(PlayerResourceData playerResourceData)
    {
        base.UndoBuy(playerResourceData);
        playerResourceData.VegetableFoodAmount--;
    }
}
