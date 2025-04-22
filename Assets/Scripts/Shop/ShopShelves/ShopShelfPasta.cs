using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopShelfPasta : ShopShelfBase
{
    public override void BuyItem(PlayerResourceData playerResourceData)
    {
        base.BuyItem(playerResourceData);
        playerResourceData.PastaFoodAmount++;

    }

    public override void UndoBuy(PlayerResourceData playerResourceData)
    {
        base.UndoBuy(playerResourceData);
        playerResourceData.PastaFoodAmount--;
    }
}
