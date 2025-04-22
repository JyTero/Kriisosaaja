using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopShelfJuice : ShopShelfBase
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.JuiceDrinkAmount++;

    }

    public override void UndoBuy(PlayerResourceData playerResourceData)
    {
        base.UndoBuy(playerResourceData);
        playerResourceData.JuiceDrinkAmount--;
    }
}
